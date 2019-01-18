# Lessons learned

## How to write Editor Scripts
- [ExecuteInEditMode will run your script in edit mode](https://blog.theknightsofunity.com/executeineditmode-will-run-script-edit-mode/)

## Tag Comparison: Strings vs. Enum
It is faster to compare enum values than strings. Unity offers a string based tags to identify objects. But enums can be compared with a noticable performance difference. One open question is left to ask: How performant is GetComponent<MyCustomEnumTag> to access a GameObjects custom tag(s)?
- [Squeezing out Performance: Comparing objects](https://forum.unity.com/threads/squeezing-out-performance-comparing-objects.178593/)


## Observer Pattern in Unity
Demonstrated at an InputManager
- [How to manage input state in Unity3D?](https://gamedev.stackexchange.com/questions/65957/how-to-manage-input-state-in-unity3d)


## Locking orientations for mobile devies
Edit -> Project Settings -> Player. Then under "Resolution and Presentation" group you can see the "Orientation" sub group. Here you can toggle off the undesired orientations.
- [Android: How do I force only portrait orientation?](https://answers.unity.com/questions/615249/android-how-do-i-force-only-portrait-orientation.html)


## How to detect Gestures on GUI elements
- IDragHandler, IEndDragHandler, IDropHandler for drag and drop.  
- IPointerClickHandler for tapping on GUi element.
- [How to detect click/touch events on UI and GameObjects](https://stackoverflow.com/questions/41391708/how-to-detect-click-touch-events-on-ui-and-gameobjects)