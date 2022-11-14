using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace VSEngine
{
    public static class Tools
    {
        public static List<int> EnumToList(this Type enumType)
        {
            List<int> result = new List<int>();
            if (!enumType.IsEnum)
            {
                return null;
            }
            foreach (var item in Enum.GetValues(enumType))
            {
                result.Add(Convert.ToInt32(item));
            }
            return result;
        }
    }
}