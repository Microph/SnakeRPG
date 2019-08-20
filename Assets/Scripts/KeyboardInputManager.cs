using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputManager : MonoBehaviour
{
    void Update()
    {
        //Switch Snake
        if(     (Input.GetButtonDown("Right Direction") && Input.GetButtonDown("Left Direction")) 
            ||  (Input.GetButtonDown("Up Direction") && Input.GetButtonDown("Down Direction"))
            &&  UnityEventManager.Instance.SwitchSnakeInputEvent != null)
        {
            UnityEventManager.Instance.SwitchSnakeInputEvent.Invoke();
        }
        else if (Input.GetButtonDown("Right Direction") && UnityEventManager.Instance.DirectionInputEvent != null)
        {
            UnityEventManager.Instance.DirectionInputEvent.Invoke(FacingDirection.Right);
        }
        else if (Input.GetButtonDown("Down Direction") && UnityEventManager.Instance.DirectionInputEvent != null)
        {
            UnityEventManager.Instance.DirectionInputEvent.Invoke(FacingDirection.Down);
        }
        else if (Input.GetButtonDown("Left Direction") && UnityEventManager.Instance.DirectionInputEvent != null)
        {
            UnityEventManager.Instance.DirectionInputEvent.Invoke(FacingDirection.Left);
        }
        else if (Input.GetButtonDown("Up Direction") && UnityEventManager.Instance.DirectionInputEvent != null)
        {
            UnityEventManager.Instance.DirectionInputEvent.Invoke(FacingDirection.Up);
        }
    }
}
