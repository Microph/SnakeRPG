using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public FacingDirection facingDirection;
}

public enum FacingDirection
{
    Right,
    Down,
    Left,
    Up
}