// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.XPath;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Xml")]
	[Tooltip("Operates Maths on nodelist content.")]
	public class XmlNodesMathOperator : DataMakerXmlActions
	{
		public enum NodeListOperators {Add,Subtract,Multiply,Divide,Min,Max};

		[ActionSection("XML Source")]
		
		public FsmString nodeListReference;
		
		[ActionSection("Set up")]
		
		[Tooltip("Set to true to force iterating from the value of the index variable. This variable will be set to false as it carries on iterating, force it back to true if you want to renter this action back to the first item.")]
		[UIHint(UIHint.Variable)]
		public NodeListOperators operation;
		
		[ActionSection("Result")]
		
		[Tooltip("Operation result as int")]
		[UIHint(UIHint.Variable)]
		public FsmInt resultAsInt;

		[Tooltip("Operation result as float")]
		[UIHint(UIHint.Variable)]
		public FsmFloat resultAsFloat;
		
		[Tooltip("Event to send when likely no nodelist was passed.")]
		public FsmEvent errorEvent;

		private XmlNodeList _nodeList;
		
		public override void Reset()
		{
			nodeListReference = null;
			
			operation = NodeListOperators.Add;
			resultAsInt = new FsmInt(){UseVariable=true};
			resultAsFloat = new FsmFloat(){UseVariable=true};
		}
		
		public override void OnEnter()
		{

			_nodeList = DataMakerXmlUtils.XmlRetrieveNodeList(nodeListReference.Value);
			if (_nodeList==null)
			{
				Fsm.Event(errorEvent);

			}else{
				DoOperation();
			}
			
			Finish();
		}

		void DoOperation()
		{
			if (_nodeList==null)
			{
				return;
			}

			float _result = 0f;
			if (operation == NodeListOperators.Min)
			{
				_result = float.MaxValue;
			}else if (operation == NodeListOperators.Max)
			{
				_result = float.MinValue;
			}


			float _localValue;
			foreach(XmlNode _node in _nodeList)
			{
					
				if( ! float.TryParse(_node.InnerText,out _localValue) )
				{
					continue;
				}

				switch (operation)
				{
				case NodeListOperators.Add:
					_result += _localValue;
					break;
					
				case NodeListOperators.Subtract:
					_result -= _localValue;
					break;
					
				case NodeListOperators.Multiply:
					_result *= _localValue;
					break;
					
				case NodeListOperators.Divide:
					if (_localValue!=0)
					{
						_result /= _localValue;
					}
					break;

				case NodeListOperators.Min:
					if (_localValue < _result)
					{
						_result = _localValue;
					}
					break;
					
				case NodeListOperators.Max:
					if (_localValue > _result)
					{
						_result = _localValue;
					}
					break;
				}

			}

			if (!resultAsInt.IsNone)
			{
				resultAsInt.Value = (int)_result;
			}

			if (!resultAsFloat.IsNone)
			{
				resultAsFloat.Value = _result;
			}

		}
		
		
	}
}