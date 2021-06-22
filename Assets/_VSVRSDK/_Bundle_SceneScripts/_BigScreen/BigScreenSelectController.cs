using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
using DG.Tweening;
public class BigScreenSelectController : MonoBehaviour
{
    public int ScreenAngle;
    bool pointed = false;
    public Vector3 startscale;

    // Start is called before the first frame update
    void Start()
    {
        startscale = transform.localScale;
        MessageDispatcher.AddListener(VRPointObjEventType.VRPointEnter.ToString(), VRPointEnter);
        MessageDispatcher.AddListener(VRPointObjEventType.VRPointExit.ToString(), VRPointExit);
        MessageDispatcher.AddListener(CommonVREventType.VRRaw_RightTrigger.ToString(),VRCommitButtonClick);
    }

    private void OnDestroy()
    {
        MessageDispatcher.RemoveListener(VRPointObjEventType.VRPointEnter.ToString(), VRPointEnter,true);
        MessageDispatcher.RemoveListener(VRPointObjEventType.VRPointExit.ToString(), VRPointExit,true);
        MessageDispatcher.RemoveListener(CommonVREventType.VRRaw_RightTrigger.ToString(),VRCommitButtonClick,true);
    }

    void VRCommitButtonClick(IMessage msg){
        if(pointed){
            WsBigScreen wbs = new WsBigScreen
            {
                id = mStaticThings.I.mAvatarID,
                enabled = true,
                angle = ScreenAngle,
                position = transform.position,
                rotation = transform.rotation,
                scale = startscale
            };
            MessageDispatcher.SendMessage(this,VrDispMessageType.BigScreenEndAnchor.ToString(),wbs,0);
        }
    }

    void VRPointEnter(IMessage msg)
    {
        if ((GameObject)msg.Data == gameObject)
        {
            pointed = true;
            float newscale = startscale.x * 1.6f;

            transform.DOScale(new Vector3(newscale, newscale, newscale), 0.6f).SetEase(Ease.OutBounce).SetId(transform);
        }
    }

    void VRPointExit(IMessage msg)
    {
        if ((GameObject)msg.Data == gameObject)
        {
            pointed = false;
            transform.DOScale(startscale, 0.6f).SetId(transform);
        }
    }
}
