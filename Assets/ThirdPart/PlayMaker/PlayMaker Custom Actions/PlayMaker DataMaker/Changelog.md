#DataMaker Change log

###1.4.6
**Released** 12/11/2019. 

**Update**  

- Updated PlayMakerUtils  
- Updated Json dll

###1.4.5
**Released** 17/06/2019. 

**Update**  

- XmlSaveInProxy now cheks for input and prevent mix up


###1.4.4
**Released** 22/05/2019. 

**New**  

- added `Name()` xpath property access, which gives you the name of the xmlnode itself.


###1.4.3
**Released** 16/05/2019. 

**Update**  

- Updated PlayMakerUtils  
- Updated Json dll


###1.4.2
**Released** 11/04/2019. 

**New**

- action `ConvertCsvStringToXmlNode` now accepts a custom delimiter.

###1.4.1
**Released** 04/04/2019. 

**New**

- action `ReadCsv` now accepts a custom delimiter.

###1.4.0
**Released** 13/11/2018. 

**New**
- new json newton dll

- new action `ConvertXmlNodeToJson`

###1.3.1
**Released** 18/08/2018

**Fix** 

- Updated PlayMaker utils


###1.3
**Released** 16/08/2018

**New**

- Namespace handling, feature default namespace as well. Default namespace nodes will need to be prefixed with `default:`


**Fix** 
 
- asbtracted `DataMakerProxyBase` monobehaviour so that it doesn't get listed and misused

**New**

- DataMakerXmlProxy now lets you save xmlnode reference from the component directly.

###1.2.9
**Released** 27/06/2018

**New**  

- Editor Browser for Memory references. 

**New**  

- Action "XmlReplaceNode"

**Fix** 
 
- Fixed `XmlGetNextNodeListPropertiesEdit` editfields definitions. 

**Update**  

- Update PlayMakerUtils  
- updated ArrayMaker  

###1.2.8
**Released** 29/01/2018

**Fix**  

- Fixed editors preview when content is null
- Fixed action editors for getting node properties between legacy and new way.

**New**  

- Action "XmlNodeRemoveAttribute"
- Action "XmlCloneNode"
- Action "XmlParentNode"
- DATAMAKER Scripting define symbol

###1.2.7

**Released** 11/09/2017

**Fix**  

- Fixed Action "XmlGetNextNodeListProperties"


###1.2.6

**Released** 11/09/2017

**New**  

- Action "XmlNodeDelete"

###1.2.5

**Released** 11/05/2017

**Fix**  

- fixed xpathquery variable assignment interface for all actions using xpath query

###1.2.4

**Released** 26/04/2017

**Fix**  

- fixed `XmlSelectSingleNode`, `XmlSetNodeProperties` and `XmlGetNextNodeListProperties` preview in action browser


###1.2.3

**Fix**  

- fixed `XmlSelectSingleNode`, `XmlSetNodeProperties` and `XmlGetNextNodeListProperties`, it will swap to a working list where fsm variables can be picked for property names

**Fix**

- Fixed `XmlSelectSingleNode` missing "found" property

###1.2.2

**Fix**  

- Fixed `XmlCreateNode`  

###1.2.1

**Fix**  

- Fixed xml string preview to prevent too long text that causes ui glitches and Unity editor complaints  

 
###1.2
**New**  

- Csv Parser for working with Csv string with and without headers, with xml converter.

**Fix**  

- Fixed `ConvertJsonStringToXmlNode` saving xml to string  
- Fixed fsmVariable selection for references

###1.1
**Update**  

- Update PlayMakerUtils  
- updated ArrayMaker  

**Fix**  

- Fixed PlayMaker 1.8+ support
  
### 1.0.5  
**Fix**  

- Fixed proxy component refreshing system  
- Fixed `XmlInsertNode` insertAfter case  
 
### 1.0.4  
**Fix**  

- Fixed multiple Proxies per GameObject detection routine within custom Actions  

**Improvement**

- improve safety for `HashTableGetxmlNodeProperties` when property.innerText is null or empty


### 1.0.3  
**Fix**  

- Remove deprecated api call `VariableEditor.FsmVarPopup()` and replaced with custom popup from PlayMakerUtils library ( created for the purpose)  
- Improved safety for `HashTableGetXmlNodeProperties` when `property.innerText` is null or empty.


### 1.0.2  
**New**  

- Merged ArrayMaker Json Plugin into DataMaker (to avoid having to maintain this inside ArrayMaker)  
- Two Json library ( [newtonsoft for .net2.0](https://json.codeplex.com/) and a [custom version](http://techblog.procurios.nl/k/n618/news/view/14605/14863/how-do-i-write-my-own-parser-(for-json).html)), one for converting from json to xml and one for quick access. I went for this because both library have different usage and each have pros and cons

**Improvement**  

- ReOrganized Folders Structure ( to be compliant for submodules)  
- Distribution is now via Ecosystem

### 1.0.1
**New**  

- New actions _XmlProxyGetXmlAsString_ and _XmlProxyRefreshStringContent_ to better handle saving xml content as string  
- new _XmlNodeRemoveAll_ action
- new demo showing how to load and save xml in playerprefs

**Fix**  

- Fixed _XmlGetNextnodeListProperties_ cache  



**Improvement**  

- Added Reset option for _XmlGetNextnodeListProperties_  

**!!Deprecation!!**

- Removed StoreInMemory string property for _DataMakerXmlProxyEditor_  ( it's better to manually select the node you want from the proxy)

### 1.0.0
**New**  

- MIT License Addition  
- Support for Ecosystem changeLog and versioning   
- Added Refresh and Save In Project for _DataMakerXmlProxyEditor_ pointing to TextAssets  

**Fix**

- Fixed contentPreview missing skin on some Unity  versions, now using Label explicitly.  
- Fixed caching of xml string content in _DataMakerXmlProxyEditor_ to reduce scene size with uncessary editor only data  
- Adds ArrayMaker to the packages ( it has xml addons custom actions)


**Improvement**   

- Undo support when adding xml proxy composent to GameObject  
- Moved to [public Github Repository](https://github.com/jeanfabre/PlayMaker--DataMaker) for visibility and better code management safety.  
- Menus now under PlayMaker/Addons for better/compliant organisation  


###0.9.0
**Note**   

- Initial release
