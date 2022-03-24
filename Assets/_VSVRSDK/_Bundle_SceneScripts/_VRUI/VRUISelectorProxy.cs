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
        Button[] buttons = GetComponentsInChildren<UnityEngine.UI.Button>(true);
        foreach (Button btn in buttons)
        {
            if (!btn.gameObject.GetComponent<BoxCollider>())
            {
                btn.gameObject.AddComponent<BoxCollider>().size = new Vector3(btn.GetComponent<RectTransform>().sizeDelta.x, btn.GetComponent<RectTransform>().sizeDelta.y, 0.02f);
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

        Collider[] cds = GetComponentsInChildren<Collider>(true);

        foreach (Collider cd in cds)
        {
            if (IsSystemMenu)
            {
                if (!cd.gameObject.GetComponent<VRUIsystemMenuMark>())
                {
                    cd.gameObject.AddComponent<VRUIsystemMenuMark>();
                }
            }
            if (!cd.gameObject.GetComponent<VRUISelectorMark>())
            {
                cd.gameObject.AddComponent<VRUISelectorMark>();
            }
        }

        InputField[] inputfields = GetComponentsInChildren<UnityEngine.UI.InputField>(true);
        foreach (InputField btn in inputfields)
        {
            if (!btn.gameObject.GetComponent<BoxCollider>())
            {
                btn.gameObject.AddComponent<BoxCollider>().size = new Vector3(btn.GetComponent<RectTransform>().sizeDelta.x, btn.GetComponent<RectTransform>().sizeDelta.y, 0.02f);
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

        Toggle[] toggles = GetComponentsInChildren<UnityEngine.UI.Toggle>(true);
        foreach (Toggle btn in toggles)
        {
            if (!btn.gameObject.GetComponent<BoxCollider>())
            {
                btn.gameObject.AddComponent<BoxCollider>().size = new Vector3(btn.GetComponent<RectTransform>().sizeDelta.x, btn.GetComponent<RectTransform>().sizeDelta.y, 0.02f);
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
