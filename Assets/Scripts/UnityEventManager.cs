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
        if(DirectionInputEvent == null)
        {
            DirectionInputEvent = new DirectionInputEventClass();
        }
    }
    
    public DirectionInputEventClass DirectionInputEvent;
}

public class DirectionInputEventClass : UnityEvent<FacingDirection>
{
}