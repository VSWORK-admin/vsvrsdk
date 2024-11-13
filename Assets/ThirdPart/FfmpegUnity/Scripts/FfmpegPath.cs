using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FfmpegUnity
{
    public static class FfmpegPath
    {
        public enum DefaultPath
        {
            NONE,
            STREAMING_ASSETS_PATH,
            PERSISTENT_DATA_PATH,
            TEMPORARY_CACHE_PATH,
        }

        public static string PathWithDefault(DefaultPath defaultPath, string path)
        {
            switch (defaultPath)
            {
                case DefaultPath.NONE:
                default:
                    return path;
                case DefaultPath.STREAMING_ASSETS_PATH:
                    return Path.Combine(Application.streamingAssetsPath, path);
                case DefaultPath.PERSISTENT_DATA_PATH:
                    return Path.Combine(Application.persistentDataPath, path);
                case DefaultPath.TEMPORARY_CACHE_PATH:
                    return Path.Combine(Application.temporaryCachePath, path);
            }
        }
    }
}
