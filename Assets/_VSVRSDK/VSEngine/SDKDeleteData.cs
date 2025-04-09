using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VSWorkSDK.Data
{
    public delegate void SDKWebResponseCallback(WebResponseData response);
    public class WebResponseData : System.IDisposable
    {
        public string Url = "";
        public bool IsDone = false;
        public float DownloadProgress = 0;
        public string Error = null;
        public string sign;
        public string localpath;
        public Texture2D img;
        public void Dispose()
        {
            localpath = null;
        }
    }
    public class RoomSycnData
    {
        public string a;
        public string b;
        public string c;
        public string d;
        public string e;
        public string f;
        public string g;
    }
    public class LoginData
    {
        public string vr_user_id;
        public string vr_user_name;
        public string vr_user_nickname;
        public int vr_user_sex;
        public string vr_user_phone;
        public string vr_user_country;
    }
   
}
