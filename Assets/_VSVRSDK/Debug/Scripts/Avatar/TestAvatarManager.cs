using com.ootii.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VSWorkSDK
{
    public class TestAvatarManager : MonoBehaviour
    {
        public TestAvatarController TempAvatar;
        private GameObject AvatarRoot;
        public bool ClearAvatarImidiatly = true;
        public float minheight = 1.1f;
        public float maxheight = 1.8f;
        public float handmaxdistance = 1.1f;
        public Dictionary<string, TestAvatarController> AllTestAvatar = new Dictionary<string, TestAvatarController>();
        public Dictionary<string, WsAvatarFrame> wsavatardic = new Dictionary<string, WsAvatarFrame>();
        private Vector3 TrueRootPositon;
        private Vector3 TrueSelfCamPosition;
        private int InitAvatarFrameCalc = 10;
        private string nowloadingaid = "";

        // Start is called before the first frame update
        void Start()
        {
            AvatarRoot = new GameObject();
            AvatarRoot.name = "_WsAvatarsRoot";
            AvatarRoot.AddComponent<DontDestroyOnLoad>();

            MessageDispatcher.AddListener(VrDispMessageType.InitWsAvatar.ToString(), InitWsAvatar, true);
            MessageDispatcher.AddListener(VrDispMessageType.DestroyWsAvatar.ToString(), DestroyWsAvatar, true);
        }

        private void DestroyWsAvatar(IMessage msg)
        {
            DestroyAvatar((string)msg.Data, (bool)msg.Sender);
        }
        void DestroyAvatar(string avtid, bool delnow, bool ignorelog = false)
        {
            if (AvatarRoot.transform.Find(avtid) != null)
            {
                if (AvatarRoot.transform.Find(avtid).GetComponent<TestAvatarController>())
                {
                    AvatarRoot.transform.Find(avtid).GetComponent<TestAvatarController>().DelayDestroy(delnow, ignorelog);
                }
            }
        }
        private void Update()
        {
            float scalefix = mStaticThings.I.MainVRROOT.lossyScale.x;
            Vector3 oriVec = new Vector3(mStaticThings.I.Maincamera.localPosition.x * scalefix, 0, mStaticThings.I.Maincamera.localPosition.z * scalefix);
            Vector3 selffix = Quaternion.AngleAxis(mStaticThings.I.MainVRROOT.eulerAngles.y, Vector3.up) * oriVec;

            TrueRootPositon = mStaticThings.I.MainVRROOT.position + selffix;
            TrueSelfCamPosition = TrueRootPositon + mStaticThings.I.trackfix.localPosition + mStaticThings.I.Maincamera.localPosition;



            if (Time.frameCount % InitAvatarFrameCalc == 0 && nowloadingaid == "" && wsavatardic.Count > 0)
            {
                Dictionary<string, float> ckavtdic = new Dictionary<string, float>();
                foreach (var kv in wsavatardic)
                {
                    float dist = Vector3.Distance(kv.Value.wp, mStaticThings.I.MainVRROOT.position);
                    ckavtdic.Add(kv.Key, dist);
                }
                KeyValuePair<string, float> kvp = GetNearestOne(ckavtdic);
                WsAvatarFrame CurWsAvatarFrame = wsavatardic[kvp.Key];
                DointiAvatar(CurWsAvatarFrame);
                nowloadingaid = CurWsAvatarFrame.aid;
            }
        }

        void DointiAvatar(WsAvatarFrame CurWsAvatarFrame)
        {
            if (CurWsAvatarFrame.id != mStaticThings.I.mAvatarID)
            {
                StartCoroutine(CreatAvatar(CurWsAvatarFrame));
            }
            else
            {
                if (wsavatardic.ContainsKey(CurWsAvatarFrame.id))
                {
                    wsavatardic.Remove(CurWsAvatarFrame.id);
                }
            }
        }

        IEnumerator CreatAvatar(WsAvatarFrame CurWsAvatarFrame)
        {
            yield return new WaitForEndOfFrame();
            if (CurWsAvatarFrame.id == null || CurWsAvatarFrame.id == "")
            {
                if (wsavatardic.ContainsKey(CurWsAvatarFrame.id))
                {
                    wsavatardic.Remove(CurWsAvatarFrame.id);
                }
                yield break;
            }

            GameObject newWsAvatar;

            //Debug.LogWarning("CCCCCCCCCCCCC:  " +  JsonUtility.ToJson(CurWsAvatarFrame));

            Transform avtobj = AvatarRoot.transform.Find(CurWsAvatarFrame.id);
            if (avtobj != null)
            {
                newWsAvatar = avtobj.gameObject;
            }
            else
            {
                if (AllTestAvatar.ContainsKey(CurWsAvatarFrame.id))
                {
                    AllTestAvatar.Remove(CurWsAvatarFrame.id);
                }

                yield return new WaitForEndOfFrame();
                newWsAvatar = Instantiate(TempAvatar.gameObject, CurWsAvatarFrame.wp, CurWsAvatarFrame.wr, AvatarRoot.transform) as GameObject;
                newWsAvatar.transform.localScale = CurWsAvatarFrame.ws;
                //Debug.LogWarning(CurWsAvatarFrame.ws);
                newWsAvatar.name = CurWsAvatarFrame.id;
            }

            TestAvatarController customADriver = newWsAvatar.GetComponent<TestAvatarController>();


            customADriver.StopDestroy();
            customADriver.AvatarID = CurWsAvatarFrame.id;
            customADriver.aid = CurWsAvatarFrame.aid;
            customADriver.nametxt.text = CurWsAvatarFrame.name;
            customADriver.tipnametxt.text = CurWsAvatarFrame.name;
            customADriver.sex = CurWsAvatarFrame.sex;
            customADriver.isvr = CurWsAvatarFrame.vr;

            if (!AllTestAvatar.ContainsKey(CurWsAvatarFrame.id))
            {
                AllTestAvatar.Add(CurWsAvatarFrame.id, customADriver);
            }

            if (wsavatardic.ContainsKey(CurWsAvatarFrame.id))
            {
                wsavatardic.Remove(CurWsAvatarFrame.id);
            }
            nowloadingaid = "";
        }


        KeyValuePair<string, float> GetNearestOne(Dictionary<string, float> dic)
        {
            if (dic.Count > 0)
            {
                List<KeyValuePair<string, float>> lst = new List<KeyValuePair<string, float>>(dic);
                lst.Sort(delegate (KeyValuePair<string, float> s1, KeyValuePair<string, float> s2)
                {
                    return s2.Value.CompareTo(s1.Value);
                });
                return lst.Last();
            }
            else
            {
                return new KeyValuePair<string, float>();
            }
        }

        private void InitWsAvatar(IMessage msg)
        {
            WsAvatarFrame CurWsAvatarFrame = (WsAvatarFrame)msg.Data;
            if (CurWsAvatarFrame.ae && (bool)msg.Sender)
            {
                if (!wsavatardic.ContainsKey(CurWsAvatarFrame.id))
                {
                    wsavatardic.Add(CurWsAvatarFrame.id, CurWsAvatarFrame);
                }
                else
                {
                    wsavatardic[CurWsAvatarFrame.id] = CurWsAvatarFrame;
                }
            }
        }
        internal void ClearAvatars()
        {
            for (int i = 0; i < AvatarRoot.transform.childCount; i++)
            {
                if (ClearAvatarImidiatly)
                {
                    AvatarRoot.transform.GetChild(i).gameObject.GetComponent<TestAvatarController>().DoDestroy(true);
                }
                else
                {
                    AvatarRoot.transform.GetChild(i).gameObject.GetComponent<TestAvatarController>().DestroyAvatarMesh();
                }
            }

            wsavatardic.Clear();

            Debug.Log("nowcache Avatar num: " + AllTestAvatar.Count);
        }

        internal WsAvatarFrame GetCurrentAvatarFrame()
        {
            WsAvatarFrame CurWsAvatarFrame = new WsAvatarFrame
            {

                wsid = mStaticThings.I.mWsID,
                id = mStaticThings.I.mAvatarID,
                aid = mStaticThings.I.aid,
                sex = mStaticThings.I.msex,
                vr = mStaticThings.I.isVRApp,
                name = mStaticThings.I.mNickName,
                ae = mStaticThings.I.SendAvatar,
                e = mStaticThings.I.SelectorEnabled,
                m = CommonVREventController.I.ismounted,
                a = mStaticThings.I.isAdmin,
                //vol = mStaticThings.I.nowMicVol,
                vol = mStaticThings.I.MicEnabled ? mStaticThings.I.nowMicVol : -1,
                scene = mStaticThings.I.mScene,
                wp = mStaticThings.I.MainVRROOT.position,
                wr = mStaticThings.I.MainVRROOT.rotation,
                ws = mStaticThings.I.MainVRROOT.localScale,
                cp = GetCurrentPoseJian(),
                cl = mStaticThings.I.nowpencolor
            };
            //Debug.LogWarning(JsonUtility.ToJson(CurWsAvatarFrame));
            return CurWsAvatarFrame;
        }

        internal void SetAvatarStopDestroy(WsAvatarFrame newAvatarFrame)
        {
            if (newAvatarFrame.id == null || newAvatarFrame.id == "")
            {
                return;
            }
            Transform delobj = AvatarRoot.transform.Find(newAvatarFrame.id);
            if (delobj != null)
            {
                GameObject newWsAvatar = delobj.gameObject;
                if (newWsAvatar.GetComponent<TestAvatarController>())
                {
                    newWsAvatar.GetComponent<TestAvatarController>().StopDestroy();
                }
            }
        }

        internal void RecieveWsAvatar(WsAvatarFrameJian curWsAvatarFrame)
        {
            if (AllTestAvatar.ContainsKey(curWsAvatarFrame.id) && AllTestAvatar[curWsAvatarFrame.id] != null)
            {
                AllTestAvatar[curWsAvatarFrame.id].RecieveWsAvatar(curWsAvatarFrame);
            }
        }

        PoseFrameJian GetCurrentPoseJian()
        {
            //nowscale = mStaticThings.I.MainVRROOT.localScale.x;
            Vector3 nowhp = mStaticThings.I.Maincamera.localPosition + mStaticThings.I.trackfix.localPosition;
            float heightfix = Mathf.Clamp(nowhp.y, minheight, maxheight);
            PoseFrameJian CurrentPoseJian = new PoseFrameJian
            {
                hp = new Vector3(nowhp.x, heightfix, nowhp.z),
                hr = mStaticThings.I.Maincamera.localRotation,
                hlp = GetLeftHandVector(handmaxdistance),
                hlr = mStaticThings.I.LeftHand.localRotation,
                hrp = GetRightHandVector(handmaxdistance),
                hrr = mStaticThings.I.RightHand.localRotation,
            };
            return CurrentPoseJian;
        }

        Vector3 GetLeftHandVector(float distance = 1.1f)
        {
            float dist = Vector3.Distance(mStaticThings.I.Maincamera.localPosition, mStaticThings.I.LeftHand.localPosition);
            if (dist < distance)
            {
                return mStaticThings.I.LeftHand.localPosition == Vector3.zero ? Vector3.zero : mStaticThings.I.LeftHand.localPosition + mStaticThings.I.trackfix.localPosition;
            }
            else
            {
                return Vector3.zero;
            }
        }

        Vector3 GetRightHandVector(float distance = 1.1f)
        {
            float dist = Vector3.Distance(mStaticThings.I.Maincamera.localPosition, mStaticThings.I.RightHand.localPosition);
            if (dist < distance)
            {
                return mStaticThings.I.RightHand.localPosition == Vector3.zero ? Vector3.zero : mStaticThings.I.RightHand.localPosition + mStaticThings.I.trackfix.localPosition;
            }
            else
            {
                return Vector3.zero;
            }
        }

    }
}
