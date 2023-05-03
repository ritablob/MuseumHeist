using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
/*
List of Event Listeners
--> CONTROLLER
- IncomingDataArduino (sent by controller script, event listeners are all individual icontrollers)
- OutgoingDataArduino (sent by all individual icontrollers, read by controller script)
- OpenConnection
- CloseConnection

*/