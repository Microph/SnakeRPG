using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameBoard gameBoard_inScene;

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
    }

    void Update()
    {

    }

}
