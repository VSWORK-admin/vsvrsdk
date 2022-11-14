using com.ootii.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSEngine
{
    public class VsScene : BaseFunction
    {
        public VsScene() : base(FunctionType.VsScene)
        {

        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        internal override void Awake()
        {
            base.Awake();
        }

        internal override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();
        }

        internal override void OnDisable()
        {
            base.OnDisable();
        }

        internal override void OnEnable()
        {
            base.OnEnable();
        }

        internal override void Start()
        {
            base.Start();
        }

        internal override void Update()
        {
            base.Update();
        }

        #region Event

        #endregion

        #region API
        /// <summary>
        /// 通过URL加载场景
        /// </summary>
        /// <param name="server"></param>
        /// <param name="id"></param>
        /// <param name="isnowserver"></param>
        /// <param name="update"></param>
        public static void SetLoadUrlIdScene(string server, string id, bool isnowserver, bool update)
        {
            URLIDSceneInfo urlinfo = new URLIDSceneInfo()
            {
                server = server,
                id = id,
                isnowserver = isnowserver,
                update = update
            };

            MessageDispatcher.SendMessageData(VrDispMessageType.DownloadURLIDScene.ToString(), urlinfo, 0);
        }
        #endregion
    }
}