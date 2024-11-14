using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;
using VSWorkSDK.Enume;

namespace Dll_Project
{
    public class SDKKodFile : DllGenerateBase
    {
        private string pdfurl = "https://vs.vscloud.vip/vs/index.php?user/publicLink&fid=4fb1JSicWrIAa057S346VmNqJD7ZYj7Q58iMbw_sUi0zS_4Muyu93h9wKAmQmAw2-R8ZjDCw2x341H0srw6gudpMdvXG5fQD1d8eGJu7SMpQYlKihw_E6ae2ZB9i-CC0y8lEDhdviFCN4SNhrzsCjjIZdXZbJ4bTrQbIyTw&file_name=/VSWORK%E5%85%83%E5%AE%87%E5%AE%99%E5%BC%95%E6%93%8E-%E5%85%AC%E5%8F%B8%E4%BB%8B%E7%BB%8D.pdf";
        string imgurl = "https://vs.vscloud.vip/vs/index.php?user/publicLink&fid=a7b2jRPdODLl0LXLqJfLN33Bex0I9u636VDkyq_h_fbdvKJzPAV0AtSyooGd-8aU9YHbf9D8JFNhRViR-PsG1Yf5WTl2_kdRl3zqvp__w_B43KIewI2VJzMQk5hJ5bv-4SlswRKp-GWWLave&file_name=/1-%E5%86%88%E4%BB%81%E6%B3%A2%E9%BD%90%E5%B3%B0.jpg";
        string sceneurl = "https://vs.vscloud.vip/vs/index.php?user/publicLink&fid=d8f5eexsmom-ScU-vd8OYv8ssPNA8smRIlBP-xscJOStIHMEIh2xsPqmruIkAd-DT88tA4_f-ulWex7QSqS2uPC2CTwFryxc-xZpRzNFEyBx8h9mrDTZJdOsLUd_pObIMiDYfRv6IzSCa7I2IhLB&file_name=/a_pptnew.scene";
        string movieurl = "https://vs.vscloud.vip/vs/index.php?user/publicLink&fid=135f-c3XplV8vbF5ZzD3KKQHcT2Bs37AG8T2U2PTxmIBVAUhVV76M0RnIOemdwuVbl7He2e2DDipJPHkGG7KrzVyykFoJu05b6PqpXllpPOHzRRjD25y9nxWBnenj_YtuA&file_name=/%E6%B5%B7%E6%B4%8B.mp4";
        string orderurl = "https://vs.vscloud.vip/vs/index.php?user/publicLink&fid=0a118ymjn4s0gTdD_DQkmDQnRgVR9jMS1_LB1H7NKnLAsbimRtUE2QDz2yaucbCvJ1QsIydTYv8C51HVVSRuXUm5BXYs386BXnivDcBbGbRBXOB7UOYRSK9OrT6gLaiMa7ySukdfPMjaTntDMoSK-pXwHhdsRZRDzPA75TJLyAJBkg&file_name=/%E4%BD%93%E8%82%B2%E9%A6%86%E7%9B%B4%E6%92%AD.order";
        string glburl = "https://vs.vscloud.vip/vs/index.php?user/publicLink&fid=b2deA_tzBaLYJb7oQfZd8rUTontblNnYHV-k17uQP10FDEsOjJ7vYOGXZRiPNzU8s8km_v7SYjTNbHRNvQf-WZj4qEHl_E0F_7we79lDSfDOezVwDuaEDbpypHMNpX7CtJGHBOaGtOtSgEkl3SBJYA&file_name=/6-%E6%A8%A1%E5%9E%8B-%E8%88%AA%E5%A4%A9%E9%A3%9E%E6%9C%BA.glb";
        string txturl = "https://vs.vscloud.vip/vs/index.php?user/publicLink&fid=fc33MO8o8BK_1jN_OKhDw7NC1E7ya2fzL7fnBeKRQSeDhda-F57y0i0GG65W1BwAl57fuR8xjmYamu2vc1UaucP1_0Lggivgha2gAGYs7RB0Zh8x&file_name=/test.txt";
        string weblinkurl = "https://vs.vscloud.vip/vs/index.php?user/publicLink&fid=f365e-2gn8b9VhGX6Jwxmd888_7oSlX5Tp9Y48skpGIsjn8PQRmZVYNiMCasx9La8UVRYAZWa1yIGdPi1JQkERuJjZ-M1h-05CX04q13wGOARbChtBNJUye9&file_name=/board.link";
        string channelurl = "https://vs.vscloud.vip/vs/index.php?user/publicLink&fid=25afoo4NQV7WYZzLuL6QtrbkACWSTusXjoUa4l-i4b1D3Tk-94ySbUO30LGBrPaF0US9T4UxqMvICrebnt68fTUye-WvGFA5TtzKLOnNgBuRzt-YfrSQMQ&file_name=/test.channel";
        
