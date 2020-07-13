using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
using UnityEngine.UI;
public class VRImageShow : MonoBehaviour
{
    public List<GameObject> Screens;
    public List<RawImage> images;
    void Start()
    {
        MessageDispatcher.AddListener(VrDispMessageType.BigScreenShowImage.ToString(), BigScreenShowImage);
    }

    private void OnDestroy()
    {
        MessageDispatcher.RemoveListener(VrDispMessageType.BigScreenShowImage.ToString(), BigScreenShowImage);
    }

    void BigScreenShowImage(IMessage msg)
    {
        Texture2D texture = msg.Data as Texture2D;


        foreach (var item in Screens)
        {
            item.GetComponent<Renderer>().material.SetTexture("_BaseMap", texture);
        }

        foreach (var item in images)
        {
            item.texture = texture;
        }

    }
}
