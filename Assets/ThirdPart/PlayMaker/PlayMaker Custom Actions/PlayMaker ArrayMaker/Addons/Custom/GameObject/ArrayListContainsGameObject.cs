//	(c) Jean Fabre, 2011-2013 All rights reserved.
//	http://www.fabrejean.net

// INSTRUCTIONS
// Drop a PlayMakerArrayList script onto a GameObject, and define a unique name for reference if several PlayMakerArrayList coexists on that GameObject.
// In this Action interface, link that GameObject in "arrayListObject" and input the reference name if defined. 
// Note: You can directly reference that GameObject or store it in an Fsm variable or global Fsm variable

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Check if a GameObject ( by name and/or tag) is within an arrayList.")]
	public class ArrayListContainsGameObject : ArrayListActions
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;
		
		[Tooltip("The name of the GameObject to find in the arrayList. You can leave this empty if you specify a Tag.")]
		public FsmString gameObjectName;

		[UIHint(UIHint.Tag)]
        [Tooltip("Find a GameObject in this arrayList with this tag. If GameObject Name is specified then both name and Tag must match.")]
		public FsmString withTag;

		[ActionSection("Result")]
		
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a GameObject variable.")]
		public FsmGameObject result;
		
		[UIHint(UIHint.Variable)]
		public FsmInt resultIndex;
		
		[Tooltip("Store in a bool wether it contains or not that GameObject")]
		[UIHint(UIHint.Variable)]
		public FsmBool isContained;	
		
		[Tooltip("Event sent if this arraList contains that GameObject")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent isContainedEvent;	

		[Tooltip("Event sent if this arraList does not contains that GameObject")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent isNotContainedEvent;
		
		
		
		public override void Reset()
		{
		
			gameObject = null;
			reference = null;
			
			gameObjectName = null;
			
			result = null;
			resultIndex = null;
			
			isContained = null;
			isContainedEvent = null;
			isNotContainedEvent = null;
			
		}
		
		
		public override void OnEnter()
		{

			if (! SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
			{
				Finish();
			}
			
			int elementIndex = DoContainsGo();
			if (elementIndex>=0)
			{
				isContained.Value = true;
				result.Value = (GameObject)proxy.arrayList[elementIndex];
				resultIndex.Value = elementIndex;
				Fsm.Event(isContainedEvent);
			}else{
				isContained.Value = false;	
				Fsm.Event(isNotContainedEvent);
			}

			Finish();
		}
		

		int DoContainsGo()
		{
			
			if (! isProxyValid())
			{
				return -1;
			}
			
			int _index =0;
			
			string _nameToken = gameObjectName.Value;
			string _tagToken = withTag.Value;
			
			foreach(GameObject _go in proxy.arrayList)
			{
				
				if (_go!=null) 
				{
					if (_tagToken == "Untagged" || withTag.IsNone)
					{
						
						if (_go.name.Equals(_nameToken))
						{
							return _index;
						}
					}else{
					
						if (string.IsNullOrEmpty(_nameToken))
						{
							if (_go.tag.Equals(_tagToken))
							{
								return _index;
							}
						}else{
							
							if (_go.name.Equals(_nameToken) && _go.tag.Equals(_tagToken))
							{
								return _index;
							}
						}
					}
					
					
				}
				_index++;
			}
			
			
			
			return -1;
		}
		
	}
}