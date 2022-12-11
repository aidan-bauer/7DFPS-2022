
using System;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{

    [Tooltip("Event to register with")]
    public GameEvent Event;

    [Tooltip("Response to Invoke when Event is Raised")]
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    internal void OnEventRaised()
    {
        Response.Invoke();
    }
}
