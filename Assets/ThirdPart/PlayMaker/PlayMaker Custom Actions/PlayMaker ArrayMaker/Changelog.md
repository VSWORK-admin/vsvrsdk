#ArrayMaker Change log

###1.3.7
**Release Date:** 28/04/2020  

**Update**: added better logs on errors 

###1.3.6
**Release Date:** 14/03/2020  

**New**: New action `ArrayListRaycastAll`  
**New**: New action `ArrayListSendStringEventToGameObjects`  

###1.3.5
**Release Date:** 03/02/2020 

**Fix:** `ArrayTableSetItem` and `ArrayTableGetItem` custom inspector fixed.

###1.3.4
**Release Date:** 20/12/2019  

**Update:** `ArrayListGetScreenResolution` now features an option  to ignore refresh rate value

###1.3.3
**Release Date:** 18/10/2019  

**New:** new action `ArrayListSendStringEventToGameObjects`

**Update:** `ArrayListIsEmpty` now features an every frame option  

###1.3.2
**Release Date:** 08/10/2019  

**Fix:**  `ArrayListGetScreenResolutions` now remove duplicates

**Update:** `PlayMaker Utils` update  

###1.3.1
**Release Date:** 05/06/2019  

**New:** new action `ArrayListGetRandomCurvedWeighted`

**Update:** `PlayMaker Utils` update  

###1.3
**Release Date:** 29/03/2019  

**New:** new action `ArrayListSetEventCallbacks`

###1.2.9
**Release Date:** 28/01/2019  

**Fix:** fixed editor time scene dirty flag to prevent benine error.

**New:** new action `ArrayListGetChildrenOfGameObject`

###1.2.8
**Release Date:** 16/11/2018  

**Update:** CollectionsActions now expose `GetHashTableProxyPointer` and `GetArrayListProxyPointer` for actions using different base but still needs to access arrayMaker proxies ( EasySave 3 Actions for example)

###1.2.7
**Release Date:** 08/11/2018  

**Fix:** fixed various harmless but annoying warnings for arrayTable actions

###1.2.6
**Release Date:** 18/09/2018  

**Update:** `PlayMaker Utils` update  

**New:** new actions `ArrayListGetAllChildOfGameObject`, `ArrayListMove`.  
**New:** new ArrayTable actions `ArrayTableAddRow`, `ArrayTableContains` ,`ArrayTableGetRowToArrayList`, `ArrayTableGetRowToHashtable`, `ArrayTableRemoveRowAt`



###1.2.5
**Release Date:** 05/09/2018  

**Update:** `PlayMaker Utils` update  

**New:** new action `ArrayListGetAllChildOfGameObject`  
**New:** new action `ArrayTableSetItem`, `ArrayTableGetItem`, `ArrayTableGetRowToArrayList`, `ArrayTableGetRowToHashtable`


###1.2.4
**Release Date:** 26/06/2018  

**Update:** `PlayMaker Utils` update  

###1.2.3
**Release Date:** 21/05/2018  

**Fix:** fixed GameObject EventData with enableEvents 

###1.2.2
**Release Date:** 03/04/2018  

**Update:** `PlayMaker Utils` update  

###1.2.1
**Release Date:** 25/01/2018  

**New** 
- Scripting define symbol `ARRAYMAKER` in place

###1.2.0
**Release Date:** 15/01/2018  

**Fix** 
- Fixed scene not marked as dirty when editing proxy

###1.1.9
**Release Date:** 23/10/2017  

**Fix** 
- Fixed `HashTableGetType` warning
- Fixed unused variable in `ArrayListGetType` and `ArrayListGetAllChildOfGameObject`


###1.1.8
**Release Date:** 17/10/2017  

**Documentation:** `HashtableGetRandom` tooltip fixed 


###1.1.7
**Release Date:** 28/09/2017  

**New:** new action `ArrayListGetAllChildOfGameObject`   

**Update:** `ArrayListContains` now lets you store the index 

**Update:** `PlayMaker Utils` update  

###1.1.6
**Release Date:** 22/06/2017  

**New:** arrayList and hashtable data can now be copied from runtime back to edit time 

**New:** `arrayListGetType` and `HashtableGetType` to know the type of an element  

###1.1.5 Build 2
**Update:** `PlayMaker Utils` update for Compatibility with Unity 4.7  

###1.1.5 Build 1
**Update:** `PlayMaker Utils` update for Compatibility with 1.8  

###1.1.5
**New**  
- `ArrayListSortGameObjectByDistance`
- Prefill count is now subject to save and cancelation to prevent deletion of all data when editing the field  

**Fix**  
- Fixed `ArrayListCount` to force useVariable for count property  
- Fixed `ArrayListGetPrevious` to not skip the last item (index 0)

###1.1.4
**Fix**  
- Fixed `ArrayListAddRange` to save value properly  

