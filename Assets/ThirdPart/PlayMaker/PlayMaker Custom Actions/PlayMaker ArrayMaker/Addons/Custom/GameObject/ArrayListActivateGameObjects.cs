//	(c) Jean Fabre, 2011-2015 All rights reserved.
//	http://www.fabrejean.net

// INSTRUCTIONS
// Drop a PlayMakerArrayList script onto a GameObject, and define a unique name for reference if several PlayMakerArrayList coexists on that GameObject.
// In this Action interface, link that GameObject in "arrayListObject" and input the reference name if defined. 
// Note: You can directly reference that GameObject or store it in an Fsm variable or global Fsm variable

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Active/Deactivate all GameObjects within an arrayList.")]
	public class ArrayListActivateGameObjects : ArrayListActions
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;
		
		[RequiredField]
		[Tooltip("Check to activate, uncheck to deactivate Game Objects.")]
		public FsmBool activate;
		
		[Tooltip("Resets the affected game objects when exiting this state to their original activate state. Useful if you want an object to be controlled only while this state is active.")]
		public FsmBool resetOnExit;


		bool[] _activeStates;

		
		public override void Reset()
		{
			gameObject = null;
			activate = null;
			resetOnExit = false;
		}
		
		
		public override void OnEnter()
		{

			if (! SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
			{
				Finish();
			}
			
			DoAction();

			Finish();

		}

		
		void DoAction()
		{
			
			if (! isProxyValid())
			{
				return;
			}
			
			_activeStates = new bool[proxy.arrayList.Count];

			int i= 0;
			foreach(GameObject _go in proxy.arrayList)
			{	
				if (_go==null)
				{
					continue;
				}

				#if UNITY_3_5 || UNITY_3_4
				_activeStates[i] = _go.active;
				_go.active = activate.Value;
				#else			
				_activeStates[i] = _go.activeSelf;
				_go.SetActive(activate.Value);
				#endif
				
				i++;
			}
		}

		public override void OnExit()
		{
			if( resetOnExit.Value && _activeStates!=null)
			{
				int i= 0;
				foreach(GameObject _go in proxy.arrayList)
				{	
					if (_go==null)
					{
						continue;
					}
					
					#if UNITY_3_5 || UNITY_3_4
					_go.active = _activeStates[i];
					#else			
					_go.SetActive(_activeStates[i]);
					#endif
					
					i++;
				}
			}
		}
		
	}
}