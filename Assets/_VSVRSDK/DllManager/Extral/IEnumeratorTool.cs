using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IEnumeratorTool : MonoBehaviourInstance<IEnumeratorTool>
{
	WaitForEndOfFrame m_WaitForEndOfFrame = new WaitForEndOfFrame();
	public WaitForEndOfFrame waitForEndOfFrame
	{
		get { return m_WaitForEndOfFrame; }
	}
}

