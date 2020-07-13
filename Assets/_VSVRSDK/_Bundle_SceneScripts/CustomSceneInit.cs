using UnityEngine;

public class CustomSceneInit : MonoBehaviour
{
    public Material skymat;
    // Start is called before the first frame update
    void Start()
    {
        //if (WsSceneLoader.I == null) { return; }

        if(skymat != null){
            RenderSettings.skybox = skymat;
        }

    }
}
