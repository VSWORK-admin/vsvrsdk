using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VRUISelectorProxy : MonoBehaviour
{

    public bool IsSystemMenu = true;
    private void Start()
    {
        Init();
    }
    // Start is called before the first frame update
    public void Init()
    {
        Button[] buttons = GetComponentsInChildren<UnityEngine.UI.Button>();
        foreach (Button btn in buttons)
        {
            if (!btn.gameObject.GetComponent<BoxCollider>())
            {
                btn.gameObject.AddComponent<BoxCollider>().size = new Vector3(btn.GetComponent<RectTransform>().sizeDelta.x, btn.GetComponent<RectTransform>().sizeDelta.y, 0.2f);
                if (IsSystemMenu)
                {
                    btn.gameObject.AddComponent<VRUIsystemMenuMark>();
                }
            }
            if (!btn.gameObject.GetComponent<VRUISelectorMark>())
            {
                btn.gameObject.AddComponent<VRUISelectorMark>();
            }

        }

        Collider[] cds = GetComponentsInChildren<Collider>();
        if (IsSystemMenu)
        {
            foreach (Collider cd in cds)
            {
                if(!cd.gameObject.GetComponent<VRUIsystemMenuMark>()){
                    cd.gameObject.AddComponent<VRUIsystemMenuMark>();
                }
                
            }
        }


        InputField[] inputfields = GetComponentsInChildren<UnityEngine.UI.InputField>();
        foreach (InputField btn in inputfields)
        {
            if (!btn.gameObject.GetComponent<BoxCollider>())
            {
                btn.gameObject.AddComponent<BoxCollider>().size = new Vector3(btn.GetComponent<RectTransform>().sizeDelta.x, btn.GetComponent<RectTransform>().sizeDelta.y, 0.2f);
                if (IsSystemMenu)
                {
                    btn.gameObject.AddComponent<VRUIsystemMenuMark>();
                }
            }
            if (!btn.gameObject.GetComponent<VRUISelectorMark>())
            {
                btn.gameObject.AddComponent<VRUISelectorMark>();
            }
        }

        Toggle[] toggles = GetComponentsInChildren<UnityEngine.UI.Toggle>();
        foreach (Toggle btn in toggles)
        {
            if (!btn.gameObject.GetComponent<BoxCollider>())
            {
                btn.gameObject.AddComponent<BoxCollider>().size = new Vector3(btn.GetComponent<RectTransform>().sizeDelta.x, btn.GetComponent<RectTransform>().sizeDelta.y, 0.2f);
                if (IsSystemMenu)
                {
                    btn.gameObject.AddComponent<VRUIsystemMenuMark>();
                }
            }
            if (!btn.gameObject.GetComponent<VRUISelectorMark>())
            {
                btn.gameObject.AddComponent<VRUISelectorMark>();
            }
        }
    }
}
