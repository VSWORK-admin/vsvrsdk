using UnityEngine;
using System.Collections;

public class ArrayListTable : MonoBehaviour {


	public PlayMakerArrayListProxy HeaderProxy;

	public PlayMakerArrayListProxy[] ColumnData;


	public string GetColumnHeader(int index)
	{
		if (HeaderProxy == null)
		{
			return string.Empty;
		}

		if (index < 0 || index >= HeaderProxy.arrayList.Count)
		{
			return string.Empty;
		}


		return HeaderProxy.arrayList[index].ToString();
	}



}
