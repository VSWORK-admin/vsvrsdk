﻿#if UNITY_2018_2_OR_NEWER
#define TMP_WEBGL_SUPPORT
#endif

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using AOT;
using System.Runtime.InteropServices; // for DllImport
using System.Collections;
using UnityEngine.EventSystems;

namespace WebGLSupport
{
    internal class WebGLInputPlugin
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern void WebGLInputInit();
        [DllImport("__Internal")]
        public static extern int WebGLInputCreate(string canvasId, int x, int y, int width, int height, int fontsize, string text, string placeholder, bool isMultiLine, bool isPassword, bool isHidden, bool isMobile);

        [DllImport("__Internal")]
        public static extern void WebGLInputEnterSubmit(int id, bool flag);

        [DllImport("__Internal")]
        public static extern void WebGLInputTab(int id, Action<int, int> cb);

        [DllImport("__Internal")]
        public static extern void WebGLInputFocus(int id);

        [DllImport("__Internal")]
        public static extern void WebGLInputOnFocus(int id, Action<int> cb);

        [DllImport("__Internal")]
        public static extern void WebGLInputOnBlur(int id, Action<int> cb);

        [DllImport("__Internal")]
        public static extern void WebGLInputOnValueChange(int id, Action<int, string> cb);
        
        [DllImport("__Internal")]
        public static extern void WebGLInputOnEditEnd(int id, Action<int, string> cb);

        [DllImport("__Internal")]
        public static extern int WebGLInputSelectionStart(int id);

        [DllImport("__Internal")]
        public static extern int WebGLInputSelectionEnd(int id);

        [DllImport("__Internal")]
        public static extern int WebGLInputSelectionDirection(int id);

        [DllImport("__Internal")]
        public static extern void WebGLInputSetSelectionRange(int id, int start, int end);

        [DllImport("__Internal")]
        public static extern void WebGLInputMaxLength(int id, int maxlength);

        [DllImport("__Internal")]
        public static extern void WebGLInputText(int id, string text);

        [DllImport("__Internal")]
        public static extern bool WebGLInputIsFocus(int id);

        [DllImport("__Internal")]
        public static extern void WebGLInputDelete(int id);

