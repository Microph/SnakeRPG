using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventManager : MonoBehaviour
{
    private static UnityEventManager _instance;
    public static UnityEventManager Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;

        //Initialize events
        SwitchSnakeInputEvent = new UnityEvent();
        DirectionInputEvent = new DirectionInputEventClass();
    }

    public UnityEvent SwitchSnakeInputEvent;
    public DirectionInputEventClass DirectionInputEvent;
}

public class DirectionInputEventClass : UnityEvent<FacingDirection>
{
}