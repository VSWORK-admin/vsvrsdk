using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace VSWorkSDK
{
    /// <summary>
    /// SDK工具类
    /// </summary>
    public static class Tools
    {/// <summary>
     /// 用于枚举转int值
     /// </summary>
     /// <param name="enumType"></param>
     /// <returns></returns>
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
        /// <summary>
        /// 用于ASTC格式转成Texture
        /// </summary>
        /// <param name="ASTCByte">ASTC格式下载后Byte数组</param>
        /// <returns>texture</returns>
        public static Texture ASTCByteToTexture(byte[] ASTCByte)
        {
            if (ASTCByte[0].ToString("x") == "13" && ASTCByte[1].ToString("x") == "ab" && ASTCByte[2].ToString("x") == "a1" && ASTCByte[3].ToString("x") == "5c")
            {
                var fomat = TextureFormat.ASTC_8x8;
                if (ASTCByte[4] == 4 && ASTCByte[5] == 4)
                    fomat = TextureFormat.ASTC_4x4;
                else if (ASTCByte[4] == 5 && ASTCByte[5] == 5)
                    fomat = TextureFormat.ASTC_5x5;
                else if (ASTCByte[4] == 6 && ASTCByte[5] == 6)
                    fomat = TextureFormat.ASTC_6x6;
                else if (ASTCByte[4] == 10 && ASTCByte[5] == 10)
                    fomat = TextureFormat.ASTC_10x10;
                else if (ASTCByte[4] == 12 && ASTCByte[5] == 12)
                    fomat = TextureFormat.ASTC_12x12;
                else if (ASTCByte[4] != 8)
                    Debug.LogErrorFormat("unknown astc format: {0}x{1}", ASTCByte[4], ASTCByte[5]);


                int width = ASTCByte[7] + (ASTCByte[8] << 8) + (ASTCByte[9] << 16);
                int height = ASTCByte[10] + (ASTCByte[11] << 8) + (ASTCByte[12] << 16);
                var tex = new Texture2D(width, height, fomat, false);
                int len = ASTCByte.Length - 16;
                var b2 = new byte[len];
                Array.Copy(ASTCByte, 16, b2, 0, len);
                tex.LoadRawTextureData(b2);
                tex.Apply();
                return tex;
            }
            return null;
        }
        /// <summary>
        /// 根据编码类型返回输入的Byte[]
        /// </summary>
        /// <param name="value">输入的值</param>
        /// <param name="">encoding by name</param>
        /// <returns></returns>
        public static byte[] GetBytes(string value,string name)
        {
          return  System.Text.Encoding.GetEncoding(name).GetBytes(value);
        }
       
    }
}