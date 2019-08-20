using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSnakeComponent : MonoBehaviour
{
    private bool _isHead = false;

    public Tuple<int, int> currentIndex;
    public PlayerSnakeComponent nextLinkedSnakePart;
    public FacingDirection facingDirectionFromInput;

    public bool IsHead
    {
        get => _isHead;
        set
        {
            _isHead = value;
            if (IsHead)
            {
                UnityEventManager.Instance.DirectionInputEvent.AddListener(RotateSnakeHead);
                GetComponent<Character>().IsSnakeHead.enabled = true;
            }
            else
            {
                UnityEventManager.Instance.DirectionInputEvent.RemoveListener(RotateSnakeHead);
                GetComponent<Character>().IsSnakeHead.enabled = false;
            }
        }
    }

    //Called by ResourceManager
    public void Setup(bool isHead, Tuple<int, int> currentIndex, PlayerSnakeComponent nextLinkedSnakePart = null)
    {
        this.IsHead = isHead;
        this.currentIndex = currentIndex;
        this.nextLinkedSnakePart = nextLinkedSnakePart;
    }

    private void OnDestroy()
    {
        UnityEventManager.Instance.DirectionInputEvent.RemoveListener(RotateSnakeHead);
    }

    //Listen to directional input event
    private void RotateSnakeHead(FacingDirection toBeFacingDirection)
    {
        Character character = GetComponent<Character>();

        //Prevent 180 turn to hit its second part
        if (nextLinkedSnakePart != null)
        {
            Tuple<int, int> toBeIndex = GameBoard.Instance.CalculateToBeIndex(character, toBeFacingDirection);
            if((toBeIndex.Equals(nextLinkedSnakePart.currentIndex)))
            {
                return;
            }
        }

        RotateCharacter(character, toBeFacingDirection);
        facingDirectionFromInput = toBeFacingDirection;
    }

    public static void RotateCharacter(Character characterScript, FacingDirection toBeFacingDirection)
    {
        switch (toBeFacingDirection)
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
