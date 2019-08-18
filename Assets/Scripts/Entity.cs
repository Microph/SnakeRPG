using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public FacingDirection facingDirection;

    protected void Setup(EntityScriptableObject entityScriptableObject, FacingDirection facingDirection = FacingDirection.Right)
    {
        spriteRenderer.sprite = entityScriptableObject.sprite;
        this.facingDirection = facingDirection;
    }
}

public enum FacingDirection
{
    Right,
    Down,
    Left,
    Up
}