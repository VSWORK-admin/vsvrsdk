using System.Collections.Generic;
using Unity.Profiling.LowLevel;
using Unity.Profiling;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine;
using static GaussianSplatRenderer;

class GaussianSplatRenderSystem
{
    // ReSharper disable MemberCanBePrivate.Global - used by HDRP/URP features that are not always compiled
    internal static readonly ProfilerMarker s_ProfDraw = new(ProfilerCategory.Render, "GaussianSplat.Draw", MarkerFlags.SampleGPU);
    internal static readonly ProfilerMarker s_ProfCompose = new(ProfilerCategory.Render, "GaussianSplat.Compose", MarkerFlags.SampleGPU);
    internal static readonly ProfilerMarker s_ProfCalcView = new(ProfilerCategory.Render, "GaussianSplat.CalcView", MarkerFlags.SampleGPU);
    // ReSharper restore MemberCanBePrivate.Global

    public static GaussianSplatRenderSystem instance => ms_Instance ??= new GaussianSplatRenderSystem();
    static GaussianSplatRenderSystem ms_Instance;

    readonly Dictionary<GaussianSplatRenderer, MaterialPropertyBlock> m_Splats = new();
    readonly HashSet<Camera> m_CameraCommandBuffersDone = new();
    readonly List<(GaussianSplatRenderer, MaterialPropertyBlock)> m_ActiveSplats = new();

    public Vector2Int resolution = Vector2Int.one;
    public float texFactor = 1.0f;
    CommandBuffer m_CommandBuffer;

    public void RegisterSplat(GaussianSplatRenderer r)
    {
        if (m_Splats.Count == 0)
        {
            if (GraphicsSettings.currentRenderPipeline == null)
            {
                Camera.onPreCull += OnPreCullCamera;
                Camera.onPreRender += OnPreRender;
            }
        }

        m_Splats.Add(r, new MaterialPropertyBlock());
    }

