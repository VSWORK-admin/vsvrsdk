
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;

namespace Dll_Project.SDK_VR_Object
{
    class Set_CustomSceneMenul_Language : DllGenerateBase
    {
        public GameObject cn;
        public GameObject kr;
        public GameObject en;
        public override void Start()
        {
          
            cn = BaseMono.ExtralDatas[0].Target.gameObject;
            kr = BaseMono.ExtralDatas[1].Target.gameObject;
            en = BaseMono.ExtralDatas[2].Target.gameObject;
            base.Start();
            switch (mStaticThings.I.NowLanguageType)
            {
                case LanguageType.English:
                    kr.SetActive(false);
                    cn.SetActive(false);
                    en.SetActive(true);
                    break;
                case LanguageType.Chinese:
                    kr.SetActive(false);
                    cn.SetActive(true);
                    en.SetActive(false);
                    break;
                case LanguageType.Korean:
                    kr.SetActive(true);
                    cn.SetActive(false);
                    en.SetActive(false);
                    break;
                case LanguageType.Japanese:
                    break;
                case LanguageType.LangA:
                    break;
                case LanguageType.LangB:
                    break;
                case LanguageType.LangC:
                    break;
                case LanguageType.LangD:
                    break;
                case LanguageType.LangE:
                    break;
                case LanguageType.LangF:
                    break;
                case LanguageType.langG:
                    break;
                case LanguageType.langH:
                    break;
                case LanguageType.langI:
                    break;
                case LanguageType.langJ:
                    break;
                case LanguageType.langK:
                    break;
                case LanguageType.langL:
                    break;
                case LanguageType.langM:
                    break;
                case LanguageType.langN:
                    break;
                default:
                    break;
            }
        }
        public override void  OnEnable()
        {
           
            
        }

     
        public override void OnDisable()
       
        {
           
        }
    }
}
