using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
public enum CubemapFaceType
{
    Right,
    Left,
    Upwards,
    Downward,
    Forward,
    Backward
}
[System.Serializable]
public class CubeMapTexture
{
    public CubemapFaceType cubemapFace;
    public Texture2D texture;
}
public class CubeMapManager : MonoBehaviour
{
    private static int pixelsLen = 1;
    private static Color[] des = null;
    #region 测试CubeMap六面位置
#if UNITY_EDITOR
    public CubeMapTexture[] textures;
    public Light Light;

    // Start is called before the first frame update
    void Start()
    {
        //Light.type = LightType.Point;
        //Light.intensity = 1.3f;
        //Light.range = 100;
        //Light.color = Color.white;
        //Light.lightmapBakeType = LightmapBakeType.Realtime;
        //Light.shadows = LightShadows.None;

        //for (int i = 0; i < textures.Length; i++)
        //{
        //    var item = textures[i];
        //    if (!item.texture.isReadable)
        //    {
        //        var path = AssetDatabase.GetAssetPath(item.texture);
        //        var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        //        //textureImporter.textureFormat = TextureImporterFormat.RGB24;
        //        textureImporter.isReadable = true;
        //        textureImporter.SaveAndReimport();

        //        item.texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        //    }
        //}
        //Light.cookie = BuildCubemap2(textures);
    }
    // Update is called once per frame
    void Update()
    {

    }
#endif
    #endregion
    public static CubemapFace GetCubemapFace(CubemapFaceType faceType)
    {
        switch (faceType)
        {
            case CubemapFaceType.Left:
                return CubemapFace.NegativeX;
            case CubemapFaceType.Right:
                return CubemapFace.PositiveX;
            case CubemapFaceType.Upwards:
                return CubemapFace.PositiveY;
            case CubemapFaceType.Downward:
                return CubemapFace.NegativeY;
            case CubemapFaceType.Forward:
                return CubemapFace.PositiveZ;
            case CubemapFaceType.Backward:
                return CubemapFace.NegativeZ;
        }

        return CubemapFace.Unknown;
    }
    /// <summary>
    /// 构建cubmap算法1
    /// </summary>
    public static Cubemap BuildCubemap(CubeMapTexture[] texs)
    {
        if (texs.Length != 6) throw new System.Exception("textureArr.Length != 6");

        var cubemap = new Cubemap(texs[0].texture.width, TextureFormat.RGB24, false);

        //Set CubemapFace.(force fix texture format question).
        for (int i = 0; i < texs.Length; i++)
        {
            var tex = texs[i];
            if (tex == null || tex.texture == null)
            {
                return cubemap;
            }
            cubemap.SetPixels(tex.texture.GetPixels().Reverse().ToArray(), GetCubemapFace(tex.cubemapFace));
        }
        cubemap.Apply();

        return cubemap;
    }
    /// <summary>
    /// 构建cubmap算法2
    /// </summary>
    public static Cubemap BuildCubemap2(CubeMapTexture[] texs)
    {
        if (texs.Length != 6) throw new System.Exception("textureArr.Length != 6");

        Cubemap cubemap = new Cubemap(texs[0].texture.width, TextureFormat.RGB24, false);
        for (int i = 0; i < texs.Length; i++)
        {
            var tex = texs[i];
            if (tex == null || tex.texture == null)
            {
                return cubemap;
            }
            Texture2D flipTexture = HorizontalFlipTexture(VerticalFlipTexture(tex.texture));

            var pixels = flipTexture.GetPixels(0, 0, flipTexture.width, flipTexture.height);

            cubemap.SetPixels(pixels, GetCubemapFace(tex.cubemapFace));
        }
        cubemap.Apply();

        return cubemap;
    }
    /// <summary>
    /// 构建cubmap算法2
    /// </summary>
    /// <summary>
    /// 构建cubmap算法2
    /// </summary>
    public static Cubemap BuildCubemapRaw(CubeMapTexture[] texs)
    {
        if (texs.Length != 6) throw new System.Exception("textureArr.Length != 6");

        Cubemap cubemap = new Cubemap(texs[0].texture.width, TextureFormat.RGBA32, false);
        for (int i = 0; i < texs.Length; i++)
        {
            var tex = texs[i];
            if (tex == null || tex.texture == null)
            {
                return cubemap;
            }

            Texture2D flipTexture = tex.texture;
            //if (tex.cubemapFace == CubemapFaceType.Upwards)
            //{
            //    flipTexture = VerticalFlipTexture(tex.texture);
            //}
            //else if (tex.cubemapFace == CubemapFaceType.Downward)
            //{
            //    flipTexture = VerticalFlipTexture(tex.texture);
            //}
            //else
            //{
            //    flipTexture = HorizontalFlipTexture(tex.texture);
            //    //Mirror(flipTexture.GetPixels(0,0, flipTexture.width, flipTexture.height)
            //    //   , flipTexture.width, flipTexture.height, flipTexture);

            //    //flipTexture.SetPixels32(FlipColors2(tex.texture.GetPixels32(), flipTexture.width, flipTexture.height));
            //    //flipTexture = InverseTex(tex.texture, false);
            //}

            var pixels = flipTexture.GetPixels(0, 0, flipTexture.width, flipTexture.height);

            cubemap.SetPixels(pixels, GetCubemapFace(tex.cubemapFace));

            //if(flipTexture != null)
            //{
            //    GameObject.Destroy(flipTexture);
            //}
            if(tex.texture != null)
            {
                GameObject.Destroy(tex.texture);
            }
        }
        cubemap.Apply();

        return cubemap;
    }
    /// <summary>
    /// 裁剪Texture2D
    /// </summary>
    /// <param name="originalTexture"></param>
    /// <param name="offsetX"></param>
    /// <param name="offsetY"></param>
    /// <param name="originalWidth"></param>
    /// <param name="originalHeight"></param>
    /// <returns></returns>
    public static Texture2D ScaleTextureCutOut(Texture2D originalTexture, int offsetX, int offsetY, float originalWidth, float originalHeight)
    {
        Texture2D newTexture = new Texture2D(Mathf.CeilToInt(originalWidth), Mathf.CeilToInt(originalHeight));
        int maxX = originalTexture.width - 1;
        int maxY = originalTexture.height - 1;
        for (int y = 0; y < newTexture.height; y++)
        {
            for (int x = 0; x < newTexture.width; x++)
            {
                float targetX = x + offsetX;
                float targetY = y + offsetY;
                int x1 = Mathf.Min(maxX, Mathf.FloorToInt(targetX));
                int y1 = Mathf.Min(maxY, Mathf.FloorToInt(targetY));
                int x2 = Mathf.Min(maxX, x1 + 1);
                int y2 = Mathf.Min(maxY, y1 + 1);

                float u = targetX - x1;
                float v = targetY - y1;
                float w1 = (1 - u) * (1 - v);
                float w2 = u * (1 - v);
                float w3 = (1 - u) * v;
                float w4 = u * v;
                Color color1 = originalTexture.GetPixel(x1, y1);
                Color color2 = originalTexture.GetPixel(x2, y1);
                Color color3 = originalTexture.GetPixel(x1, y2);
                Color color4 = originalTexture.GetPixel(x2, y2);
                Color color = new Color(Mathf.Clamp01(color1.r * w1 + color2.r * w2 + color3.r * w3 + color4.r * w4),
                                        Mathf.Clamp01(color1.g * w1 + color2.g * w2 + color3.g * w3 + color4.g * w4),
                                        Mathf.Clamp01(color1.b * w1 + color2.b * w2 + color3.b * w3 + color4.b * w4),
                                        Mathf.Clamp01(color1.a * w1 + color2.a * w2 + color3.a * w3 + color4.a * w4)
                                        );
                newTexture.SetPixel(x, y, color);
            }
        }
        newTexture.anisoLevel = 2;
        newTexture.Apply();
        return newTexture;
    }
    /// <summary>
    /// 缩放Textur2D
    /// </summary>
    /// <param name="source"></param>
    /// <param name="targetWidth"></param>
    /// <param name="targetHeight"></param>
    /// <returns></returns>
    public static Texture2D ScaleTexture(Texture2D source, float targetWidth, float targetHeight)
    {
        Texture2D result = new Texture2D((int)targetWidth, (int)targetHeight, source.format, false);

        float incX = (1.0f / targetWidth);
        float incY = (1.0f / targetHeight);

        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }

