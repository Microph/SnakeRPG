using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameBoard gameBoard_inScene;

    private float timeIncrementer;
    private float moveInterval;

    void Start()
    {
        SetupNewGame(gameBoard_inScene);
    }

    private void SetupNewGame(GameBoard gameBoard)
    {
        gameBoard.SetupNewTiles(10, 19);
        gameBoard.SpawnPlayer_StartGame();
        gameBoard.SpawnEnemy_StartGame();
        gameBoard.SpawnAHero();

        //Initilize timers
        timeIncrementer = 0;
        moveInterval = 1;
    }

    void Update()
    {
        //Update tick
        timeIncrementer += Time.deltaTime;

        if(timeIncrementer < moveInterval)
        {
            return;
        }
        else
        {
            timeIncrementer = 0;
            gameBoard_inScene.UpdatePlayerSnake();
        }
    }

}
