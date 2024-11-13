using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
#if BAKE_GAUSSION_SPLAT
[ExecuteInEditMode]
#endif
public class GaussianSplattingModel : MonoBehaviour
{
    public string modelFilePath = "";
    public Bounds cropBox;
    private GaussianSplatRenderer gs;

    private void Awake()
    {
        //gs = FindObjectOfType<GaussianSplatRenderer>();
        gs = GaussianSplatRenderer.Instance;
        if(gs != null)
        {
            gs.RegisterModel(this);
        }
    }
    [ContextMenu("Load")]
    public void RegisterModel()
    {
        gs = GaussianSplatRenderer.Instance;
        if (gs != null)
        {
            gs.RegisterModel(this);
        }
    }

    public void UnRegisterModel()
    {
        gs.UnRegisterModel(this);
    }
    private void OnDestroy()
    {
        gs.UnRegisterModel(this);
    }
}
