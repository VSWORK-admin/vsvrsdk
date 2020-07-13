using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public class WsBigScreenController : MonoBehaviour
{
    public Transform StartBigScreen;
    public bool StartEnabled = true;
    public Transform[] ScreenMarks;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in ScreenMarks)
        {
            item.gameObject.SetActive(false);
        }
        int ScreenAngle = StartBigScreen.GetComponent<BigScreenSelectController>().ScreenAngle;

        WsBigScreen startscreen = new WsBigScreen()
        {
            enabled = StartEnabled,
            angle = ScreenAngle,
            position = StartBigScreen.position,
            rotation = StartBigScreen.rotation,
            scale = StartBigScreen.lossyScale
        };

        MessageDispatcher.SendMessage(this,VrDispMessageType.BigScreenSetPos.ToString(),startscreen,0);
        MessageDispatcher.AddListener(VrDispMessageType.BigScreenStartAnchor.ToString(), BigScreenStartAnchor);
        MessageDispatcher.AddListener(VrDispMessageType.BigScreenEndAnchor.ToString(), BigScreenEndAnchor);
    }


    private void OnDestroy()
    {
        MessageDispatcher.RemoveListener(VrDispMessageType.BigScreenStartAnchor.ToString(), BigScreenStartAnchor);
        MessageDispatcher.RemoveListener(VrDispMessageType.BigScreenEndAnchor.ToString(), BigScreenEndAnchor);
    }

    void BigScreenStartAnchor(IMessage msg)
    {
        foreach (var item in ScreenMarks)
        {
            item.gameObject.SetActive(true);
        }
    }


    void BigScreenEndAnchor(IMessage msg)
    {
        foreach (var item in ScreenMarks)
        {
            item.gameObject.SetActive(false);
        }
    }
}
