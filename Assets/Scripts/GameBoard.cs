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

    private Character playerCharacterHead;
    private PlayerSnakeComponent playerSnakeHead;

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

    #region Spawning
    public void SpawnPlayer_StartGame()
    {
        playerCharacterHead = ResourceManager.Instance.GeneratePlayerSnake(true, new Tuple<int, int>(1, 4));
        playerSnakeHead = playerCharacterHead.GetComponent<PlayerSnakeComponent>();
        MoveSnake(playerCharacterHead, FacingDirection.Right, new Tuple<int, int>(1, 5));
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

    public void UpdateBoardState()
    {
        Tuple<int, int> toBeIndex = CalculateToBeIndex(playerCharacterHead, playerSnakeHead.facingDirectionFromInput);

        //Hit map bound
        if (IsTheIndexOutOfBound(toBeIndex))
        {
            DestroyHeadAndAssignNextHead(playerSnakeHead);
        }
        //Meet a character
        else if (tiles[toBeIndex.Item1, toBeIndex.Item2].occupiedEntity is Character characterInToBeTile)
        {
            switch (characterInToBeTile.characterSide)
            {
                case CharacterSide.Hero:
                    PlayerSnakeComponent lastPartSnakeComponent = GetLastPartFromHead(playerSnakeHead);
                    Character lastPlayerPart = lastPartSnakeComponent.GetComponent<Character>();

                    //Stash the hero (will place it at the tail after player finish moving)
                    Character stashedHero = characterInToBeTile;
                    stashedHero.spriteRenderer.flipX = lastPlayerPart.spriteRenderer.flipX;
                    Tuple<int, int> stashedIndex = lastPartSnakeComponent.currentIndex;

                    //Move first, then place new hero at the tail
                    MoveSnake(playerCharacterHead, playerSnakeHead.facingDirectionFromInput, toBeIndex);
                    tiles[stashedIndex.Item1, stashedIndex.Item2].occupiedEntity = stashedHero;
                    stashedHero.characterSide = CharacterSide.PlayerSnake;
                    stashedHero.transform.position = tiles[stashedIndex.Item1, stashedIndex.Item2].worldPosition;
                    stashedHero.lastMoveFacingDirection = lastPlayerPart.lastMoveFacingDirection;
                    PlayerSnakeComponent.RotateCharacter(stashedHero, stashedHero.lastMoveFacingDirection);

                    //Add snake component to the hero
                    PlayerSnakeComponent newSnakeComponent = stashedHero.gameObject.AddComponent<PlayerSnakeComponent>();
                    newSnakeComponent.Setup(false, stashedIndex);

                    //Assign linked part to second-to-last part
                    lastPartSnakeComponent.nextLinkedSnakePart = newSnakeComponent;
                    break;
                case CharacterSide.Enemy:
                    break;
                case CharacterSide.PlayerSnake:
                    DestroyHeadAndAssignNextHead(playerSnakeHead);
                    break;
            }
        }
        //Found item
        else if(tiles[toBeIndex.Item1, toBeIndex.Item2].occupiedEntity is Item itemInToBeTile)
        {

        }
        //Found Empty tile
        else
        {
            MoveSnake(playerCharacterHead, playerSnakeHead.facingDirectionFromInput, toBeIndex);
        }
    }

    private void DestroyHeadAndAssignNextHead(PlayerSnakeComponent toBeDestroyedPlayerSnakeHead)
    {
        //Destroy Head
        tiles[toBeDestroyedPlayerSnakeHead.currentIndex.Item1, toBeDestroyedPlayerSnakeHead.currentIndex.Item2].occupiedEntity = null;
        toBeDestroyedPlayerSnakeHead.IsHead = false;
        Destroy(playerSnakeHead.gameObject);

        //Assign new head if exists
        PlayerSnakeComponent toBeNextHead = toBeDestroyedPlayerSnakeHead.nextLinkedSnakePart;
        if (toBeNextHead != null)
        {
            playerCharacterHead = toBeNextHead.GetComponent<Character>();
            playerSnakeHead = toBeNextHead;
            toBeNextHead.IsHead = true;
            playerSnakeHead.facingDirectionFromInput = toBeDestroyedPlayerSnakeHead.GetComponent<Character>().lastMoveFacingDirection;
        }
    }

    private void MoveSnake(Character aPlayerPart, FacingDirection toBe_FacingDirection, Tuple<int, int> toBeIndex)
    {
        PlayerSnakeComponent snakeComponent = aPlayerPart.GetComponent<PlayerSnakeComponent>();

        //Tile occupation
        tiles[toBeIndex.Item1, toBeIndex.Item2].occupiedEntity = aPlayerPart;
        tiles[snakeComponent.currentIndex.Item1, snakeComponent.currentIndex.Item2].occupiedEntity = null;
        
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

    #region Utility
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

    private PlayerSnakeComponent GetLastPartFromHead(PlayerSnakeComponent playerSnakeComponent)
    {
        if (playerSnakeComponent.nextLinkedSnakePart == null)
        {
            return playerSnakeComponent;
        }
        else
        {
            return GetLastPartFromHead(playerSnakeComponent.nextLinkedSnakePart);
        }
    }
    #endregion
}
