using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dll_Project
{
    public static class DllGameObjectTool
    {
        /// <summary>
        /// 提取异常及其内部异常堆栈跟踪
        /// </summary>
        /// <param name="exception">提取的例外</param>
        /// <param name="lastStackTrace">最后提取的堆栈跟踪（对于递归）， String.Empty or null</param>
        /// <param name="exCount">提取的堆栈数（对于递归）</param>
        /// <returns>Syste.String</returns>
        public static string ExtractAllStackTrace(this Exception exception, string lastStackTrace = null, int exCount = 1)
        {
            var ex = exception;
            const string entryFormat = "#{0}: {1}\r\n{2}";
            //修复最后一个堆栈跟踪参数
            lastStackTrace = lastStackTrace ?? string.Empty;
            //添加异常的堆栈跟踪
            lastStackTrace += string.Format(entryFormat, exCount, ex.Message, ex.StackTrace);
            if (exception.Data.Count > 0)
            {
                lastStackTrace += "\r\n    Data: ";
                foreach (var item in exception.Data)
                {
                    var entry = (DictionaryEntry)item;
                    lastStackTrace += string.Format("\r\n\t{0}: {1}", entry.Key, exception.Data[entry.Key]);
                }
            }
            //递归添加内部异常
            if ((ex = ex.InnerException) != null)
                return ex.ExtractAllStackTrace(string.Format("{0}\r\n\r\n", lastStackTrace), ++exCount);
            return lastStackTrace;
        }
        public static T GetDllBaseComponent<T>(this Transform trans, string TrueClassName) where T : DllGenerateBase
        {
            var scripts = trans.GetComponents<GeneralDllBehavior>();

            if (scripts == null) return null;

            for (int i = 0; i < scripts.Length; i++)
            {
                if (scripts[i].ScriptClassName.Equals(TrueClassName))
                {
                    return (T)scripts[i].DllClass;
                }
            }

            return null;
        }

        public static T GetDllBaseComponent<T>(this GameObject obj, string TrueClassName) where T : DllGenerateBase
        {
            var scripts = obj.GetComponents<GeneralDllBehavior>();

            if (scripts == null) return null;

            for (int i = 0; i < scripts.Length; i++)
            {
                if (scripts[i].ScriptClassName.Equals(TrueClassName))
                {
                    return (T)scripts[i].DllClass;
                }
            }

            return null;
        }

        public static T GetDllComponent<T>(this Transform trans) where T : DllGenerateBase
        {
            var scripts = trans.GetComponents<GeneralDllBehavior>();

            if (scripts == null) return null;

            var type = typeof(T);

            for (int i = 0; i < scripts.Length; i++)
            {
                if (scripts[i].ScriptClassName.Equals(type.FullName))
                {
                    return (T)scripts[i].DllClass;
                }
            }

            return null;
        }

        public static T GetDllComponent<T>(this GameObject obj) where T : DllGenerateBase
        {
            var scripts = obj.GetComponents<GeneralDllBehavior>();

            if (scripts == null) return null;

            var type = typeof(T);

            for (int i = 0; i < scripts.Length; i++)
            {
                if (scripts[i].ScriptClassName.Equals(type.FullName))
                {
                    return (T)scripts[i].DllClass;
                }
            }

            return null;
        }

        public static T AddDllComponent<T>(this GameObject go, string ScriptClassName) where T : DllGenerateBase
        {
            var Script = go.GetDllBaseComponent<T>(ScriptClassName);
            if (Script != null)
            {
                return Script;
            }

            GeneralDllBehaviorAdapter adapter = go.GetComponent<GeneralDllBehaviorAdapter>();
            if (adapter == null)
            {
                adapter = go.AddComponent<GeneralDllBehaviorAdapter>();
            }
            if (adapter != null)
            {
                adapter.ScriptClassName = ScriptClassName;
                //引用自己加
                //adapter.ExtralDatas = ExtralDatas;
                //adapter.ExtralDataObjs = ExtralDataObjs;
                //adapter.ExtralDataInfos = ExtralDataInfos;
                var DllBehavior = go.AddComponent<GeneralDllBehavior>();
                if (DllBehavior != null)
                {
                    return DllBehavior.DllClass as T;
                }
            }

            return null;
        }
    }
}