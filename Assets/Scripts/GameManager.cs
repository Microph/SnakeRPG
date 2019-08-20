using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private bool _isGameOver = false;
    private float _timeIncrementer;
    private float _moveInterval;

    public bool IsGameOver
    {
        get => _isGameOver;
        set
        {
            _isGameOver = value;
            if (_isGameOver)
            {
                GameOverEvent();
            }
        } 
    }

    private void GameOverEvent()
    {
        throw new NotImplementedException();
    }

    private void Awake()
    {
        _instance = this;
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
        _timeIncrementer = 0;
        _moveInterval = 0.33f;
    }

    void Update()
    {
        if (IsGameOver)
        {
            return;
        }

        //Update tick
        _timeIncrementer += Time.deltaTime;

        if(_timeIncrementer < _moveInterval)
        {
            return;
        }
        else
        {
            _timeIncrementer = 0;
            GameBoard.Instance.UpdateBoardState();
        }
    }

}
