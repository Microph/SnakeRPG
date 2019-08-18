using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButton("Right Direction") && UnityEventManager.Instance.DirectionInputEvent != null)
        {
            UnityEventManager.Instance.DirectionInputEvent.Invoke(FacingDirection.Right);
        }
        else if (Input.GetButton("Down Direction") && UnityEventManager.Instance.DirectionInputEvent != null)
        {
            UnityEventManager.Instance.DirectionInputEvent.Invoke(FacingDirection.Down);
        }
        else if (Input.GetButton("Left Direction") && UnityEventManager.Instance.DirectionInputEvent != null)
        {
            UnityEventManager.Instance.DirectionInputEvent.Invoke(FacingDirection.Left);
        }
        else if (Input.GetButton("Up Direction") && UnityEventManager.Instance.DirectionInputEvent != null)
        {
            UnityEventManager.Instance.DirectionInputEvent.Invoke(FacingDirection.Up);
        }
    }
}
