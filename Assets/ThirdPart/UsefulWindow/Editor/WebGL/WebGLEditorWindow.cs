using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using static PlasticGui.LaunchDiffParameters;
using UnityEditor.UIElements;

public class WebGLEditorWindow : EditorWindow
{
    [MenuItem("WebGL/ChangeFont", false, 0)]
    private static void AddWindow()
    {
        //创建窗口
        Rect wr = new Rect(0, 0, 300, 200);
        WebGLEditorWindow window = (WebGLEditorWindow)EditorWindow.GetWindowWithRect(typeof(WebGLEditorWindow), wr, true, "WebGLEditorWindow");
        window.Show();
    }
    public static void ChangeFont(Font font)
    {
        var nowScene = EditorSceneManager.GetActiveScene();

        if (!Application.isBatchMode)
        {
            if (nowScene.isDirty)
            {
                if (!EditorUtility.DisplayDialog("警告", "你是否需要保存当前场景？", "保存", "取消"))
                {
                    return;
                }
                EditorSceneManager.SaveScene(nowScene);
            }
        }

        EditorSceneManager.MarkSceneDirty(nowScene);

        var RootObjs = nowScene.GetRootGameObjects();
        for (int i = 0; i < RootObjs.Length; i++)
        {
            var child = RootObjs[i];
            AddComFunc(child.transform, font);
            ChangeFontFunction(child.transform,font);
        }
        EditorSceneManager.SaveScene(nowScene);
    }
    public static void RemoveWebGLInputCom()
    {
        var nowScene = EditorSceneManager.GetActiveScene();

        if (!Application.isBatchMode)
        {
            if (nowScene.isDirty)
            {
                if (!EditorUtility.DisplayDialog("警告", "你是否需要保存当前场景？", "保存", "取消"))
                {
                    return;
                }
                EditorSceneManager.SaveScene(nowScene);
            }
        }

        EditorSceneManager.MarkSceneDirty(nowScene);

        var RootObjs = nowScene.GetRootGameObjects();
        for (int i = 0; i < RootObjs.Length; i++)
        {
            var child = RootObjs[i];
            RemoveComFunc(child.transform);
            RemoveWebGLInputFunction(child.transform);
        }
        EditorSceneManager.SaveScene(nowScene);
    }
    public static void ChangePrefabFont(Font font, GameObject go)
    {
        //string[] guids = AssetDatabase.FindAssets("t:Prefab", path);
        //string _prefabPath = "";
        //GameObject _prefab;
        //foreach (var _guid in guids)
        //{
        //    _prefabPath = AssetDatabase.GUIDToAssetPath(_guid);
        //    GameObject _prefab = AssetDatabase.LoadAssetAtPath(_prefabPath, typeof(GameObject)) as GameObject;
        //    GameObject newPrefab = PrefabUtility.InstantiatePrefab(_prefab) as GameObject;
        //    ChangeFontFunction(newPrefab.transform, font);
        //    PrefabUtility.SaveAsPrefabAsset(newPrefab, _prefabPath, out bool isSuccess);
        //    DestroyImmediate(newPrefab);
        //}
        string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go); ;
        GameObject newPrefab = PrefabUtility.InstantiatePrefab(go) as GameObject;
        ChangeFontFunction(newPrefab.transform, font);
        PrefabUtility.SaveAsPrefabAsset(newPrefab, prefabPath, out bool isSuccess);
        DestroyImmediate(newPrefab);
    }
    private static void AddComFunc(Transform child, Font font)
    {
        Text textCom = child.GetComponent<Text>();
        if (textCom != null)
        {
            textCom.font = font;
        }
        InputField inputF = child.GetComponent<InputField>();
        if (inputF != null)
        {
            var webglInput = inputF.transform.GetComponent<WebGLSupport.WebGLInput>();
            if (webglInput == null)
            {
                inputF.gameObject.AddComponent<WebGLSupport.WebGLInput>();
            }
        }
        TMPro.TMP_InputField inputTmpF = child.GetComponent<TMPro.TMP_InputField>();
        if (inputTmpF != null)
        {
            var webglInput = inputTmpF.transform.GetComponent<WebGLSupport.WebGLInput>();
            if (webglInput == null)
            {
                inputTmpF.gameObject.AddComponent<WebGLSupport.WebGLInput>();
            }
        }
    }
    private static void ChangeFontFunction(Transform trans,Font font)
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            var child = trans.GetChild(i);
            if (child == null) continue;

            AddComFunc(child,font);

            ChangeFontFunction(child, font);
        }
    }
    private static void RemoveComFunc(Transform child)
    {
        InputField inputF = child.GetComponent<InputField>();
        if (inputF != null)
        {
            var webglInput = inputF.transform.GetComponent<WebGLSupport.WebGLInput>();
            if (webglInput != null)
            {
                DestroyImmediate(webglInput);
            }
        }
        TMPro.TMP_InputField inputTmpF = child.GetComponent<TMPro.TMP_InputField>();
        if (inputTmpF != null)
        {
            var webglInput = inputTmpF.transform.GetComponent<WebGLSupport.WebGLInput>();
            if (webglInput == null)
            {
                DestroyImmediate(webglInput);
            }
        }
    }
    public static void RemoveWebGLInputFunction(Transform trans)
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            var child = trans.GetChild(i);
            if (child == null) continue;

            RemoveComFunc(child);

            RemoveWebGLInputFunction(child);
        }
    }

    private Font FontObj;   //
    // Font NewFontObj;   //
    private GameObject prefab;
    void OnGUI()
    {
        FontObj = (Font)EditorGUILayout.ObjectField(FontObj, typeof(Font), false);

        if (GUILayout.Button("替换场景字体", GUILayout.Width(200)))
        {
            //if (FontObj != null && FontObj != NewFontObj)
            {
                ChangeFont(FontObj);
                //NewFontObj = FontObj;
                //打开通知栏
                this.ShowNotification(new GUIContent("操作完成"));
            }
            //else
            //{
            //    //打开通知栏
            //    this.ShowNotification(new GUIContent("字体没变"));
            //}
        }
        prefab =(GameObject) EditorGUILayout.ObjectField(prefab,typeof(GameObject),true);
        if (GUILayout.Button("替换预制体字体", GUILayout.Width(200)))
        {
            //if (FontObj != null && FontObj != NewFontObj)
            {
                //NewFontObj = FontObj;
                ChangePrefabFont(FontObj,prefab);
                //打开通知栏
                this.ShowNotification(new GUIContent("操作完成"));
            }
            //else
            //{
            //    //打开通知栏
            //    this.ShowNotification(new GUIContent("字体没变"));
            //}
        }
        GUILayout.BeginVertical(GUILayout.Height(200));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("去掉中文输入组件", GUILayout.Width(200)))
        {
            RemoveWebGLInputCom();
            //打开通知栏
            this.ShowNotification(new GUIContent("操作完成"));
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}