using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private float timeIncrementer;
    private float moveInterval;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        SetupNewGame(GameBoard.Instance);
    }

    private void SetupNewGame(GameBoard gameBoard)
    {
        gameBoard.SetupNewTiles(10, 19);
        gameBoard.SpawnPlayer_StartGame();
        gameBoard.SpawnEnemy_StartGame();
        gameBoard.SpawnAHero();
        gameBoard.SpawnAHero();
        gameBoard.SpawnAHero();
        gameBoard.SpawnAHero();

        //Initilize timers
        timeIncrementer = 0;
        moveInterval = 0.33f;
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
            GameBoard.Instance.UpdateBoardState();
        }
    }

}
