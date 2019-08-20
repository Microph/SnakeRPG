using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputManager : MonoBehaviour
{
    public void InvokeUpInput()
    {
        UnityEventManager.Instance.DirectionInputEvent.Invoke(FacingDirection.Up);
    }

    public void InvokeRightInput()
    {
        UnityEventManager.Instance.DirectionInputEvent.Invoke(FacingDirection.Right);
    }

    public void InvokeDownInput()
    {
        UnityEventManager.Instance.DirectionInputEvent.Invoke(FacingDirection.Down);
    }

    public void InvokeLeftInput()
    {
        UnityEventManager.Instance.DirectionInputEvent.Invoke(FacingDirection.Left);
    }
}
