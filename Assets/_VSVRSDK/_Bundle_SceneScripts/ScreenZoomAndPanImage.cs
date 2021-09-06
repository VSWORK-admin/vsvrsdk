using UnityEngine;
using System.Collections;

public class ScreenZoomAndPanImage : MonoBehaviour
{

    public Transform ImageTarget;
    //private Transform targetP;

    public float dist;

    private float panSpeedX;
    private float panSpeedY;
    private float zoomSpeed;

    public float PanxSpeedModifier = 0.2f;
    public float PanySpeedModifier = 0.2f;
    public float zoomSpeedModifier = 5;

    public float yclampmin = -100;
    public float yclampmax = 100;
    public float xclampmin = -100;
    public float xclampmax = 100;

    public float zoomclampmin = 1f;
    public float zoomclampmax = 10f;
    //public float panSpeedModifier=1f;

    // Use this for initialization
    void Start()
    {
        dist = ImageTarget.localScale.x;
    }

    void OnEnable()
    {
        IT_Gesture.onDraggingE += OnDragging;
        //IT_Gesture.onMFDraggingE += OnMFDragging;
        IT_Gesture.onPinchE += OnPinch;
    }

    void OnDisable()
    {
        IT_Gesture.onDraggingE -= OnDragging;
        //IT_Gesture.onMFDraggingE -= OnMFDragging;
        IT_Gesture.onPinchE -= OnPinch;
    }





    // Update is called once per frame
    void Update()
    {
        if (!ImageTarget.gameObject.activeInHierarchy) return;

        ImageTarget.localScale = new Vector3(dist, dist, dist);

        float x = ImageTarget.GetComponent<RectTransform>().anchoredPosition.x;
        float y = ImageTarget.GetComponent<RectTransform>().anchoredPosition.y;
        x = Mathf.Clamp(x + panSpeedX, xclampmin, xclampmax);
        y = Mathf.Clamp(y + panSpeedY, yclampmin, yclampmax);
        ImageTarget.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        //calculate the zoom and apply it
        dist += Time.deltaTime * zoomSpeed * 0.01f;
        dist = Mathf.Clamp(dist, zoomclampmin, zoomclampmax);
        //transform.localPosition=new Vector3(0, 0, dist);

        //reduce all the speed
        panSpeedX *= (1 - Time.deltaTime * 12);
        panSpeedY *= (1 - Time.deltaTime * 3);
        zoomSpeed *= (1 - Time.deltaTime * 4);

        //use mouse scroll wheel to simulate pinch, sorry I sort of cheated here
        zoomSpeed += Input.GetAxis("Mouse ScrollWheel") * 500 * zoomSpeedModifier;
    }

    //called when one finger drag are detected
    void OnDragging(DragInfo dragInfo)
    {
        if (!ImageTarget.gameObject.activeInHierarchy) return;
        //apply the DPI scaling
        dragInfo.delta /= IT_Gesture.GetDPIFactor();
        //vertical movement is corresponded to rotation in x-axis
        panSpeedY = dragInfo.delta.y * PanxSpeedModifier;
        //horizontal movement is corresponded to rotation in y-axis
        panSpeedX = dragInfo.delta.x * PanySpeedModifier;
    }

    //called when pinch is detected
    void OnPinch(PinchInfo pinfo)
    {
        if (!ImageTarget.gameObject.activeInHierarchy) return;
        zoomSpeed -= pinfo.magnitude * zoomSpeedModifier / IT_Gesture.GetDPIFactor();
    }

}
