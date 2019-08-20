using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float STARTING_MOVE_INTERVAL = 0.70f;
    public float MOVE_SPEED_INCREASE_RATE = 0.07f;
    public float LOWEST_MOVE_INTERVAL = 0.1f;
    public float HERO_SPAWN_INTERVAL = 10f;
    public float ENEMY_SPAWN_INTERVAL = 10f;
    public float ITEM_SPAWN_INTERVAL = 20f;

    public Text scoreText;
    public GameObject gameOverPanel;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private int _score = 0;
    private bool _isGameOver = true;
    private bool _isInBattle = false;
    private bool _doesSwitchHead = false;
    private float _moveTimeCounter;
    private float _heroSpawnTimeCounter;
    private float _enemySpawnTimeCounter;
    private float _itemSpawnTimeCounter;
    private float _currentmoveInterval;
    private float _savedCurrentMoveInterval;

    public bool IsInBattle
    {
        get => _isInBattle;
        set
        {
            //Not reset timer if already in battle last move
            if (value == true && IsInBattle)
            {
                return;
            }

            _isInBattle = value;
            if (_isInBattle)
            {
                _moveTimeCounter = 0;
                _savedCurrentMoveInterval = _currentmoveInterval;
                _currentmoveInterval = 0.5f;
            }
            else
            {
                _currentmoveInterval = _savedCurrentMoveInterval;
            }
        }
    }

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            scoreText.text = _score.ToString();
        }
    }

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

    public void StartGame()
    {
        _isGameOver = false;
    }

    public bool DoesSwitchHead
    {
        get => _doesSwitchHead;
        set => _doesSwitchHead = value;
    }

    public void IncreaseScore(int v)
    {
        Score += v;
    }

    public void IncreaseSnakeSpeed()
    {
        _savedCurrentMoveInterval -= MOVE_SPEED_INCREASE_RATE;
        if(_savedCurrentMoveInterval < LOWEST_MOVE_INTERVAL)
        {
            _savedCurrentMoveInterval = LOWEST_MOVE_INTERVAL;
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void GameOverEvent()
    {
        gameOverPanel.SetActive(true);
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

        //Initilize timers
        _moveTimeCounter = 0;
        _heroSpawnTimeCounter = 0;
        _enemySpawnTimeCounter = 0;
        _itemSpawnTimeCounter = 0;
        _currentmoveInterval = STARTING_MOVE_INTERVAL;
        _savedCurrentMoveInterval = _currentmoveInterval;
    }

    void Update()
    {
        if (_isGameOver)
        {
            return;
        }

        _moveTimeCounter += Time.deltaTime;
        if(_moveTimeCounter >= _currentmoveInterval)
        { 
            _moveTimeCounter = 0;
            if (_doesSwitchHead)
            {
                _doesSwitchHead = false;
            }
            else
            {
                GameBoard.Instance.UpdateBoardState();
            }
        }

        _heroSpawnTimeCounter += Time.deltaTime;
        if (_heroSpawnTimeCounter >= HERO_SPAWN_INTERVAL)
        {
            _heroSpawnTimeCounter = 0;
            GameBoard.Instance.SpawnAHero();
        }

        _enemySpawnTimeCounter += Time.deltaTime;
        if (_enemySpawnTimeCounter >= ENEMY_SPAWN_INTERVAL)
        {
            _enemySpawnTimeCounter = 0;
            GameBoard.Instance.SpawnAnEnemy();
        }

        _itemSpawnTimeCounter += Time.deltaTime;
        if (_itemSpawnTimeCounter >= ITEM_SPAWN_INTERVAL)
        {
            _itemSpawnTimeCounter = 0;
            GameBoard.Instance.SpawnAnItem();
        }
    }
}
