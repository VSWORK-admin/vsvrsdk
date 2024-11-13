using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(GaussianSplatRenderer))]
public class Info : MonoBehaviour, GaussianSplatRenderer.Observer
{
	#region Members
	public string lastMessage = "";
	public int nb_splats = 0;

	protected GaussianSplatRenderer gs;
	#endregion

	#region MonoBehaviour methods
	protected void Start()
	{
		gs = GetComponent<GaussianSplatRenderer>();
		
		gs.AddObserver(this);
	}

	protected void Update()
	{
		if (gs.state >= GaussianSplatRenderer.State.DISABLED)
		{
			lastMessage = GaussianSplatRenderer.Native.GetLastMessage();
			nb_splats = GaussianSplatRenderer.Native.GetNbSplat();
		}
	}
	#endregion

	#region Observer implementation
	public void OnStateChanged(GaussianSplatRenderer gs, GaussianSplatRenderer.State state)
	{
		switch (state)
		{
			case GaussianSplatRenderer.State.DISABLED:
				lastMessage = "";
				nb_splats = 0;

				break;
			default:
				break;
		}
	}
	#endregion
}
