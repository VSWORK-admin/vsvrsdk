using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class UsefulWindow : EditorWindow
{
    [MenuItem("搜索/UsefulWindowEditor", false, 0)]
    private static void AddWindow()
    {
        //创建窗口
        Rect wr = new Rect(0, 0, 500, 100);
        UsefulWindow window = (UsefulWindow)EditorWindow.GetWindowWithRect(typeof(UsefulWindow), wr, true, "UsefulWindowEditor");
        window.Show();
    }
    private static string m_textValue;
    private static Transform m_objectValue; //定义Object
    private void OnGUI()
    {
        GUILayout.BeginVertical(GUILayout.Height(200));
        GUILayout.BeginHorizontal();
        m_textValue = EditorGUILayout.TextField("类名：", m_textValue);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        m_objectValue = EditorGUILayout.ObjectField("物体：",m_objectValue, typeof(Transform), true) as Transform;
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("搜索热更脚本"))
        {
            SearchDllCom();
        }
        if (GUILayout.Button("搜索热更物体引用"))
        {
            SearchDllObject();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
    private static List<GameObject> listTargetItem = new List<GameObject>();
    private static void SearchDllCom()
    {
        if (!Application.isBatchMode && (EditorApplication.isCompiling || Application.isPlaying))
        {
            EditorUtility.DisplayDialog("警告", "你当前处于编译或者运行中，请等待编译完成或者停止运行", "了解");
            return;
        }
        listTargetItem.Clear();

        var nowScene = EditorSceneManager.GetActiveScene();

        var RootObjs = nowScene.GetRootGameObjects();

        for (int i = 0; i < RootObjs.Length; i++)
        {
            var Obj = RootObjs[i];

            if(Obj != null)
            {
                GetComInGo(Obj.transform, m_textValue);

                GetItemData(Obj.transform, m_textValue);
            }
        }

        if(listTargetItem.Count > 0)
        {
            Selection.objects = listTargetItem.ToArray();
        }
    }
    private static void SearchDllObject()
    {
        if (!Application.isBatchMode && (EditorApplication.isCompiling || Application.isPlaying))
        {
            EditorUtility.DisplayDialog("警告", "你当前处于编译或者运行中，请等待编译完成或者停止运行", "了解");
            return;
        }
        listTargetItem.Clear();

        var nowScene = EditorSceneManager.GetActiveScene();

        var RootObjs = nowScene.GetRootGameObjects();

        for (int i = 0; i < RootObjs.Length; i++)
        {
            var Obj = RootObjs[i];

            if (Obj != null)
            {
                GetItemTransformInGo(Obj.transform, m_objectValue);

                GetItemTransformData(Obj.transform, m_objectValue);
            }
        }

        if (listTargetItem.Count > 0)
        {
            Selection.objects = listTargetItem.ToArray();
            m_objectValue = null;
        }
    }
    private static void GetComInGo(Transform child,string info)
    {
        var be = child.GetComponent<GeneralDllBehavior>();
        if (be != null && be.ScriptClassName.Contains(info))
        {
            if (!listTargetItem.Contains(be.gameObject))
                listTargetItem.Add(be.gameObject);
        }
        var beAd = child.GetComponent<GeneralDllBehaviorAdapter>();
        if (beAd != null && beAd.ScriptClassName.Contains(info))
        {
            if (!listTargetItem.Contains(beAd.gameObject))
                listTargetItem.Add(beAd.gameObject);
        }

        var DragbeDll = child.GetComponent<DragDllBehavior>();
        if (DragbeDll != null && DragbeDll.ScriptClassName.Contains(info))
        {
            if (!listTargetItem.Contains(DragbeDll.gameObject))
                listTargetItem.Add(DragbeDll.gameObject);
        }
    }
    private static void GetItemTransformInGo(Transform child, Transform info)
    {
        var be = child.GetComponent<GeneralDllBehavior>();
        if (be != null)
        {
            for(int i = 0;i< be.ExtralDatas.Length;i++)
            {
                var item = be.ExtralDatas[i];
                if(item.Target != null && item.Target == info)
                {
                    if (!listTargetItem.Contains(be.gameObject))
                        listTargetItem.Add(be.gameObject);
                }
                if (item.Info.Length > 0)
                {
                    for (int j = 0; j < item.Info.Length; j++)
                    {
                        var itemJ = item.Info[j];
                        if (itemJ.Target != null && itemJ.Target == info)
                        {
                            if (!listTargetItem.Contains(be.gameObject))
                                listTargetItem.Add(be.gameObject);
                        }
                    }
                }
            }
        }
        var beAd = child.GetComponent<GeneralDllBehaviorAdapter>();
        if (beAd != null)
        {
            for (int i = 0; i < beAd.ExtralDatas.Length; i++)
            {
                var item = beAd.ExtralDatas[i];
                if (item.Target != null && item.Target == info)
                {
                    if (!listTargetItem.Contains(beAd.gameObject))
                        listTargetItem.Add(beAd.gameObject);
                }
                if (item.Info.Length > 0)
                {
                    for (int j = 0; j < item.Info.Length; j++)
                    {
                        var itemJ = item.Info[j];
                        if (itemJ.Target != null && itemJ.Target == info)
                        {
                            if (!listTargetItem.Contains(beAd.gameObject))
                                listTargetItem.Add(beAd.gameObject);
                        }
                    }
                }
            }
        }
        var DragbeDll = child.GetComponent<DragDllBehavior>();
        if (DragbeDll != null)
        {
            for (int i = 0; i < DragbeDll.ExtralDatas.Length; i++)
            {
                var item = DragbeDll.ExtralDatas[i];
                if (item.Target != null && item.Target == info)
                {
                    if (!listTargetItem.Contains(DragbeDll.gameObject))
                        listTargetItem.Add(DragbeDll.gameObject);
                }
                if (item.Info.Length > 0)
                {
                    for (int j = 0; j < item.Info.Length; j++)
                    {
                        var itemJ = item.Info[j];
                        if (itemJ.Target != null && itemJ.Target == info)
                        {
                            if (!listTargetItem.Contains(DragbeDll.gameObject))
                                listTargetItem.Add(DragbeDll.gameObject);
                        }
                    }
                }
            }
        }

        var hfInfo = child.GetComponent<HFExtralData>();
        if (hfInfo != null)
        {
            for (int i = 0; i < hfInfo.ExtralDatas.Length; i++)
            {
                var item = hfInfo.ExtralDatas[i];
                if (item.Target != null && item.Target == info)
                {
                    if (!listTargetItem.Contains(hfInfo.gameObject))
                        listTargetItem.Add(hfInfo.gameObject);
                }
                if (item.Info.Length > 0)
                {
                    for (int j = 0; j < item.Info.Length; j++)
                    {
                        var itemJ = item.Info[j];
                        if (itemJ.Target != null && itemJ.Target == info)
                        {
                            if (!listTargetItem.Contains(hfInfo.gameObject))
                                listTargetItem.Add(hfInfo.gameObject);
                        }
                    }
                }
            }
        }
    }
    private static void GetItemData(Transform trans,string info)
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            var child = trans.GetChild(i);

            if (child == null) continue;

            GetComInGo(child,info);

            GetItemData(child, info);
        }
    }

    private static void GetItemTransformData(Transform trans, Transform info)
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            var child = trans.GetChild(i);

            if (child == null) continue;

            GetItemTransformInGo(child, info);

            GetItemTransformData(child, info);
        }
    }
    [ExecuteInEditMode]
    [MenuItem("Tools/MaterialBindTexture")]
    private static void MaterialBindTexture()
    {
        string[] idMats = AssetDatabase.FindAssets("t:Material", new string[] { "Assets/test" });
        string[] idTexs = AssetDatabase.FindAssets("t:Texture2D", new string[] { "Assets/test/Texture" });

        for (int i = 0; i < idMats.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(idMats[i]);
            Debug.Log(path);
            string fileName = Path.GetFileNameWithoutExtension(path);

            Material material = AssetDatabase.LoadAssetAtPath(path, typeof(Material)) as Material;
            if (material == null) continue;

            for (int j = 0; j < idTexs.Length; j++)
            {
                string pathTex = AssetDatabase.GUIDToAssetPath(idTexs[j]);
                string fileNameTex = Path.GetFileNameWithoutExtension(pathTex);
                if (fileName.Equals(fileNameTex))
                {
                    Texture2D tex = AssetDatabase.LoadAssetAtPath(pathTex, typeof(Texture2D)) as Texture2D;
                    material.mainTexture = tex;
                }
            }
        }
        AssetDatabase.SaveAssets();
    }
}
