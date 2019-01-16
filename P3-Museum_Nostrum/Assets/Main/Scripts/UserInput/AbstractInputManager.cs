using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractInputManager : MonoBehaviour {
    // Inspiration: https://gamedev.stackexchange.com/questions/65957/how-to-manage-input-state-in-unity3d

    private List<IUserInputListener> allObservers = new List<IUserInputListener>();

	public virtual void AddUserInputListener(IUserInputListener observer, int priority)
    {
        if (!allObservers.Contains(observer)) allObservers.Add(observer);
    }

    public virtual void RemoveUserInputListener(IUserInputListener observer)
    {
        if (allObservers.Contains(observer)) allObservers.Remove(observer);
    }

    public virtual void SendInputMessage(InputMessage im)
    {
        for (int i = 0; i < allObservers.Count; ++i)
        {
            InputMessage current = allObservers[i].TakeInput(im);
            if (current.handled) break;
        }
    }
}