###1.1.3
**New**  
- Support for Sprite assets  

###1.1.2
**Fix**  
- Removed uncessary debug logs in actions  

###1.1.1
**Improvement**  
- Included Unity 5 refactored changes, so now the package opens as is in Unity 5 without any additional steps  

### 1.1.0  
**New**  
- Refactored Github Repositories, using now submodule  
- Moved thirdParty code and samples in respective folders ( for clarity and to be compatible with submodule)  

### 1.0.4  
**New**  
- Support for "byte" value  

### 1.0.3
**New**  
- New ArrayListActivateGameObjects

**Improvement**  
- Get the variable's index in ArrayListAdd ( thanks to [dudebxl](http://hutonggames.com/playmakerforum/index.php?topic=10012.msg32820#new) from PlayMaker forum)

### 1.0.2
**Fix**  
- Fixed ArrayListSendEventToGameObjects Finish() call

### 1.0.1
**Fix**  
- Fixed ArrayListSendEventToGameObjects only sending to owner instead of GamObjects in the list
- Safer actions when fsmvar not defined



### 1.0.0
**Improvement**   
- Better git hosting with PlayMaker Utils as SubModule  
- Comply with Ecosystem ChangeLog and versionning system  


###0.9.9

**New**  
- MIT License Addition

**Fix**   
- Windows Phone compatibility work in progress, with extensions for ArrayList LastIndexOf and manual snapshots for lists copies and removal of commented lines giving a hard time to the compiler  
- easySave addon HashTableEasyLoad action  
- texture usage in EasySave sample to use it's own texture set to read write properly.



###0.9.8
**Issue**  
- removed undo when adding proxies to selection, just not finding the right way to do a consistent behavior... and it created obsolete warnings on 4.x

**New**   
- added reset to iterative actions to make sure we have a way to force iterating back from the the first item if we exited the loop early.  
- added currentIndex in iterative actions to know where we are, not just to get the value itself.  
- added ArrayListIsEmpty and HashTableIsEmpty action  
- added ArrayListGetFarthestGameObjectInSight  
- added HashtableEditKey action  
- remove obsolete warnings on U4  
- added arrayList min max and average values custom actions to start some statistic actions around content.  

###0.9.7
**New**  
- added snapshots: prefilled data is stored and use RevertToSnapshot to get back to it, and TakeSnapShot to record the current state of things.  

###0.9.6
**New**  
- fixed prefill setup when downsizing the number of prefilled items.  

###0.9.5
**New**  
- added ArrayListSwapIndex 

###0.9.4
**New**  
- added ArrayListGetGameObjectMaxFsmFloatIndex, thanks for FlyingRobot: http://hutonggames.com/playmakerforum/index.php?topic=5116  
- added ArrayListGetClosestGameObjectInSight, thanks to FlyingRobot: http://hutonggames.com/playmakerforum/index.php?topic=5056  
- added ArrayListInsert, thanks to FlyingRobot: http://hutonggames.com/playmakerforum/index.php?topic=5146.0  
- added backEvent properties actions.

###0.9.3
**Fix**  
- fixed vector2 support ( display of vector2 data in inspector)

**New**
- added AudioClip support  
- added HashTableAddMany, HashTableSetMany, HashTableGetMany  

###0.9.2
**New**
- Added addons for EasySave serialization   
- Added proper support for Vector2 in arrayList and HashTable inspector  

**Improvement** 
- Using now utomate to automate packaging  
- Packages now separated, just the framework, with samples and addons  

**Fix**  
- fixed null values display in inspector for arrayList nad HashTable.

###0.9.1
**Note:** **_Breaking Build_**. NOT COMPATIBLE WITH PREVIOUS VERSIONS  
Many actions public interface changed, so you will need to re-assign setters and getters on all of them.   
This version is a huge improvment in terms of ease of use, with now much more comprehensive and logical actions interfaces, no more lenghtly actions for nothing, 
 the latest playmaker version features a new FsmVar class that let the user select first the variable type, and then select it or feed some values, I don't have to expose ALL possibilities anymore which was confusing, prone to error and frustrations and totally messy.
 
###0.8.0
**Note:** **_Breaking Build_**. previous version had a different file organisation, so now it sits in its own folder and not within "PlayMaker" which was a mistake, 
 I assumed actions would only be detected if within the Actions folder...
 
**Note**  
- goes to 0.9 because I can :) well I had a 0.8 version sort of, but so much went that I moved to 0.9

**New**  
- New actions  

**Fix**  
- incorporate fixes for Texture and proceduralMaterial assignment  
- fixed prefab instance editing not serializing properly  
- fixed ArrayListCopyTo action  

###0.7.0

**Improvement**  
- now using gamObject for reference instead of the component itself via fsmObject, since this doesn't bring any advantages ( as we still need a string reference...)

###0.6.0  
**Note:**  
- Initial Public Release