        [DllImport("__Internal")]
        public static extern void WebGLInputForceBlur(int id);

#if WEBGLINPUT_TAB
        [DllImport("__Internal")]
        public static extern void WebGLInputEnableTabText(int id, bool enable);
#endif
#else
        public static void WebGLInputInit() {}
        public static int WebGLInputCreate(string canvasId, int x, int y, int width, int height, int fontsize, string text, string placeholder, bool isMultiLine, bool isPassword, bool isHidden, bool isMobile) { return 0; }
        public static void WebGLInputEnterSubmit(int id, bool flag) { }
        public static void WebGLInputTab(int id, Action<int, int> cb) { }
        public static void WebGLInputFocus(int id) { }
        public static void WebGLInputOnFocus(int id, Action<int> cb) { }
        public static void WebGLInputOnBlur(int id, Action<int> cb) { }
        public static void WebGLInputOnValueChange(int id, Action<int, string> cb) { }
        public static void WebGLInputOnEditEnd(int id, Action<int, string> cb) { }
        public static int WebGLInputSelectionStart(int id) { return 0; }
        public static int WebGLInputSelectionEnd(int id) { return 0; }
        public static int WebGLInputSelectionDirection(int id) { return 0; }
        public static void WebGLInputSetSelectionRange(int id, int start, int end) { }
        public static void WebGLInputMaxLength(int id, int maxlength) { }
        public static void WebGLInputText(int id, string text) { }
        public static bool WebGLInputIsFocus(int id) { return false; }
        public static void WebGLInputDelete(int id) { }
        public static void WebGLInputForceBlur(int id) { }

#if WEBGLINPUT_TAB
        public static void WebGLInputEnableTabText(int id, bool enable) { }
#endif

#endif
    }

    public class WebGLInput : MonoBehaviour, IComparable<WebGLInput>
    {
        static Dictionary<int, WebGLInput> instances = new Dictionary<int, WebGLInput>();
        public static string CanvasId { get; set; }

#if WEBGLINPUT_TAB
        public bool enableTabText = false;
#endif

        static WebGLInput()
        {
#if UNITY_2020_1_OR_NEWER
            WebGLInput.CanvasId = "unity-container";
#elif UNITY_2019_1_OR_NEWER
            WebGLInput.CanvasId = "unityContainer";//
#else
            WebGLInput.CanvasId = "gameContainer";
#endif
            WebGLInputPlugin.WebGLInputInit();
        }
        public int Id { get { return id; } }
        internal int id = -1;
        public IInputField input;
        bool blurBlock = false;

        [TooltipAttribute("show input element on canvas. this will make you select text by drag.")]
        public bool showHtmlElement = false;

        private IInputField Setup()
        {
            if (GetComponent<InputField>()) return new WrappedInputField(GetComponent<InputField>());
#if TMP_WEBGL_SUPPORT
            if (GetComponent<TMPro.TMP_InputField>()) return new WrappedTMPInputField(GetComponent<TMPro.TMP_InputField>());
#endif // TMP_WEBGL_SUPPORT
            throw new Exception("Can not Setup WebGLInput!!");
        }

        private void Awake()
        {
            input = Setup();
#if !(UNITY_WEBGL && !UNITY_EDITOR)
            // WebGL 以外、更新メソッドは動作しないようにします/否则，更新方法将不起作用
            enabled = false;
#endif
            // モバイルの入力対応/支持移动输入
            if (Application.isMobilePlatform)
            {
                gameObject.AddComponent<WebGLInputMobile>();
            }
        }

        /// <summary>
        /// Get the element rect of input
        /// </summary>
        /// <returns></returns>
        RectInt GetElemetRect()
        {
            var rect = GetScreenCoordinates(input.RectTransform());
            // モバイルの場合、強制表示する/对于移动，强制显示
            if (showHtmlElement || Application.isMobilePlatform)
            {
                var x = (int)(rect.x);
                var y = (int)(Screen.height - (rect.y + rect.height));
                return new RectInt(x, y, (int)rect.width, (int)rect.height);
            }
            else
            {
                var x = (int)(rect.x);
                var y = (int)(Screen.height - (rect.y));
                return new RectInt(x, y, (int)rect.width, (int)1);
            }
        }
        /// <summary>
        /// 対象が選択されたとき/选择对象时
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSelect()
        {
            if (id != -1) throw new Exception("OnSelect : id != -1");
            var rect = GetElemetRect();
            bool isPassword = input.contentType == ContentType.Password;
            var fontSize = Mathf.Max(14, input.fontSize); // limit font size : 14 !!
            // モバイルの場合、強制表示する/对于移动，强制显示
            var isHidden = !(showHtmlElement || Application.isMobilePlatform);
            id = WebGLInputPlugin.WebGLInputCreate(WebGLInput.CanvasId, rect.x, rect.y, rect.width, rect.height, fontSize, input.text, input.placeholder, input.lineType != LineType.SingleLine, isPassword, isHidden, Application.isMobilePlatform);
            instances[id] = this;
            WebGLInputPlugin.WebGLInputEnterSubmit(id, input.lineType != LineType.MultiLineNewline);
            WebGLInputPlugin.WebGLInputOnFocus(id, OnFocus);
            WebGLInputPlugin.WebGLInputOnBlur(id, OnBlur);
            WebGLInputPlugin.WebGLInputOnValueChange(id, OnValueChange);
            WebGLInputPlugin.WebGLInputOnEditEnd(id, OnEditEnd);
            WebGLInputPlugin.WebGLInputTab(id, OnTab);
            // default value : https://www.w3schools.com/tags/att_input_maxlength.asp
            WebGLInputPlugin.WebGLInputMaxLength(id, (input.characterLimit > 0) ? input.characterLimit : 524288);
            WebGLInputPlugin.WebGLInputFocus(id);
#if WEBGLINPUT_TAB
            WebGLInputPlugin.WebGLInputEnableTabText(id, enableTabText);
#endif
            if (input.OnFocusSelectAll)
            {
                WebGLInputPlugin.WebGLInputSetSelectionRange(id, 0, input.text.Length);
            }

            WebGLWindow.OnBlurEvent += OnWindowBlur;
        }

        void OnWindowBlur()
        {
            blurBlock = true;
        }

        /// <summary>
        /// 取得画面内的描绘范围
        /// </summary>
        /// <param name="uiElement"></param>
        /// <returns></returns>
        Rect GetScreenCoordinates(RectTransform uiElement)
        {
            var worldCorners = new Vector3[4];
            uiElement.GetWorldCorners(worldCorners);

            // try to support RenderMode:WorldSpace
            var canvas = uiElement.GetComponentInParent<Canvas>();
            var useCamera = (canvas.renderMode != RenderMode.ScreenSpaceOverlay);
            if (canvas && useCamera)
            {
                var camera = canvas.worldCamera;
                if (!camera) camera = Camera.main;

                for (var i = 0; i < worldCorners.Length; i++)
                {
                    worldCorners[i] = camera.WorldToScreenPoint(worldCorners[i]);
                }
            }

            var min = new Vector3(float.MaxValue, float.MaxValue);
            var max = new Vector3(float.MinValue, float.MinValue);
            for (var i = 0; i < worldCorners.Length; i++)
            {
                min.x = Mathf.Min(min.x, worldCorners[i].x);
                min.y = Mathf.Min(min.y, worldCorners[i].y);
                max.x = Mathf.Max(max.x, worldCorners[i].x);
                max.y = Mathf.Max(max.y, worldCorners[i].y);
            }

            return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
        }

        internal void DeactivateInputField()
        {
            if (!instances.ContainsKey(id)) return;

            WebGLInputPlugin.WebGLInputDelete(id);
            input.DeactivateInputField();
            instances.Remove(id);
            id = -1;    // reset id to -1;
            WebGLWindow.OnBlurEvent -= OnWindowBlur;
        }

        [MonoPInvokeCallback(typeof(Action<int>))]
        static void OnFocus(int id)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Input.ResetInputAxes(); // Inputの状態リセット/状态复位
            UnityEngine.WebGLInput.captureAllKeyboardInput = false;
#endif
        }

        [MonoPInvokeCallback(typeof(Action<int>))]
        static void OnBlur(int id)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            UnityEngine.WebGLInput.captureAllKeyboardInput = true;
            Input.ResetInputAxes(); // Inputの状態リセット/状态复位
