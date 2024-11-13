using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
using LitJson;
using System.IO;

namespace SDKUI
{
    public static class SDKUIDataStaticClass
    {
        public static SDKUIData sDKUIData;
        public static void SDKUIConnectWSRoom(JsonData jsonData)
        {
            SDKUIData ssDKUIData = JsonMapper.ToObject<SDKUIData>(jsonData.ToString());
            MessageDispatcher.SendMessage("", "SDKUIConnectWSRoom", ssDKUIData, 0.05f);
        }
    }
    public class SDKUIData
    {
        public int messageId;
        public SDKUIRoomData roomInfo;
        public SDKUIAvatarData avatar;
        public SDKUIUserData userInfo;
        public SDKUIGroup group;
        public string apitoken;
        public string username;
        public int languageType;


    }
    public class SDKUIRoomData
    {
        public string vr_rooms_id;
        public string vr_rooms_name;
        public string vr_rooms_socketioserver;
        public string vr_rooms_firstroom;
        public string vr_rooms_pass;
        public string vr_rooms_title;
        public string vr_rooms_settings;
        public int vr_rooms_mediaen;
        public int vr_rooms_vtype;
        public int vr_rooms_ven;
        public int vr_rooms_firstteam;
        public int vr_rooms_en3d;
        public int vr_rooms_vrange;
        public float vr_rooms_maxhdis;
        public int vr_rooms_adis;
        public string vr_rooms_gmeappid;
        public string vr_rooms_gmeappkey;
        public string vr_rooms_gmeroomid;
        public int vr_rooms_gmetxtenabled;
        public string vr_rooms_voiceapi;
        public string vr_rooms_actionapi;
        public string vr_rooms_tbpapi;
        public int vr_rooms_exenable;
        public int vr_rooms_chenable;
        public string vr_rooms_icon;
        public int vr_rooms_max;
        public int vr_rooms_aemax;
        public string vr_rooms_admincmd;
        public string vr_rooms_token;
        public string vr_scenes_id;
        public string create_time;
        public string update_time;
        public string vr_rooms_config;
        public int vr_rooms_share;
        public int vr_group_max;
        public int vr_rooms_vip;
        public int vr_rooms_bind;
    }

    public class SDKUIAvatarData
    {
        public int public_avatars_id;
        public string public_avatars_mark;
        public string vr_avatars_id;
        public int public_avatars_sort;
        public int public_avatars_enable;
        public string app_id;
        public string vr_avatars_path;
        public string vr_avatars_bundle;
        public string vr_avatars_glb;
        public string vr_avatars_glbkind;
        public string vr_avatars_version;
        public string vr_avatars_name;
        public int vr_avatars_gender;
        public int vr_avatars_glbv;
        public string vr_avatars_intro;
        public string vr_avatars_icon;
        public int vr_avatars_sort;
        public int vr_avatars_enabled;
        public string create_time;
        public string update_time;
        public string group_id;
        public string vr_avatars_show;
        public int is_top;
        public int is_public_recommend;
    }

    public class SDKUIUserData
    {
        public string vr_user_nickname;
        public string vr_user_id;
        public int vr_user_sex;
    }

    public class SDKUIGroup
    {
        public string vr_user_id;
        public string vr_user_name;
        public string vr_user_country;
        public string vr_user_phone;
        public string vr_user_userpass;
        public string vr_user_intro;
        public string vr_user_nickname;
        public string vr_user_icon;
        public string vr_user_ustream;
        public string vr_user_stream;
        public int vr_user_sex;
        public string vr_user_regapp;
        public string vr_user_token;
        public string vr_user_logintime;
        public int user_group_id;
        public string vr_group_id;
        public string user_group_pass;
        public string user_group_name;
        public int user_group_single;
        public string name;
        public int user_group_enabled;
        public int user_group_enhide;
        public string vr_group_name;
        public string vr_group_intro;
        public string vr_group_icon;
        public int vr_group_filekind;
        public string vr_group_fileserver;
        public string vr_group_fileurl;
        public string vr_group_fileweb;
        public string vr_group_fileadmin;
        public string vr_group_filepass;
        public string vr_group_linkroomid;
        public int user_max_num;
        public string initial_admin;
        public string initial_password;
        public string create_time;
        public string update_time;
        public string app_id;
        public string vr_group_alias;

    }


}

