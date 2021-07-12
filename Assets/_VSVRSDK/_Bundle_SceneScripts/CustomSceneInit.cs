using UnityEngine;

public class CustomSceneInit : MonoBehaviour
{
    public Material skymat;
    // Start is called before the first frame update
    void Start()
    {
        //if (GameManager.Instance.sceneLoader == null) { return; }

        if(skymat != null){
            RenderSettings.skybox = skymat;
        }

    }
}
