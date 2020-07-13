using UnityEditor;
using UnityEngine.UI;

namespace HutongGames.PlayMakerEditor
{
    [InitializeOnLoad]
	public static class UiIcons
	{       
        static UiIcons()
        {
            Actions.AddCategoryIcon("UI", typeof(Button));
            Actions.AddCategoryIcon("GUI", typeof(Button));
            Actions.AddCategoryIcon("GUILayout", typeof(GridLayoutGroup));
            Actions.AddCategoryIcon("Input", typeof(InputField));
        }
	}
}