        result.Apply();
        return result;
    }
    //水平翻转
    public static Texture2D HorizontalFlipTexture(Texture2D texture)
    {
        //得到图片的宽高
        int width = texture.width;
        int height = texture.height;

        Texture2D flipTexture = new Texture2D(width, height);

        for (int i = 0; i < width; i++)
        {
            flipTexture.SetPixels(i, 0, 1, height, texture.GetPixels(width - i - 1, 0, 1, height));
        }
        flipTexture.Apply();

        return flipTexture;
    }
    // 垂直翻转
    public static Texture2D VerticalFlipTexture(Texture2D texture)
    {
        //得到图片的宽高
        int width = texture.width;
        int height = texture.height;

        Texture2D flipTexture = new Texture2D(width, height);
        for (int i = 0; i < height; i++)
        {
            flipTexture.SetPixels(0, i, width, 1, texture.GetPixels(0, height - i - 1, width, 1));
        }
        flipTexture.Apply();
        return flipTexture;
    }
    /// <summary>
    /// 图片镜像
    /// </summary>
    /// <param name="src">原图片二进制数据</param>
    /// <param name="srcW">原图片宽度</param>
    /// <param name="srcH">原图片高度</param>
    /// <param name="desTexture">输出目标图片</param>
    public static void Mirror(Color[] src, int srcW, int srcH, Texture2D desTexture)
    {
        if (pixelsLen != src.Length || des == null)
        {
            pixelsLen = src.Length;
            des = new Color[pixelsLen];
        }

        if (desTexture.width != srcW || desTexture.height != srcH)
        {
            desTexture.Reinitialize(srcW, srcH);
        }

        for (int i = 0; i < srcH; i++)
        {
            for (int j = 0; j < srcW; j++)
            {
                des[i * srcW + j] = src[(i + 1) * srcW - j - 1];
            }
        }

        desTexture.SetPixels(des);
        desTexture.Apply();
    }
    /// <summary>
    /// 图片逆时针旋转90度
    /// </summary>
    /// <param name="src">原图片二进制数据</param>
    /// <param name="srcW">原图片宽度</param>
    /// <param name="srcH">原图片高度</param>
    /// <param name="desTexture">输出目标图片</param>
    public static Texture2D RotationLeft90(Color32[] src, int srcW, int srcH)
    {
        Color32[] des = new Color32[src.Length];
        Texture2D desTexture = new Texture2D(srcH, srcW);
        //if (desTexture.width != srcH || desTexture.height != srcW)
        //{
        //    desTexture.Resize(srcH, srcW);
        //}

        for (int i = 0; i < srcW; i++)
        {
            for (int j = 0; j < srcH; j++)
            {
                des[i * srcH + j] = src[(srcH - 1 - j) * srcW + i];
            }
        }

        desTexture.SetPixels32(des);
        desTexture.Apply();
        return desTexture;
    }
    /// <summary>
    /// 图片顺时针旋转90度
    /// </summary>
    /// <param name="src">原图片二进制数据</param>
    /// <param name="srcW">原图片宽度</param>
    /// <param name="srcH">原图片高度</param>
    /// <param name="desTexture">输出目标图片</param>
    public static Texture2D RotationRight90(Color32[] src, int srcW, int srcH)
    {

        Color32[] des = new Color32[src.Length];
        Texture2D desTexture = new Texture2D(srcH, srcW);
        for (int i = 0; i < srcH; i++)
        {
            for (int j = 0; j < srcW; j++)
            {
                des[(srcW - j - 1) * srcH + i] = src[i * srcW + j];
            }
        }

        desTexture.SetPixels32(des);
        desTexture.Apply();
        return desTexture;
    }
    /// <summary>
    /// 两张图合并
    /// </summary>
    /// <param name="_baseTexture2D"></param>
    /// <param name="_texture2D"></param>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_w"></param>
    /// <param name="_h"></param>
    /// <returns></returns>
    public static Texture2D MergeImage(Texture2D _baseTexture2D, Texture2D _texture2D, int _x, int _y, int _w, int _h)
    {
        //取图
        Color32[] color = _texture2D.GetPixels32(0);
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                _baseTexture2D.SetPixels32(_x + i * (_texture2D.width + _w), _y + j * (_texture2D.height + _h), _texture2D.width, _texture2D.height, color); //宽度
            }
        }
        //应用
        _baseTexture2D.Apply();
        return _baseTexture2D;
    }
    /// <summary>
    /// 将图片上下并镜像翻转
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Texture2D ChangeRGB(Texture2D source)
    {
        Texture2D result = new Texture2D(source.width, source.height, TextureFormat.RGB24, false);

        Color32[] sourceColor = source.GetPixels32();
        Color32[] newColor = new Color32[sourceColor.Length];
        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                newColor[(result.width * (result.height - i - 1)) + j] = sourceColor[(result.width * i) + j];
            }
        }
        result.SetPixels32(newColor);
        result.Apply();
        return result;
    }
    /// <summary>
    /// 将图片上下并镜像翻转
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Texture2D ChangeRGB2(Texture2D source)
    {
        Texture2D result = new Texture2D(source.width, source.height, TextureFormat.RGB24, false);

        Color32[] sourceColor = source.GetPixels32();
        Color32[] newColor = new Color32[sourceColor.Length];
        int currentR = 0;//当前的行
        int currentC = 0;//当前的列
        for (int i = 0; i < sourceColor.Length; i++)
        {
            if (i % result.width == 0)
            {
                currentR = i / result.width;
                currentC = 0;
            }
            else
            {
                currentC++;
            }
            newColor[(result.width * (result.height - currentR - 1)) + currentC] = sourceColor[(result.width * currentR) + currentC];
        }
        result.SetPixels32(newColor);
        result.Apply();
        return result;
    }
    /// <summary>
    /// 上下翻转
    /// </summary>
    /// <param name="originalColors"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static Color32[] FlipColors(Color32[] originalColors, int width, int height)
    {
        Color32[] tempRow = new Color32[width];
        for (int y = 0; y < height / 2; y++)
        {
            int topRowIndex = y * width;
            int bottomRowIndex = (height - y - 1) * width;

            // 交换顶部和底部的像素行
            Array.Copy(originalColors, topRowIndex, tempRow, 0, width);
            Array.Copy(originalColors, bottomRowIndex, originalColors, topRowIndex, width);
            Array.Copy(tempRow, 0, originalColors, bottomRowIndex, width);
        }
        return originalColors;
    }

    /// <summary>
    /// 左右翻转
    /// </summary>
    /// <param name="originalColors"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static Color32[] FlipColors2(Color32[] originalColors, int width, int height)
    {
        Color32[] tempRow = new Color32[height];
        for (int y = 0; y < width / 2; y++)
        {
            int topRowIndex = y * height;
            int bottomRowIndex = (width - y - 1) * height;

            // 交换顶部和底部的像素行
            Array.Copy(originalColors, topRowIndex, tempRow, 0, height);
            Array.Copy(originalColors, bottomRowIndex, originalColors, topRowIndex, height);
            Array.Copy(tempRow, 0, originalColors, bottomRowIndex, height);
        }
        return originalColors;
    }

    /// <summary>
    /// 翻转贴图
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public static Texture2D InverseTex(Texture2D origin, bool isInverseV = true)
    {
        Texture2D newTex = new Texture2D(width: origin.width, height: origin.height, textureFormat: origin.format, mipChain: false, linear: true);
        newTex.name = origin.name;
        //原图片只读模式需要使用RenderTexture进行拷贝
        if (origin.isReadable == false)
        {
            FillPixelsFromReadOnlyTex(newTex,origin: origin);
            origin = newTex;
        }

        //翻转
        Color[] colors = new Color[origin.height * origin.height];
        for (int row = 0; row < origin.height; row++)
        {
            for (int col = 0; col < origin.width; col++)
            {
                int index = row * origin.width + col;
                int x = isInverseV == true ? col : origin.width - 1 - col;
                int y = isInverseV == true ? origin.height - 1 - row : row;
                colors[index] = origin.GetPixel(x: x, y: y);
            }
        }
        newTex.SetPixels(colors: colors);
        newTex.Apply();
        //string msg = isInverseV == true ? "V方向" : "U方向";
        //Debug.Log($"{msg}翻转贴图:{origin.name}");
        return newTex;
    }

    /// <summary>
    /// 从不可读贴图中提取像素
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="dest"></param>
    public static void FillPixelsFromReadOnlyTex(Texture2D dest, Texture2D origin)
    {
        var destRenderTexture = RenderTexture.GetTemporary(width: origin.width, height: origin.height, depthBuffer: 0, format: RenderTextureFormat.ARGB32, readWrite: RenderTextureReadWrite.Linear);
        Graphics.Blit(origin, destRenderTexture);
        dest.ReadPixels(new Rect(0, 0, destRenderTexture.width, destRenderTexture.height), 0, 0);
        dest.Apply();
        //Debug.Log($"从不可读贴图:{origin.name}中提取像素");
    }
}
