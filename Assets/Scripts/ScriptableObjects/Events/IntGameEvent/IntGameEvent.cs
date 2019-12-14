// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Events/Int Game Event")]
public class IntGameEvent : ScriptableObject
{
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<IntGameEventListener> eventListeners =
        new List<IntGameEventListener>();

    public void Raise(int data)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--) {
            eventListeners[i].OnEventRaised(data);
        }
    }

    public void RegisterListener(IntGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(IntGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}