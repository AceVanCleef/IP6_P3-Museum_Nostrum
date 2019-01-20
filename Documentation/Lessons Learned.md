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

## GetComponent<OnInterface>() works
```
IOnTap tapHandler = hitGameObject.GetComponent<IOnTap>();
if (tapHandler != null)
{
    tapHandler.OnTap();
}
```

## C# Sealing an interface after implementing it
In C#, a method implemented from Interface is automatically sealed by default unless you use the keyword virtual on it.
```
class A : ITest
{
    public void SomeMethod()  { ... } //sealed by default.
}
```
- [Sealing an interface after implementing it](https://stackoverflow.com/questions/5386420/sealing-an-interface-after-implementing-it)

## Difference between Interface and Abstract class
An interface only allows implementation of public methods, thus guaranteeing calling classes certain methods to be found. An abstract class in the other hand allows the definition of public, protected and private classes which will be inherited by subclasses. Therefore, an abstract class provides a contract towards its subclasses. An Interface a contracts towards other classes calling the implementating classes.
- [Why can't I have protected interface members?](https://stackoverflow.com/questions/516148/why-cant-i-have-protected-interface-members)