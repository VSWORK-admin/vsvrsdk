// (c) Copyright HutongGames, LLC. All rights reserved.

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
#if !UNITY_2019_3_OR_NEWER

	[ActionCategory(ActionCategory.GUIElement)]
	[Tooltip("Sets the Text used by the GUIText Component attached to a Game Object.")]
	#if UNITY_2017_2_OR_NEWER
	#pragma warning disable CS0618  
	[Obsolete("GUIText is part of the legacy UI system and will be removed in a future release")]
	#endif
	public class SetGUIText : ComponentAction<GUIText>
	{
		[RequiredField]
		[CheckForComponent(typeof(GUIText))]
		public FsmOwnerDefault gameObject;

        [UIHint(UIHint.TextArea)]
		public FsmString text;

		public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			text = "";
		}

		public override void OnEnter()
		{
			DoSetGUIText();

		    if (!everyFrame)
		    {
		        Finish();
		    }
		}
		
		public override void OnUpdate()
		{
			DoSetGUIText();
		}
		
		void DoSetGUIText()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
		    if (UpdateCache(go))
		    {
		        guiText.text = text.Value;
		    }
		}
	}
#else

    [ActionCategory(ActionCategory.GUIElement)]
    [Tooltip("Sets the Text used by the GUIText Component attached to a Game Object.")]
    [Obsolete("GUIText is part of the legacy UI system removed in 2019.3")]
    public class SetGUIText : FsmStateAction
    {
        [ActionSection("Obsolete. Use Unity UI instead.")]

        [UIHint(UIHint.TextArea)]
        public FsmString text;
    }

#endif
}

