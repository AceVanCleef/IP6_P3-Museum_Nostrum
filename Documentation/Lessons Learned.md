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


## Unity Drag and Drop using EventSystem
Top tutorial on how to implement user input for mobile devices. It introduces the standard interfaces IDragHandler, IBeginDragHandler and IEndHandler and follows up by showing how to define custom event handlers.
- [Unity UI Drag and Drop Tutorial](https://www.youtube.com/watch?time_continue=989&v=c47QYgsJrWc)

## Define custom events for Unity's EventSystem
You can add custom events by declaring an interface in any .cs - file as following:
```
namespace UnityEngine.EventSystem {
	public interface IMyInterface : IEventSystemHandler {
		void MyCallback();
	}
}
```

Your class can then implement IMyInterface:
```
public class MyClass : MonoBehavior, IMyinterface{ ... }
```

- [Unity UI Drag and Drop Tutorial](https://youtu.be/c47QYgsJrWc?t=989)


## Outline Shader for Unity 5.6+
Shrimpey created this awesome outline highlightning shader.
![Shader demo.](https://github.com/Shrimpey/UltimateOutline/raw/master/images/overview.jpg?raw=true "example")
- [Github Repository](https://github.com/Shrimpey/UltimateOutline)
- Tutorial - How to setup this shader: [Silhouette Highlight / Outline / Glow Diffuse Shader in Unity 5](https://www.youtube.com/watch?v=00qMZlacZQo)
Please note: By importing an image, a material will be automatically generated. Change its Shader to Outlined >> UltimateOutline. After that, setup the shader to your liking.
Tipp: In a future iteration of this game, a script could handle this shader setup to minimize manuell labor.

## How to set the value of a variable in a custom shader?
```
//@shader
uniform float _FirstOutlineWidth;
```
This can be set by...
```
GetComponent<Renderer>().material.SetFloat("_FirstOutlineWidth", newWidth);
```
and read them by...
```
GetComponent<Renderer>().material.GetFloat("_FirstOutlineWidth")
```