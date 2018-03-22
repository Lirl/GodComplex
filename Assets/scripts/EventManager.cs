using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
Unity C# Event manager using UnityEvents and a Hashtable for loosely typed params with event.
This EventManager expands the usefulness of UnityEvents by allowing values of any type to be passed as a
parameter in the Event eg: int, string, Vector3 etc.

Usage:

// Handler
private void MyEventHandlerMethodName (Hashtable eventParams){
	if (eventParams.ContainsKey("MY_EVENT_KEY")){
		// DO SOMETHING
	}
}

// Add Listener for Event
EventManager.StartListening ("MY_EVENT", MyEventHandlerMethodName);

// Trigger Event:
EventManager.TriggerEvent ("MY_EVENT", new Hashtable(){{"MY_EVENT_KEY", "valueOfAnyType"}});

// Pass null instead of a Hashtable if no params
EventManager.TriggerEvent ("MY_EVENT", null);

*/

public class GCEvent : UnityEvent<Hashtable> { }

public class EventManager : MonoBehaviour {

    private Dictionary<string, GCEvent> eventDictionary;

    private static EventManager eventManager;

    //	SINGLETON
    public static EventManager instance {
        get {
            if (!eventManager) {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager) {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                } else {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init() {
        if (eventDictionary == null) {
            eventDictionary = new Dictionary<string, GCEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction<Hashtable> listener) {
        GCEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.AddListener(listener);
        } else {
            thisEvent = new GCEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<Hashtable> listener) {
        if (eventManager == null) return;
        GCEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, Hashtable eventParams = default(Hashtable)) {
        GCEvent thisEvent = null;

        Debug.Log(eventName);

        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.Invoke(eventParams);
        }
    }

    public static void TriggerEvent(string eventName) {
        TriggerEvent(eventName, null);
    }
}