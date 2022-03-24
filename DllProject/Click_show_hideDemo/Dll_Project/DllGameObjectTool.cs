using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dll_Project
{
    public static class DllGameObjectTool
    {

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
    }
}