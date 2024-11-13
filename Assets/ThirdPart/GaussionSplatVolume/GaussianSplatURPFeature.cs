// SPDX-License-Identifier: MIT
//#if GS_ENABLE_URP

using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static Unity.Burst.Intrinsics.X86.Avx;

namespace GaussianSplatting.Runtime
{
    // Note: I have no idea what is the purpose of ScriptableRendererFeature vs ScriptableRenderPass, which one of those
    // is supposed to do resource management vs logic, etc. etc. Code below "seems to work" but I'm just fumbling along,
    // without understanding any of it.
    //
    // ReSharper disable once InconsistentNaming
    class GaussianSplatURPFeature : ScriptableRendererFeature
    {
        class GSRenderPass : ScriptableRenderPass
        {
            //RTHandle m_RenderTarget;
            internal ScriptableRenderer m_Renderer = null;
            //internal RenderTargetHandle destination;
            internal CommandBuffer m_Cmb = null;

            public void Dispose()
            {
                //m_RenderTarget?.Release();
            }

            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                //RenderTextureDescriptor rtDesc = renderingData.cameraData.cameraTargetDescriptor;
                //rtDesc.depthBufferBits = 0;
                //rtDesc.msaaSamples = 1;
                //rtDesc.graphicsFormat = GraphicsFormat.R16G16B16A16_SFloat;
                //if (m_RenderTarget == null)
                //{
                //    m_RenderTarget = RTHandles.Alloc(rtDesc.width, rtDesc.height, 1, (DepthBits)rtDesc.depthBufferBits, rtDesc.graphicsFormat
                //        , FilterMode.Point, TextureWrapMode.Clamp, name: "_GaussianSplatRT");
                //}
                ////RenderingUtils.ReAllocateIfNeeded(ref m_RenderTarget, rtDesc, FilterMode.Point, TextureWrapMode.Clamp, name: "_GaussianSplatRT");
                //cmd.SetGlobalTexture(m_RenderTarget.name, m_RenderTarget.nameID);

                //ConfigureTarget(m_RenderTarget, m_Renderer.cameraColorTarget);
                //ConfigureClear(ClearFlag.Color, new Color(0, 0, 0, 0));
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (m_Cmb == null)
                    return;
                //GaussianSplatRenderSystem.instance.RenderSplatsDepth(renderingData.cameraData.camera, m_Cmb, m_RenderTarget);
                GaussianSplatRenderSystem.instance.RenderSplatsDepth(renderingData.cameraData.camera, m_Cmb, m_Renderer.cameraDepthTarget);
                bool bRenderGS = false;
                // add sorting, view calc and drawing commands for each splat object
                Material matComposite = GaussianSplatRenderSystem.instance.SortAndRenderSplats(renderingData.cameraData.camera, m_Cmb,ref bRenderGS);

                // compose
                //m_Cmb.BeginSample(GaussianSplatRenderSystem.s_ProfCompose.ToString());
                if (bRenderGS)
                {
                    //m_Cmb.Blit(m_RenderTarget, m_Renderer.cameraColorTarget, matComposite, 0);
                    //m_Cmb.Blit(m_Renderer.cameraColorTarget, destination.Identifier(), matComposite,0);
                    m_Cmb.Blit(m_Renderer.cameraColorTarget, m_Renderer.cameraColorTarget, matComposite,0);
                }
                //else
                //{
                //    m_Cmb.Blit(m_Renderer.cameraColorTarget, destination.Identifier());
                //}
                //Blitter.BlitCameraTexture(m_Cmb, m_RenderTarget, m_Renderer.cameraColorTarget, matComposite, 0);
                //m_Cmb.EndSample(GaussianSplatRenderSystem.s_ProfCompose.ToString());
                context.ExecuteCommandBuffer(m_Cmb);
            }
        }

        GSRenderPass m_Pass;
        bool m_HasCamera;

        public override void Create()
        {
            m_Pass = new GSRenderPass
            {
                renderPassEvent = RenderPassEvent.BeforeRenderingTransparents
            };
        }

        public override void OnCameraPreCull(ScriptableRenderer renderer, in CameraData cameraData)
        {
            m_HasCamera = false;
            var system = GaussianSplatRenderSystem.instance;
            if (!system.GatherSplatsForCamera(cameraData.camera))
                return;

            CommandBuffer cmb = system.InitialClearCmdBuffer(cameraData.camera);
            m_Pass.m_Cmb = cmb;
            system.OnPreRender(cameraData.camera);
            m_HasCamera = true;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (!m_HasCamera)
                return;
            //RenderTargetHandle destination = new RenderTargetHandle(renderer.cameraColorTarget);
            m_Pass.m_Renderer = renderer;
            //m_Pass.destination = destination;
            renderer.EnqueuePass(m_Pass);
        }

        protected override void Dispose(bool disposing)
        {
            m_Pass?.Dispose();
            m_Pass = null;
        }
    }
}

//#endif // #if GS_ENABLE_URP
