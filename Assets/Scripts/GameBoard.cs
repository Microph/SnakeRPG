using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameBoard : MonoBehaviour
{
    private static GameBoard _instance;
    public static GameBoard Instance { get { return _instance; } }

    public Tile[,] tiles;
    public Transform zeroZeroTilePos, zeroOneTilePos;

    private Character _playerCharacterHead;
    private PlayerSnakeComponent _playerSnakeHead;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        UnityEventManager.Instance.SwitchSnakeInputEvent.AddListener(SwitchSnake);
    }

    private void OnDestroy()
    {
        UnityEventManager.Instance.SwitchSnakeInputEvent.RemoveListener(SwitchSnake);
    }

    private void SwitchSnake()
    {
        if (_playerSnakeHead.nextLinkedSnakePart == null)
        {
            return;
        }

        GameManager.Instance.DoesSwitchHead = true;
        //Stash prev head data
        Character stashedPrevHeadCharacter = _playerCharacterHead;
        Tuple<int, int> stashedPrevHeadIndex = _playerSnakeHead.currentIndex;

        //Pass facingDirectionFromInput to next part
        _playerSnakeHead.nextLinkedSnakePart.facingDirectionFromInput = _playerCharacterHead.lastMoveFacingDirection;

        //Delete current head data in tile and remove snake component 
        tiles[_playerSnakeHead.currentIndex.Item1, _playerSnakeHead.currentIndex.Item2].occupiedEntity = null;
        _playerSnakeHead.IsHead = false;
        Destroy(_playerSnakeHead);

        //Assign next part to be head
        _playerSnakeHead = _playerSnakeHead.nextLinkedSnakePart;
        _playerCharacterHead = _playerSnakeHead.GetComponent<Character>();
        _playerSnakeHead.IsHead = true;

        MoveAndMakeMeetingCharacterJoinTail(stashedPrevHeadCharacter, stashedPrevHeadCharacter.lastMoveFacingDirection, stashedPrevHeadIndex);
    }

    
    //Must always call first when start game
    public void SetupNewTiles(int rows, int columns)
    {
        tiles = new Tile[rows, columns];
        float tileSize = GetTileSize(); 
        for (int i=0; i<rows; i++)
        {
            for(int j=0; j<columns; j++)
            {
                tiles[i, j] = new Tile(new Vector2(zeroZeroTilePos.position.x + (j * tileSize), zeroZeroTilePos.position.y + (i * tileSize)));
            }
        }
    }

    public void UpdateBoardState()
    {
        Tuple<int, int> toBeIndex = CalculateToBeIndex(_playerCharacterHead, _playerSnakeHead.facingDirectionFromInput);

        //Hit map bound
        if (IsTheIndexOutOfBound(toBeIndex))
        {
            if (!DestroyHeadAndAssignNextHeadThenCheckGameOver(_playerSnakeHead))
            {
                MoveSnake(_playerCharacterHead, _playerSnakeHead.facingDirectionFromInput, CalculateToBeIndex(_playerCharacterHead, _playerSnakeHead.facingDirectionFromInput));
            }
        }
        //Meet a character
        else if (tiles[toBeIndex.Item1, toBeIndex.Item2].occupiedEntity is Character characterInToBeTile)
        {
            switch (characterInToBeTile.CurrentCharacterSide)
            {
                case CharacterSide.PlayerSnake:
                    if (!DestroyHeadAndAssignNextHeadThenCheckGameOver(_playerSnakeHead))
                    {
                        MoveSnake(_playerCharacterHead, _playerSnakeHead.facingDirectionFromInput, CalculateToBeIndex(_playerCharacterHead, _playerSnakeHead.facingDirectionFromInput));
                    }
                    break;
                case CharacterSide.Hero:
                    MoveAndMakeMeetingCharacterJoinTail(characterInToBeTile, _playerSnakeHead.facingDirectionFromInput, toBeIndex);
                    break;
                case CharacterSide.Enemy:
                    CommenceBattle(_playerCharacterHead, characterInToBeTile);
                    
                    if (characterInToBeTile.CurrentHP <= 0)
                    {
                        //Destroy the enemy
                        tiles[toBeIndex.Item1, toBeIndex.Item2].occupiedEntity = null;
                        Destroy(characterInToBeTile.gameObject);
                        GameManager.Instance.IncreaseScore(SumSnakeHP(_playerSnakeHead));
                        GameManager.Instance.IncreaseSnakeSpeed();
                    }
                    //Enemy not dies -> Attack back
                    else
                    {
                        CommenceBattle(characterInToBeTile, _playerCharacterHead);
                        
                        if (_playerCharacterHead.CurrentHP <= 0)
                        {
                            DestroyHeadAndAssignNextHeadThenCheckGameOver(_playerSnakeHead);
                        }
                    }
                    break;
            }
        }
        //Found item
        else if(tiles[toBeIndex.Item1, toBeIndex.Item2].occupiedEntity is Item itemInToBeTile)
        {
            _playerCharacterHead.CurrentATK += itemInToBeTile.itemInfoScriptableObject.atkModifier;
            _playerCharacterHead.CurrentHP += itemInToBeTile.itemInfoScriptableObject.hpModifier;
            _playerCharacterHead.CurrentShield += itemInToBeTile.itemInfoScriptableObject.shieldModifier;
            MoveSnake(_playerCharacterHead, _playerSnakeHead.facingDirectionFromInput, toBeIndex);
        }
        //Found Empty tile
        else
        {
            MoveSnake(_playerCharacterHead, _playerSnakeHead.facingDirectionFromInput, toBeIndex);
        }
    }

    private void CommenceBattle(Character attacker, Character defender)
    {
        int attackerPoint = attacker.CurrentATK;
        if (attacker.CurrentCharacterSide == CharacterSide.PlayerSnake)
        {
            attackerPoint *= 2;
        }

        int damageToDefender = attackerPoint - defender.CurrentShield;
        if (damageToDefender < 1)
        {
            damageToDefender = 1;
        }
        defender.CurrentHP -= damageToDefender;
    }

    private bool DestroyHeadAndAssignNextHeadThenCheckGameOver(PlayerSnakeComponent toBeDestroyedPlayerSnakeHead)
    {
        //Destroy Head
        tiles[toBeDestroyedPlayerSnakeHead.currentIndex.Item1, toBeDestroyedPlayerSnakeHead.currentIndex.Item2].occupiedEntity = null;
        toBeDestroyedPlayerSnakeHead.IsHead = false;
        Destroy(_playerSnakeHead.gameObject);

        //Assign new head if exists
        PlayerSnakeComponent toBeNextHead = toBeDestroyedPlayerSnakeHead.nextLinkedSnakePart;
        if (toBeNextHead != null)
        {
            _playerSnakeHead = toBeNextHead;
            _playerCharacterHead = _playerSnakeHead.GetComponent<Character>();
            _playerSnakeHead.facingDirectionFromInput = toBeDestroyedPlayerSnakeHead.GetComponent<Character>().lastMoveFacingDirection;
            _playerSnakeHead.IsHead = true;
        }
        else
        {
            GameManager.Instance.IsGameOver = true;
            return true;
        }

        return false;
    }

    private void MoveSnake(Character aPlayerPart, FacingDirection toBe_FacingDirection, Tuple<int, int> toBeIndex)
    {
        PlayerSnakeComponent snakeComponent = aPlayerPart.GetComponent<PlayerSnakeComponent>();

        //Tile occupation
        tiles[snakeComponent.currentIndex.Item1, snakeComponent.currentIndex.Item2].occupiedEntity = null;
        tiles[toBeIndex.Item1, toBeIndex.Item2].occupiedEntity = aPlayerPart;
        
        //Position and Rotation
        aPlayerPart.transform.position = tiles[toBeIndex.Item1, toBeIndex.Item2].worldPosition;
        PlayerSnakeComponent.RotateCharacter(aPlayerPart, toBe_FacingDirection);

        //Recursion to next part
        if (snakeComponent.nextLinkedSnakePart != null)
        {
            Character nextPart = snakeComponent.nextLinkedSnakePart.GetComponent<Character>();
            MoveSnake(nextPart, aPlayerPart.lastMoveFacingDirection, snakeComponent.currentIndex);
        }

        //After calling move method -> update current index and facing direction
        snakeComponent.currentIndex = toBeIndex;
        aPlayerPart.lastMoveFacingDirection = toBe_FacingDirection;
    }

    private void MoveAndMakeMeetingCharacterJoinTail(Character meetingCharacter, FacingDirection toBeFacingDirection, Tuple<int, int> toBeIndex)
    {
        //Must call before Move() to store the correct values
        PlayerSnakeComponent lastSnakePartComponent = GetLastSnakePart(_playerSnakeHead);
        Character lastSnakePartCharacter = lastSnakePartComponent.GetComponent<Character>();
        Tuple<int, int> lastSnakePartIndex = lastSnakePartComponent.currentIndex;

        //Move first, then place stashed prev head at the tail
        MoveSnake(_playerCharacterHead, toBeFacingDirection, toBeIndex);

        //Update last part data
        tiles[lastSnakePartIndex.Item1, lastSnakePartIndex.Item2].occupiedEntity = meetingCharacter;
        meetingCharacter.CurrentCharacterSide = CharacterSide.PlayerSnake;
        meetingCharacter.lastMoveFacingDirection = lastSnakePartCharacter.lastMoveFacingDirection;

        //Add snake component
        PlayerSnakeComponent newSnakeComponent = meetingCharacter.gameObject.AddComponent<PlayerSnakeComponent>();
        newSnakeComponent.Setup(false, lastSnakePartIndex);

        //Assign linked part to second-to-last part
        lastSnakePartComponent.nextLinkedSnakePart = newSnakeComponent;

        //Adjust Sprite
        meetingCharacter.spriteRenderer.flipX = lastSnakePartCharacter.spriteRenderer.flipX;
        meetingCharacter.transform.position = tiles[lastSnakePartIndex.Item1, lastSnakePartIndex.Item2].worldPosition;
        PlayerSnakeComponent.RotateCharacter(meetingCharacter, meetingCharacter.lastMoveFacingDirection);
    }

    private PlayerSnakeComponent GetLastSnakePart(PlayerSnakeComponent playerSnakeComponent)
    {
        if (playerSnakeComponent.nextLinkedSnakePart == null)
        {
            return playerSnakeComponent;
        }
        else
        {
            return GetLastSnakePart(playerSnakeComponent.nextLinkedSnakePart);
        }
    }

    private int SumSnakeHP(PlayerSnakeComponent playerSnakeComponent)
    {
        if (playerSnakeComponent.nextLinkedSnakePart == null)
        {
            return playerSnakeComponent.GetComponent<Character>().CurrentHP;
        }
        else
        {
            return playerSnakeComponent.GetComponent<Character>().CurrentHP + SumSnakeHP(playerSnakeComponent.nextLinkedSnakePart);
        }
    }

    #region Spawning
    public void SpawnPlayer_StartGame()
    {
        _playerCharacterHead = ResourceManager.Instance.GeneratePlayerSnake(true, new Tuple<int, int>(1, 4));
        _playerSnakeHead = _playerCharacterHead.GetComponent<PlayerSnakeComponent>();
        MoveSnake(_playerCharacterHead, FacingDirection.Right, new Tuple<int, int>(1, 5));
    }

    //Fix first position to prevent spawning right next to player
    public void SpawnEnemy_StartGame()
    {
        Character newEnemy = ResourceManager.Instance.GenerateEnemy();
        tiles[7, 13].occupiedEntity = newEnemy;
        newEnemy.transform.position = tiles[7, 13].worldPosition;
    }

    public void SpawnAnEnemy()
    {
        Character newEnemy = ResourceManager.Instance.GenerateEnemy();
        Tile tile = GetARandomUnoccupiedTile();
        tile.occupiedEntity = newEnemy;
        newEnemy.transform.position = tile.worldPosition;
    }

    public void SpawnAHero()
    {
        Character newHero = ResourceManager.Instance.GenerateHero();
        Tile tile = GetARandomUnoccupiedTile();
        tile.occupiedEntity = newHero;
        newHero.transform.position = tile.worldPosition;
    }

    public void SpawnAnItem()
    {
        Item newItem = ResourceManager.Instance.GenerateItem();
        Tile tile = GetARandomUnoccupiedTile();
        tile.occupiedEntity = newItem;
        newItem.transform.position = tile.worldPosition;
    }
    #endregion

    #region Public Methods
    public float GetTileSize()
    {
        return zeroOneTilePos.position.x - zeroZeroTilePos.position.x;
    }

    public bool IsTheIndexOutOfBound(Tuple<int, int> index)
    {
        if(index.Item1 < 0 || index.Item2 < 0 || index.Item1 >= tiles.GetLength(0) || index.Item2 >= tiles.GetLength(1))
        {
            return true;
        }

        return false;
    }

    public Tile GetARandomUnoccupiedTile()
    {
        var list = GetUnoccupiedTileList();
        return list[Random.Range(0, list.Count)];
    }

    public List<Tile> GetUnoccupiedTileList()
    {
        List<Tile> tileList = new List<Tile>();
        foreach(Tile tile in tiles)
        {
            if(tile.occupiedEntity == null)
            {
                tileList.Add(tile);
            }
        }

        return tileList;
    } 

    public Tuple<int, int> CalculateToBeIndex(Character character, FacingDirection toBe_FacingDirection)
    {
        int iRow = 0, iCol = 0;
        switch (toBe_FacingDirection)
        {
            case FacingDirection.Right:
                iRow = 0;
                iCol = 1;
                break;
            case FacingDirection.Down:
                iRow = -1;
                iCol = 0;
                break;
            case FacingDirection.Left:
                iRow = 0;
                iCol = -1;
                break;
            case FacingDirection.Up:
                iRow = 1;
                iCol = 0;
                break;
        }
        PlayerSnakeComponent snakeComponent = character.GetComponent<PlayerSnakeComponent>();

        return new Tuple<int, int>(snakeComponent.currentIndex.Item1 + iRow , snakeComponent.currentIndex.Item2 + iCol);
    }
    #endregion

}
