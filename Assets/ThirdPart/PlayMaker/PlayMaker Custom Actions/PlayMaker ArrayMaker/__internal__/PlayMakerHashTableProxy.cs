//	(c) Jean Fabre, 2011-2018

// INSTRUCTIONS
// Drop this script onto a GameObject.
//
// -- PLayMaker Events:
// PLayMaker Events can be dispatched when items are added, set or removed from this HashTable. 
// you can enable/Disable this feature
// Fill each events with the relevent Events name. Make sure they are actually defined in PlayMaker AND used in Fsm as GlobalTransitions.
//
// -- PreFill: 
// You can define during authoring the content of this HashTable. Simply reveal the Prefill tab in the inspector. 
// select a type from the list
// Select the number of items
// fill each items with your key and Data.
// When the scene starts, the array will be filled with this data
//
// LIMITATIONS: Keys are for now only strings. I might add a more flexible way to defines other types for keys 
//
// --HashTable Content viewing;
// During PlayBack, you can view and edit the content of the HashTable. 
// you can edit data on the fly
// You can preview only a portion of the content using a start index and the number of element to show ( maximum 30 elements at a time).
//
//
// -- Referencing Within PlayMaker
// Reference this PlayMakerHashTableProxy Component in "hashTableObject" [and optionaly provide a unique name reference] in one of the related playmaker action:
//   -- OR -- 
// Reference this PlayMakerHashTableProxy Component in an FsmObject Variable with type set to "PLayMakerHashTableProxy". Then use this variable for reference within the related Actions:
// The second technics works well when PlayMakerHashTableProxy is defined only once on a given gameObject. 
// Name reference becomes uncessary and within Actions, leave the reference name field blank too.
// Else, you can maintain a FsmString that contains the reference of that PlayMakerHashTableProxy and use the pair ( FsmObject AND FsmString ) within Actions.
//
// Common actions for HashTable
// HashTableAdd,
// HashTableClear,
// HashTableContains,
// HashTableContainsKey,
// HashTableContainsValue,
// HashTableCopyTo,
// HashTableCount,
// HashTableGet,
// HashTableKeys,
// HashTableSet,
// HashTableValues,
// If you need more actions, do not hesitate to contact the author
// note: You can directly reference the GameObject or store it in an Fsm variable or global Fsm variable for better referencing
//
//
// To manage this component on the fly, use one of the following actions:
//
// FindHashTable
// CreateHashTable
// DestroyHashTable
//
// What is the reference Name for?
//   Several PlayMakerHashTable can coexists on a given GameObject:
//   If only one PlayMakerHashTable is added to a given GameObject, the reference name is not necessary ( tho recommanded for easier evolution of your project).
//   If several PlayMakerHashTable coexists on a given GameObject, then it becomes necessary to properly provide unique references for stable and predictable behavior.