    public void UnregisterSplat(GaussianSplatRenderer r)
    {
        if (!m_Splats.ContainsKey(r))
            return;
        m_Splats.Remove(r);
        if (m_Splats.Count == 0)
        {
            if (m_CameraCommandBuffersDone != null)
            {
                if (m_CommandBuffer != null)
                {
                    foreach (var cam in m_CameraCommandBuffersDone)
                    {
                        if (cam)
                            cam.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, m_CommandBuffer);
                    }
                }
                m_CameraCommandBuffersDone.Clear();
            }

            m_ActiveSplats.Clear();
            m_CommandBuffer?.Dispose();
            m_CommandBuffer = null;
            Camera.onPreCull -= OnPreCullCamera;
            Camera.onPreRender -= OnPreRender;
        }
    }

    // ReSharper disable once MemberCanBePrivate.Global - used by HDRP/URP features that are not always compiled
    public bool GatherSplatsForCamera(Camera cam)
    {
        if (cam.cameraType == CameraType.Preview)
            return false;
        // gather all active & valid splat objects
        m_ActiveSplats.Clear();
        foreach (var kvp in m_Splats)
        {
            var gs = kvp.Key;
            if (gs == null || !gs.isActiveAndEnabled /*|| !gs.HasValidAsset || !gs.HasValidRenderSetup*/)
                continue;
            m_ActiveSplats.Add((kvp.Key, kvp.Value));
        }
        if (m_ActiveSplats.Count == 0)
            return false;

        // sort them by depth from camera
        var camTr = cam.transform;
        m_ActiveSplats.Sort((a, b) =>
        {
            var trA = a.Item1.transform;
            var trB = b.Item1.transform;
            var posA = camTr.InverseTransformPoint(trA.position);
            var posB = camTr.InverseTransformPoint(trB.position);
            return posA.z.CompareTo(posB.z);
        });

        return true;
    }
    public void RenderSplatsDepth(Camera cam, CommandBuffer cmb, RenderTargetIdentifier source)
    {
        foreach (var kvp in m_ActiveSplats)
        {
            var gs = kvp.Item1;

            //Grab depth texture
            if (gs.depthMaterial != null)
            {
                for (int i = 0; i < (gs.isXr ? 2 : 1); ++i)
                {
                    gs.depthMaterial.SetFloat("_Scale", 1.0f);
                    gs.depthMaterial.SetInt("_Eye", i);

                    if (gs.camDepthRTex != null && gs.camDepthRTex.Length > 0 && gs.camDepthTex.Length > 0)
                    {
                        cmb.Blit(source, gs.camDepthRTex[i], gs.depthMaterial);
                        cmb.CopyTexture(gs.camDepthRTex[i], gs.camDepthTex[i]);
                    }
                }
            }

        }
    }
    // ReSharper disable once MemberCanBePrivate.Global - used by HDRP/URP features that are not always compiled
    public Material SortAndRenderSplats(Camera cam, CommandBuffer cmb,ref bool bRenderGS)
    {
        if(m_ActiveSplats.Count <= 0)
        {
            bRenderGS = false;
            return null;
        }

        Material matComposite = null;
        foreach (var kvp in m_ActiveSplats)
        {
            var gs = kvp.Item1;
            //gs.cam = cam;
            matComposite = gs.renderMaterial;
            var mpb = kvp.Item2;
            // sort
            var matrix = gs.transform.localToWorldMatrix;
            
            //Do nothing until initialization is done.
            if (gs.pov[0] <= 0 || gs.state != GaussianSplatRenderer.State.RENDERING || gs.renderMaterial == null)
            {
                bRenderGS = false;
                continue;
            }

            if (!gs.WaitPovPreprocessed(gs.pov[0]))
            {
                bRenderGS = false;
                continue;
            }

            if (gs.isXr && !gs.WaitPovPreprocessed(gs.pov[1]))
            {
                bRenderGS = false;
                continue;
            }

            gs.SendDrawEvent();

            if (!gs.WaitPovDrawn(gs.pov[0]))
            {
                bRenderGS = false;
                continue;
            }

            if (gs.isXr && !gs.WaitPovDrawn(gs.pov[1]))
            {
                bRenderGS = false;
                continue;
            }
            matComposite.SetFloat("_Scale", 1.0f);
            //matComposite.SetTexture("_GaussianSplattingTexRightEye", gs.texOld);
            for (int i = 0; i < (gs.isXr ? 2 : 1); ++i)
            {         
                matComposite.SetTexture(i == 0 ? "_GaussianSplattingTexLeftEye" : "_GaussianSplattingTexRightEye", gs.tex[i]);
                matComposite.SetTexture(i == 0 ? "_GaussianSplattingDepthTexLeftEye" : "_GaussianSplattingDepthTexRightEye", gs.depthTex[i]);
            }
            bRenderGS = true;
        }
        return matComposite;
    }

    // ReSharper disable once MemberCanBePrivate.Global - used by HDRP/URP features that are not always compiled
    // ReSharper disable once UnusedMethodReturnValue.Global - used by HDRP/URP features that are not always compiled
    public CommandBuffer InitialClearCmdBuffer(Camera cam)
    {
        m_CommandBuffer ??= new CommandBuffer { name = "RenderGaussianSplats" };
        if (GraphicsSettings.currentRenderPipeline == null && cam != null && !m_CameraCommandBuffersDone.Contains(cam))
        {
            //cam.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, m_CommandBuffer);
            cam.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, m_CommandBuffer);
            m_CameraCommandBuffersDone.Add(cam);
        }

        // get render target for all splats
        m_CommandBuffer.Clear();
        return m_CommandBuffer;
    }

    void OnPreCullCamera(Camera cam)
    {
        if (!GatherSplatsForCamera(cam))
            return;

        InitialClearCmdBuffer(cam);

        m_CommandBuffer.GetTemporaryRT(GaussianSplatRenderer.Props.GaussianSplatRT, -1, -1, 0, FilterMode.Point, GraphicsFormat.R16G16B16A16_SFloat);
        m_CommandBuffer.SetRenderTarget(GaussianSplatRenderer.Props.GaussianSplatRT, BuiltinRenderTextureType.CurrentActive);
        m_CommandBuffer.ClearRenderTarget(RTClearFlags.Color, new Color(0, 0, 0, 0), 0, 0);
        bool bRenderGS = false;
        // add sorting, view calc and drawing commands for each splat object
        Material matComposite = SortAndRenderSplats(cam, m_CommandBuffer,ref bRenderGS);

        // compose
        m_CommandBuffer.BeginSample(s_ProfCompose.ToString());
        m_CommandBuffer.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
        m_CommandBuffer.DrawProcedural(Matrix4x4.identity, matComposite, 0, MeshTopology.Triangles, 3, 1);
        m_CommandBuffer.EndSample(s_ProfCompose.ToString());
        m_CommandBuffer.ReleaseTemporaryRT(GaussianSplatRenderer.Props.GaussianSplatRT);
    }

    public void OnPreRender(Camera cam)
    {
        foreach (var kvp in m_ActiveSplats)
        {
            var gs = kvp.Item1;
//#if UNITY_EDITOR
            gs.cam = cam;
//#endif
            //Do nothing until initialization is done.
            if (gs.pov[0] <= 0 || gs.renderMaterial == null || (gs.state != GaussianSplatRenderer.State.RENDERING && gs.state != GaussianSplatRenderer.State.PAUSE)) { return; }

            bool doit = true;

            if (gs.isXr)
            {
                if (GaussianSplatRenderer.TryGetEyesPoses(out Vector3 lpos, out Vector3 rpos, out Quaternion lrot, out Quaternion rrot))
                {
                    if (gs.real_leye == null) { gs.real_leye = new GameObject("real leye"); gs.real_leye.transform.parent = cam.transform.parent; }
                    gs.real_leye.transform.localPosition = lpos;
                    gs.real_leye.transform.localRotation = lrot;

                    if (gs.real_reye == null) { gs.real_reye = new GameObject("real reye"); gs.real_reye.transform.parent = cam.transform.parent; }
                    gs.real_reye.transform.localPosition = rpos;
                    gs.real_reye.transform.localRotation = rrot;
                }
                else
                {
                    doit = false;
                }
            }

            if (doit)
            {
                for (int i = 0; i < (gs.isXr ? 2 : 1); ++i)
                {
                    if (gs.tex != null && gs.tex[i] != null)
                    {
                        float fovy = cam.fieldOfView * Mathf.PI / 180;
                        Matrix4x4 proj_mat = cam.projectionMatrix;
                        Vector3 cam_pos = cam.transform.position;
                        Quaternion cam_rot = cam.transform.rotation;

                        if (gs.isXr)
                        {
                            if (i == 0)
                            {
                                proj_mat = cam.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
                                cam_pos = gs.real_leye.transform.position;
                                cam_rot = gs.real_leye.transform.rotation;
                            }
                            else
                            {
                                proj_mat = cam.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
                                cam_pos = gs.real_reye.transform.position;
                                cam_rot = gs.real_reye.transform.rotation;
                            }
                        }

                        gs.PreProcessPass(gs.pov[i], cam_pos, cam_rot, proj_mat, fovy);
                    }
                }

                gs.SendPreprocessEvent();
            }

            lock (gs.registeredModels)
            {
                foreach (RegisteredModel m in gs.registeredModels)
                {
                    if (m.model != null && m.currentCropBox.size != Vector3.zero && m.currentCropBox != m.model.cropBox)
                    {
                        float[] min = { m.model.cropBox.min.x, m.model.cropBox.min.y, m.model.cropBox.min.z };
                        float[] max = { m.model.cropBox.max.x, m.model.cropBox.max.y, m.model.cropBox.max.z };
                        Native.SetModelCrop(m.modelId, min, max);
                        m.currentCropBox = m.model.cropBox;
                    }
                }
            }
        }
    }
}