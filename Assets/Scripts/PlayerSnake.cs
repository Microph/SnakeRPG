using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSnake : MonoBehaviour
{
    public bool isHead = false;
    public PlayerSnake nextLinkedPart = null;

    //Called by ResourceManager
    public void Setup(bool isHead, PlayerSnake nextLinkedPart)
    {
        this.isHead = isHead;
        this.nextLinkedPart = nextLinkedPart;
    }
}