using UnityEngine;
using HutongGames.PlayMaker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class PlayMakerHashTableProxy : PlayMakerCollectionProxy {
	
	// the actual hashTable
	public Hashtable _hashTable;
	
	public Hashtable hashTable
    {
        get { return _hashTable; }
    }
	
	private Hashtable _snapShot;
	
	public void Awake()
	{	
		_hashTable = new Hashtable();
		
		PreFillHashTable();
		
		TakeSnapShot();
	}
	
	public bool isCollectionDefined()
	{
		return hashTable != null;
	}
	
	public void TakeSnapShot()
	{
		_snapShot = new Hashtable();
		foreach(object key in _hashTable.Keys){		
			_snapShot[key] = _hashTable[key];
		}
	}
	
	public void RevertToSnapShot()
	{
		_hashTable = new Hashtable();
		foreach(object key in _snapShot.Keys){		
			_hashTable[key] = _snapShot[key];
		}
	}
	
	
	public void InspectorEdit(int index){
		dispatchEvent(setEvent,index,"int");
	}
	
	[ContextMenu ("Copy HashTable Content")]
	private void CopyContentToPrefill()
	{

		this.preFillCount = hashTable.Count;

		preFillKeyList =  hashTable.Keys.OfType<string>().ToList();
	
		switch (this.preFillType) {
			
		case (VariableEnum.Bool):
			preFillBoolList = new List<bool>(new  bool[this.preFillCount]);
			break;
		case (VariableEnum.Color):
			preFillColorList = new List<Color>(new Color[this.preFillCount]);
			break;
		case (VariableEnum.Float):
			preFillFloatList= new List<float>(new float[this.preFillCount]);
			break;
		case (VariableEnum.GameObject):
			preFillGameObjectList = new List<GameObject>(new GameObject[this.preFillCount]);
			break;
		case (VariableEnum.Int):
			preFillIntList = new List<int>(new int[this.preFillCount]);
			break;
		case (VariableEnum.Material):
			preFillMaterialList = new List<Material>(this.preFillCount);
			break;
		case (VariableEnum.Quaternion):
			preFillQuaternionList = new List<Quaternion>(this.preFillCount);
			break;
		case (VariableEnum.Rect):
			preFillRectList	= new List<Rect>(this.preFillCount);
			break;
		case (VariableEnum.String):
			preFillStringList = new List<string>(new string[this.preFillCount]);  
			break;
		case (VariableEnum.Texture):
			preFillTextureList = new List<Texture2D>(this.preFillCount);
			break;
		case (VariableEnum.Vector2):
			preFillVector2List = new List<Vector2>(this.preFillCount);
			break;
		case (VariableEnum.Vector3):
			preFillVector3List = new List<Vector3>(new Vector3[this.preFillCount]);
			break;
		case (VariableEnum.AudioClip):
			preFillAudioClipList = new List<AudioClip>(this.preFillCount);
			break;
		case (VariableEnum.Byte):
			preFillByteList = new List<byte>(this.preFillCount);
			break;
		case (VariableEnum.Sprite):
			preFillSpriteList = new List<Sprite>(this.preFillCount);
			break;
		default:
			break;
		}


		for(int i=0;i<preFillKeyList.Count;i++)
		{
			switch (preFillType) {
			case (VariableEnum.Bool):
				preFillBoolList[i] = Convert.ToBoolean(hashTable[preFillKeyList[i]]);
				break;
			case (VariableEnum.Color):
				preFillColorList[i] =  PlayMakerUtils.ConvertToColor(hashTable[preFillKeyList[i]]);	
				break;
			case (VariableEnum.Float):
				preFillFloatList[i] = Convert.ToSingle(hashTable[preFillKeyList[i]]);
				break;
			case (VariableEnum.GameObject):
				preFillGameObjectList[i] = hashTable[preFillKeyList[i]] as GameObject;
				break;
			case (VariableEnum.Int):
				preFillIntList[i] = Convert.ToInt32(hashTable[preFillKeyList[i]]);	
				break;
			case (VariableEnum.Material):
				preFillMaterialList[i] = hashTable[preFillKeyList[i]] as Material;
				break;
			case (VariableEnum.Quaternion):
				preFillQuaternionList[i] = PlayMakerUtils.ConvertToQuaternion(hashTable[preFillKeyList[i]]);
				break;
			case (VariableEnum.Rect):
				preFillRectList[i] = PlayMakerUtils.ConvertToRect(hashTable[preFillKeyList[i]]);	
				break;
			case (VariableEnum.String):
				preFillStringList[i] = Convert.ToString(hashTable[preFillKeyList[i]]);	
				break;
			case (VariableEnum.Texture):
				preFillTextureList[i] = hashTable[preFillKeyList[i]] as Texture2D;	
				break;
			case (VariableEnum.Vector2):
				preFillVector2List[i] = (Vector2)hashTable[preFillKeyList[i]];		
				break;
			case (VariableEnum.Vector3):
				preFillVector3List[i] = PlayMakerUtils.ConvertToVector3(hashTable[preFillKeyList[i]]);		
				break;
			case (VariableEnum.AudioClip):
				preFillAudioClipList[i] = hashTable[preFillKeyList[i]] as AudioClip;		
				break;
			case (VariableEnum.Byte):
				preFillByteList[i] = Convert.ToByte(hashTable[preFillKeyList[i]]);		
				break;
			case (VariableEnum.Sprite):
				preFillSpriteList[i] = hashTable[preFillKeyList[i]]  as Sprite;		
				break;
			default:
				break;
			}
		}

		
		#if UNITY_EDITOR
		UnityEditor.Unsupported.CopyComponentToPasteboard(this);
		#endif 

	}
	private void PreFillHashTable()
	{
		for(int i=0;i<preFillKeyList.Count;i++)
		{
			switch (preFillType) {
				case (VariableEnum.Bool):
					hashTable[preFillKeyList[i]] = preFillBoolList[i];
					break;
				case (VariableEnum.Color):
					hashTable[preFillKeyList[i]] = preFillColorList[i];	
					break;
				case (VariableEnum.Float):
					hashTable[preFillKeyList[i]] = preFillFloatList[i];
					break;
				case (VariableEnum.GameObject):
					hashTable[preFillKeyList[i]] = preFillGameObjectList[i];
					break;
				case (VariableEnum.Int):
					hashTable[preFillKeyList[i]] = preFillIntList[i];	
					break;
				case (VariableEnum.Material):
					hashTable[preFillKeyList[i]] = preFillMaterialList[i];;
					break;
				case (VariableEnum.Quaternion):
					hashTable[preFillKeyList[i]] = preFillQuaternionList[i];
					break;
				case (VariableEnum.Rect):
					hashTable[preFillKeyList[i]] = preFillRectList[i];	
					break;
				case (VariableEnum.String):
					hashTable[preFillKeyList[i]] = preFillStringList[i];	
					break;
				case (VariableEnum.Texture):
					hashTable[preFillKeyList[i]] = preFillTextureList[i];;	
					break;
				case (VariableEnum.Vector2):
					hashTable[preFillKeyList[i]] = preFillVector2List[i];		
					break;
				case (VariableEnum.Vector3):
					hashTable[preFillKeyList[i]] = preFillVector3List[i];		
					break;
				case (VariableEnum.AudioClip):
					hashTable[preFillKeyList[i]] = preFillAudioClipList[i];		
					break;
				case (VariableEnum.Byte):
					hashTable[preFillKeyList[i]] = preFillByteList[i];		
					break;
				case (VariableEnum.Sprite):
					hashTable[preFillKeyList[i]] = preFillSpriteList[i];		
				break;
				default:
					break;
			}
		}
	}
	
}
