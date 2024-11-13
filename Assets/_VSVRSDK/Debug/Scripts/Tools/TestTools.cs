using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSWorkSDK
{
    public static class TestTools
    {
        public static int ToInt(this string s)
        {
            int result = 0;
            if (int.TryParse(s, out result))
            {
                return result;
            }
            return 0;
        }
    }
}