using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSnakeComponent : MonoBehaviour
{
    public bool isHead = false;
    public int currentRow, currentCol;
    public int nextLinkedPartRow, nextLinkedPartCol;

    //Called by ResourceManager
    public void Setup(bool isHead, int currentRow, int currentCol, int nextLinkedPartRow = -1, int nextLinkedPartCol = -1)
    {
        this.isHead = isHead;
        this.currentRow = currentRow;
        this.currentCol = currentCol;
        this.nextLinkedPartRow = nextLinkedPartRow;
        this.nextLinkedPartCol = nextLinkedPartCol;
    }

    private void Update()
    {
        if (isHead)
        {
            //Actively update sprite rotation to input
        }
    }
}
