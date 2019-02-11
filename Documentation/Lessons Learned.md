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

## AssetPostprocessor: Changing the shader of a picture after import automatically
Have a closer look at this: [AssetPostprocessor](https://docs.unity3d.com/ScriptReference/AssetPostprocessor.html)


## Predefined .gitattributes
You can google search a predefined gitattributes file that lists a large list of file formats.
-[FullStackForger/.gitattributes](https://gist.github.com/FullStackForger/fe2b3da81e60337757fe82d74ebf7d7a)


## Glass material
A shader and a fully transparent png approach described here:
- [Glass Material for Unity 5](https://gamedev.stackexchange.com/questions/98557/glass-material-for-unity-5)
- [Glass Shader (by Alastair Aitchison)](https://alastaira.wordpress.com/2013/12/21/glass-shader/)

## Gold material
Set smoothness and metallic to max. value and choose a yellow color that suits you.
Next, add a reflection probe to the 3D model in your scene and adjust its box size and its type (Baked or Realtime (adapts on environment when eg. player walks by)).
- [Creating a Gold Material / Shader in Unity 5
](https://www.youtube.com/watch?v=Jbd0cx5GM_Q)

## Merge conflict, remove local changes
```
<<<<<<< HEAD:file.txt
Hello world ( what you already had locally)
======= 
Goodbye (what was introduced by the other commit, in this case 77976da35a11)
>>>>>>> 77976da35a11db4580b80ae27e8d65caf5208086:file.txt
```
- [Git conflict markers](https://stackoverflow.com/questions/7901864/git-conflict-markers)


### How to Change Alpha of color
```
image = GetComponent<Image>();
var tempColor = image.color;
tempColor.a = 1f;
image.color = tempColor;
```
- [How to modify color.alpha in UI Image?
](https://answers.unity.com/questions/1121691/how-to-modify-images-coloralpha.html)