        public override void Init()
        {
            base.Init();
        }
        public override void Awake()
        {
            base.Awake();
        }
        public override void OnEnable()
        {
            base.OnEnable();
            VSEngine.Instance.OnEventGetOrderFileString += OnGetOrderFileString;
            VSEngine.Instance.OnEventGetTxtFileString += OnGetTxtFileString;
            VSEngine.Instance.OnEventDownLoadFileProgress += OnDownloadFileProgress;

        }
        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventGetOrderFileString -= OnGetOrderFileString;
            VSEngine.Instance.OnEventGetTxtFileString -= OnGetTxtFileString;
            VSEngine.Instance.OnEventDownLoadFileProgress -= OnDownloadFileProgress;
        }
        public override void Start()
        {
            base.Start();
        }
        public override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.F1))
            {
                VSEngine.Instance.ClearCacheDataFile(CacheDataFileType.MediaFile);
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                VSEngine.Instance.ShowBigScreen(true);
                VSEngine.Instance.SetBigScreenProperty(Vector3.zero, Quaternion.Euler(Vector3.zero), Vector3.one, 10, true);

                WsMediaFile file = new WsMediaFile()
                {
                    name = "testmedia",
                    url = pdfurl.Replace(VSEngine.Instance.GetMediaResServerUrl(),""),
                    ext = "PDF",
                    fileMd5 = "",
                    mtime = ""
                };
                VSEngine.Instance.LoadBigScreenMediaFileAndSync(file);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                WsMediaFile file = new WsMediaFile()
                {
                    name = "testimage",
                    url = imgurl,
                    fileMd5 = "",
                    mtime = ""
                };
                VSEngine.Instance.LoadBigScreenImageFile(file);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                WsMediaFile file = new WsMediaFile()
                {
                    name = "testscene",
                    url = sceneurl,
                    fileMd5 = "",
                    mtime = ""
                };
                VSEngine.Instance.LoadSceneFile(file);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                WsMediaFile file = new WsMediaFile()
                {
                    name = "testmovie.mp4",
                    url = movieurl,
                    fileMd5 = "",
                    mtime = ""
                };
                VSEngine.Instance.LoadBigScreenMovieFile(file);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                WsMediaFile file = new WsMediaFile()
                {
                    name = "testorder2",
                    url = orderurl,
                    ext = "order",
                    fileMd5 = "",
                    mtime = ""
                };
                VSEngine.Instance.LoadBigScreenOrderFile(file);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                WsMediaFile file = new WsMediaFile()
                {
                    name = "testglb",
                    url = glburl,
                    ext = "glb",
                    fileMd5 = "",
                    mtime = ""
                };
                VSEngine.Instance.LoadGlbFile(file);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                WsMediaFile file = new WsMediaFile()
                {
                    name = "testtxt",
                    url = txturl,
                    ext = "txt",
                    fileMd5 = "",
                    mtime = ""
                };
                VSEngine.Instance.LoadTxtFile(file);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                WsMediaFile file = new WsMediaFile()
                {
                    name = "testpdf",
                    url = pdfurl,
                    fileMd5 = "",
                    mtime = ""
                };
                VSEngine.Instance.LoadBigScreenPDFFile(file);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                WsMediaFile file = new WsMediaFile()
                {
                    name = "testweblink",
                    url = weblinkurl,
                    fileMd5 = "",
                    mtime = ""
                };
                VSEngine.Instance.LoadBigScreenWebViewLinkFile(file);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                WsMediaFile file = new WsMediaFile()
                {
                    name = "testchannel",
                    url = channelurl,
                    fileMd5 = "",
                    mtime = ""
                };
                VSEngine.Instance.LoadSwitchRoomChannelFile(file);
            }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        void OnGetOrderFileString(string value)
        {
            Debug.Log("VSEngine OnGetOrderFileString value " + value);
        }
        void OnGetTxtFileString(string value)
        {
            Debug.Log("VSEngine OnGetTxtFileString value " + value);
        }
        void OnDownloadFileProgress(VRProgressInfo progressdata)
        {
            Debug.Log("VSEngine OnDownloadFileProgress progress " + progressdata.name + " " + progressdata.progress);
        }
    }
}
