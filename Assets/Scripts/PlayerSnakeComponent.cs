using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSnakeComponent : MonoBehaviour
{
    public bool isHead = false;
    public Tuple<int, int> currentIndex;
    public Tuple<int, int> nextLinkedIndex;
    public FacingDirection facingDirectionFromInput;

    private Character characterScript;

    //Called by ResourceManager
    public void Setup(bool isHead, Tuple<int, int> currentIndex, Tuple<int, int> nextLinkedIndex = null)
    {
        this.isHead = isHead;
        this.currentIndex = currentIndex;
        this.nextLinkedIndex = nextLinkedIndex ?? new Tuple<int, int>(-1, -1);
    }

    private void Start()
    {
        characterScript = GetComponent<Character>();
        if (isHead)
        {
            UnityEventManager.Instance.DirectionInputEvent.AddListener(RotateSnakeHead);
        }
    }

    private void OnDisable()
    {
        UnityEventManager.Instance.DirectionInputEvent.RemoveListener(RotateSnakeHead);
    }

    private void RotateSnakeHead(FacingDirection facingDirection)
    {
        RotateSnakePart(characterScript, facingDirection);
        facingDirectionFromInput = facingDirection;

    }

    public static void RotateSnakePart(Character characterScript, FacingDirection facingDirection)
    {
        switch (facingDirection)
        {
            case FacingDirection.Right:
                characterScript.spriteRenderer.flipX = false;
                characterScript.spriteRenderer.flipY = false;
                characterScript.spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case FacingDirection.Down:
                if (characterScript.spriteRenderer.flipX == true)
                {
                    characterScript.spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                else
                {
                    characterScript.spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, -90);
                }
                break;
            case FacingDirection.Left:
                characterScript.spriteRenderer.flipX = true;
                characterScript.spriteRenderer.flipY = false;
                characterScript.spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case FacingDirection.Up:
                if (characterScript.spriteRenderer.flipX == true)
                {
                    characterScript.spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, -90);
                }
                else
                {
                    characterScript.spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                break;
        }
    }
}
