using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using com.ootii.Messages;
public class HomeVRMenuController : MonoBehaviour
{
    private Transform lefthand;
    public bool isOnLefHand;
    public bool isOnBodyAnchor;
    public GameObject canvas;
    public GameObject cavasparent;
    bool isinit = false;
    public bool AdminControlOnly;

    public CommonVREventType toggleButton;

    public bool IsToggle;
    public CommonVREventType OnButton;
    public CommonVREventType OffButton;
    bool iscanvasshow = false;
    public bool isSelfShow = true;
    private void Start()
    {
        if (mStaticThings.I == null) { return; }

        lefthand = mStaticThings.I.LeftHand;

        Button[] buttons = GetComponentsInChildren<UnityEngine.UI.Button>();
        foreach (Button btn in buttons)
        {
            //Debug.LogWarning("BIND : "  + btn.name);
            btn.onClick.AddListener(() => { OnButtonClicked(btn); });
        }

        canvas.GetComponent<VRUISelectorProxy>().Init();
        isinit = true;

        MessageDispatcher.AddListener(VrDispMessageType.SetAdmin.ToString(), SetAdmin);

        MessageDispatcher.AddListener(CommonVREventType.VRRaw_Start_ButtonClick.ToString(), DeactiveCanvas);
        MessageDispatcher.AddListener(toggleButton.ToString(), ToggleCanvas);
        MessageDispatcher.AddListener(OnButton.ToString(), ToggleCanvas);
        MessageDispatcher.AddListener(OffButton.ToString(), ToggleCanvas);
        //MessageDispatcher.AddListener(CommonVREventType.VRRaw_Y_ButtonClick.ToString(), DeactiveCanvas);
        cavasparent.SetActive(false);
        MessageDispatcher.AddListener(VrDispMessageType.SceneMenuEnable.ToString(), SceneMenuAction);
        MessageDispatcher.AddListener(VrDispMessageType.SystemMenuEvent.ToString(), SystemMenuEvent);
    }

    private void OnDestroy()
    {
        MessageDispatcher.RemoveListener(VrDispMessageType.SceneMenuEnable.ToString(), SceneMenuAction);
        MessageDispatcher.RemoveListener(VrDispMessageType.SystemMenuEvent.ToString(), SystemMenuEvent);

        MessageDispatcher.RemoveListener(VrDispMessageType.SetAdmin.ToString(), SetAdmin);
        MessageDispatcher.RemoveListener(CommonVREventType.VRRaw_Start_ButtonClick.ToString(), DeactiveCanvas);
        MessageDispatcher.RemoveListener(toggleButton.ToString(), ToggleCanvas);
        MessageDispatcher.RemoveListener(OnButton.ToString(), ToggleCanvas);
        MessageDispatcher.RemoveListener(OffButton.ToString(), ToggleCanvas);
        //MessageDispatcher.RemoveListener(CommonVREventType.VRRaw_Y_ButtonClick.ToString(), DeactiveCanvas);
    }

    void SceneMenuAction(IMessage msg)
    {
        bool ison = (bool)msg.Data;
        iscanvasshow = ison;
        cavasparent.SetActive(ison);
        if (isSelfShow)
        {
            MessageDispatcher.SendMessage(this, VrDispMessageType.SystemMenuEnable.ToString(), false, 0);
        }
    }

    void ToggleCanvas(IMessage msg)
    {
        if (AdminControlOnly)
        {
            if (!mStaticThings.I.isAdmin && !mStaticThings.I.sadmin)
            {
                iscanvasshow = false;
                cavasparent.SetActive(iscanvasshow);
                return;
            }
        }
        if (msg.Type == toggleButton.ToString())
        {
            if (IsToggle && mStaticThings.I.WsAvatarIsReady)
            {
                MessageDispatcher.SendMessage(VrDispMessageType.SetMenuRootPos.ToString());
                iscanvasshow = !iscanvasshow;
                cavasparent.SetActive(iscanvasshow);
                if (iscanvasshow && isSelfShow)
                {
                    MessageDispatcher.SendMessage(this, VrDispMessageType.SystemMenuEnable.ToString(), false, 0);
                }
            }
        }
        else
        {
            if (!IsToggle)
            {
                if (msg.Type == OnButton.ToString() && mStaticThings.I.WsAvatarIsReady)
                {
                    MessageDispatcher.SendMessage(VrDispMessageType.SetMenuRootPos.ToString());
                    cavasparent.SetActive(true);
                    iscanvasshow = true;
                    if (isSelfShow)
                {
                    MessageDispatcher.SendMessage(this, VrDispMessageType.SystemMenuEnable.ToString(), false, 0);
                }
                }
                else if (msg.Type == OffButton.ToString())
                {
                    iscanvasshow = false;
                    cavasparent.SetActive(false);
                }
            }
        }
    }

    void SystemMenuEvent(IMessage msg)
    {
        bool en = (bool)msg.Data;
        if (en)
        {
            if (isSelfShow)
            {
                iscanvasshow = false;
                cavasparent.SetActive(false);
            }
        }
        else
        {
            iscanvasshow = false;
            cavasparent.SetActive(false);
        }
    }

    void DeactiveCanvas(IMessage msg)
    {
        if (!isSelfShow)
        {
            cavasparent.SetActive(false);
        }
    }

    void SetAdmin(IMessage msg)
    {
        if (!isinit) return;
        if (AdminControlOnly)
        {
            if (!mStaticThings.I.isAdmin && !mStaticThings.I.sadmin)
            {
                iscanvasshow = false;
                cavasparent.SetActive(iscanvasshow);
            }
        }
    }
    void Update()
    {
        if (mStaticThings.I == null) { return; }
        if (AdminControlOnly && !mStaticThings.I.isAdmin && !mStaticThings.I.sadmin) { return; }
        if (!iscanvasshow)
        {
            if (cavasparent.activeSelf)
            {
                cavasparent.SetActive(false);
            }
            return;
        }
        if (isOnLefHand)
        {
            transform.position = lefthand.position;
            transform.rotation = lefthand.rotation;
            transform.localScale = lefthand.lossyScale;
        }
        else if (isOnBodyAnchor)
        {
            transform.position = mStaticThings.I.MenuAnchor.position;
            transform.rotation = mStaticThings.I.MenuAnchor.rotation;
            transform.localScale = mStaticThings.I.MainVRROOT.lossyScale;
        }
    }

    private void OnButtonClicked(Button bt)
    {
        if (bt.name.Length > 7 && bt.name.Substring(0, 7) == "change_")
        {
            string[] infos = new string[7];
            infos = bt.name.Split('_');
            string a = infos.Length > 1 ? infos[1] : "";
            string b = infos.Length > 2 ? infos[2] : "";
            string c = infos.Length > 3 ? infos[3] : "";
            string d = infos.Length > 4 ? infos[4] : "";
            string e = infos.Length > 5 ? infos[5] : "";
            string f = infos.Length > 6 ? infos[6] : "";
            string g = infos.Length > 7 ? infos[7] : "";
            WsCChangeInfo wsinfo = new WsCChangeInfo()
            {
                a = a,
                b = b,
                c = c,
                d = d,
                e = e,
                f = f,
                g = g,
            };
            MessageDispatcher.SendMessage(this, WsMessageType.SendCChangeObj.ToString(), wsinfo, 0);
        }
        else if (bt.name.Length > 8 && bt.name.Substring(0, 8) == "placeto_")
        {
            MessageDispatcher.SendMessage(true, VrDispMessageType.AllPlaceTo.ToString(), bt.name.Substring(8, bt.name.Length - 8), 0);
        }
    }
}