#endif
            instances[id].StartCoroutine(Blur(id));
        }

        static IEnumerator Blur(int id)
        {
            yield return null;
            if (!instances.ContainsKey(id)) yield break;

            var block = instances[id].blurBlock;    // get blur block state
            instances[id].blurBlock = false;        // reset instalce block state
            if (block) yield break;                 // if block. break it!!
            instances[id].DeactivateInputField();
        }

        [MonoPInvokeCallback(typeof(Action<int, string>))]
        static void OnValueChange(int id, string value)
        {
            if (!instances.ContainsKey(id)) return;

            var instance = instances[id];
            if (!instance.input.ReadOnly)
            {
                instance.input.text = value;
            }

            // InputField.ContentType.Name 如果为Name，则首字母为强制大写字母，因此与小写字母相比
            if (instance.input.contentType == ContentType.Name)
            {
                if (string.Compare(instance.input.text, value, true) == 0)
                {
                    value = instance.input.text;
                }
            }

            // InputField の ContentType 将基于的整形文本重新设置为HTML的input
            if (value != instance.input.text)
            {
                var start = WebGLInputPlugin.WebGLInputSelectionStart(id);
                var end = WebGLInputPlugin.WebGLInputSelectionEnd(id);
                // take the offset.when char remove from input.
                var offset = instance.input.text.Length - value.Length;

                WebGLInputPlugin.WebGLInputText(id, instance.input.text);
                // reset the input element selection range!!
                WebGLInputPlugin.WebGLInputSetSelectionRange(id, start + offset, end + offset);
            }
        }
        [MonoPInvokeCallback(typeof(Action<int, string>))]
        static void OnEditEnd(int id, string value)
        {
            if (!instances[id].input.ReadOnly)
            {
                instances[id].input.text = value;
            }
        }
        [MonoPInvokeCallback(typeof(Action<int, int>))]
        static void OnTab(int id, int value)
        {
            WebGLInputTabFocus.OnTab(instances[id], value);
        }

        void Update()
        {
            if (input == null || !input.isFocused)
            {
                CheckOutFocus();
                return;
            }

            // 未登録の場合、選択する/ 未注册时，选择
            if (!instances.ContainsKey(id))
            {
                if (Application.isMobilePlatform)
                {
                    return;
                } else
                {
                    OnSelect();
                }
            }
            else if (!WebGLInputPlugin.WebGLInputIsFocus(id))
            {
                if (Application.isMobilePlatform)
                {
                    //input.DeactivateInputField();
                    return;
                }
                else
                {
                    // focus this id
                    WebGLInputPlugin.WebGLInputFocus(id);
                }
            }

            var start = WebGLInputPlugin.WebGLInputSelectionStart(id);
            var end = WebGLInputPlugin.WebGLInputSelectionEnd(id);
            // 選択方向によって設定します/選択方向によって設定します
            if (WebGLInputPlugin.WebGLInputSelectionDirection(id) == -1)
            {
                input.selectionFocusPosition = start;
                input.selectionAnchorPosition = end;
            }
            else
            {
                input.selectionFocusPosition = end;
                input.selectionAnchorPosition = start;
            }

            input.Rebuild();
        }

        private void OnDestroy()
        {
            if (!instances.ContainsKey(id)) return;

#if UNITY_WEBGL && !UNITY_EDITOR
            UnityEngine.WebGLInput.captureAllKeyboardInput = true;
            Input.ResetInputAxes(); // Inputの状態リセット
#endif
            DeactivateInputField();
        }

        private void OnEnable()
        {
            WebGLInputTabFocus.Add(this);
        }
        private void OnDisable()
        {
            WebGLInputTabFocus.Remove(this);
        }
        public int CompareTo(WebGLInput other)
        {
            var a = GetScreenCoordinates(input.RectTransform());
            var b = GetScreenCoordinates(other.input.RectTransform());
            var res = b.y.CompareTo(a.y);
            if (res == 0) res = a.x.CompareTo(b.x);
            return res;
        }

        public void CheckOutFocus()
        {
            if (!Application.isMobilePlatform) return;
            if (!instances.ContainsKey(id)) return;
            var current = EventSystem.current.currentSelectedGameObject;
            if (current != null) return;
            WebGLInputPlugin.WebGLInputForceBlur(id);   // Input ではないし、キーボードを閉じる/关闭键盘
        }

        /// <summary>
        /// to manage tab focus
        /// base on scene position
        /// </summary>
        static class WebGLInputTabFocus
        {
            static List<WebGLInput> inputs = new List<WebGLInput>();

            public static void Add(WebGLInput input)
            {
                inputs.Add(input);
                inputs.Sort();
            }

            public static void Remove(WebGLInput input)
            {
                inputs.Remove(input);
            }

            public static void OnTab(WebGLInput input, int value)
            {
                if (inputs.Count <= 1) return;
                var index = inputs.IndexOf(input);
                index += value;
                if (index < 0) index = inputs.Count - 1;
                else if (index >= inputs.Count) index = 0;
                inputs[index].input.ActivateInputField();
            }
        }
    }
}
