using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// manages events and their listeners using Singleton design pattern
/// adds and removes listeners for specific eventIDs (dictionary that maps event IDs (strings) to list of EventListeners)
/// triggers listeners for eventIDs by notifying the registered listeners (via EventGo)
/// </summary>

public class EventManager
{
    private static EventManager instance;

    public static EventManager Instance
    {
        get
        {
            if (instance == null) instance = new EventManager();
            return instance;
        }
    }

    public delegate void EventListener(string eventName, object param = null);
    private Dictionary<string, List<EventListener>> eventListener;

    EventManager()
    {
        eventListener = new Dictionary<string, List<EventListener>>();
    }

    public void AddEventListener(string eventID, EventListener listener)
    {
        if (!eventListener.ContainsKey(eventID)) eventListener.Add(eventID, new List<EventListener>());
        eventListener[eventID].Add(listener);
    }

    public void RemoveEventListener(string eventID, EventListener listener)
    {
        if (eventListener.ContainsKey(eventID))
            eventListener[eventID].Remove(listener);
    }

    public void EventGo(string eventID, string eventName, object param = null)
    {
        if (eventListener.ContainsKey(eventID))
            for (int i = eventListener[eventID].Count - 1; i >= 0; i--)
                eventListener[eventID][i](eventName, param);
    }
}
