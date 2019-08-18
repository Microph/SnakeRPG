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
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);

            //Initialize events
            if(DirectionInputEvent == null)
            {
                DirectionInputEvent = new DirectionInputEventClass();
            }
        }
    }

    public DirectionInputEventClass DirectionInputEvent;
}

public class DirectionInputEventClass : UnityEvent<FacingDirection>
{
}