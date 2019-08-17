using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameBoard gameBoard;

    // Start is called before the first frame update
    void Start()
    {
        SetupNewGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupNewGame()
    {
        //spawn player
        gameBoard.SpawnPlayer();

        //spawn an enemy
        gameBoard.SpawnEnemy();

        //spawn an ally
        gameBoard.SpawnAlly();
    }
}
