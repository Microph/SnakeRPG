using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameBoard : MonoBehaviour
{
    public Tile[,] tiles;
    public Transform zeroZeroTilePos, zeroOneTilePos;

    private Character playerCharacterHead;
    private PlayerSnakeComponent playerSnakeComponent;

    //Must always happen first
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
        playerSnakeComponent = playerCharacterHead.GetComponent<PlayerSnakeComponent>();
        MoveASnakePart(playerCharacterHead, FacingDirection.Right, new Tuple<int, int>(1, 5));
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
        //TODO: verify to-be tile before move
        Tuple<int, int> toBeIndex = CalculateToBeIndex(playerCharacterHead, playerSnakeComponent.facingDirectionFromInput);
        if (tiles[toBeIndex.Item1, toBeIndex.Item2].occupiedEntity is Character characterCheck)
        {
            //TODO: Get the ally to back next tick instead
            PlayerSnakeComponent lastPartSnakeComponent = GetLastPartFromHead(playerSnakeComponent);
            Character lastPlayerPart = lastPartSnakeComponent.GetComponent<Character>();

            Character stashedHero = characterCheck;
            stashedHero.spriteRenderer.flipX = lastPlayerPart.spriteRenderer.flipX;
            Tuple<int, int> stashedIndex = lastPartSnakeComponent.currentIndex;

            //Move first then place newly gen at the last line //will be change to add next tick
            MoveASnakePart(playerCharacterHead, playerSnakeComponent.facingDirectionFromInput, toBeIndex);

            tiles[stashedIndex.Item1, stashedIndex.Item2].occupiedEntity = stashedHero;
            stashedHero.transform.position = tiles[stashedIndex.Item1, stashedIndex.Item2].worldPosition;
            stashedHero.lastMoveFacingDirection = lastPlayerPart.lastMoveFacingDirection;
            PlayerSnakeComponent.RotateSnakePart(stashedHero, stashedHero.lastMoveFacingDirection);

            PlayerSnakeComponent newSnakeComponent = stashedHero.gameObject.AddComponent<PlayerSnakeComponent>();
            newSnakeComponent.Setup(false, stashedIndex);

            lastPartSnakeComponent.nextLinkedIndex = stashedIndex;
        }
        else
        {
            MoveASnakePart(playerCharacterHead, playerSnakeComponent.facingDirectionFromInput, toBeIndex);
        }
    }
    
    private void MoveASnakePart(Character aPlayerPart, FacingDirection toBe_FacingDirection, Tuple<int, int> toBeIndex)
    {
        PlayerSnakeComponent snakeComponent = aPlayerPart.GetComponent<PlayerSnakeComponent>();
        tiles[toBeIndex.Item1, toBeIndex.Item2].occupiedEntity = aPlayerPart;
        tiles[snakeComponent.currentIndex.Item1, snakeComponent.currentIndex.Item2].occupiedEntity = null;
        //Update Position and Rotation
        aPlayerPart.transform.position = tiles[toBeIndex.Item1, toBeIndex.Item2].worldPosition;
        PlayerSnakeComponent.RotateSnakePart(aPlayerPart, toBe_FacingDirection);

        //Recursion
        if (!snakeComponent.nextLinkedIndex.Equals(new Tuple<int, int>(-1, -1)))
        {
            Character nextPart = (Character)tiles[snakeComponent.nextLinkedIndex.Item1, snakeComponent.nextLinkedIndex.Item2].occupiedEntity;
            snakeComponent.nextLinkedIndex = snakeComponent.currentIndex;
            MoveASnakePart(nextPart, aPlayerPart.lastMoveFacingDirection, snakeComponent.currentIndex);
        }

        snakeComponent.currentIndex = toBeIndex;
        aPlayerPart.lastMoveFacingDirection = toBe_FacingDirection;
    }

    #region Utility
    public float GetTileSize()
    {
        return zeroOneTilePos.position.x - zeroZeroTilePos.position.x;
    }

    private Tile GetARandomUnoccupiedTile()
    {
        var list = GetUnoccupiedTileList();
        return list[Random.Range(0, list.Count)];
    }

    private List<Tile> GetUnoccupiedTileList()
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

    private Tuple<int, int> CalculateToBeIndex(Character character, FacingDirection toBe_FacingDirection)
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
        if (playerSnakeComponent.nextLinkedIndex.Equals(new Tuple<int, int>(-1, -1)))
        {
            return playerSnakeComponent;
        }
        else
        {
            return GetLastPartFromHead(tiles[playerSnakeComponent.nextLinkedIndex.Item1, playerSnakeComponent.nextLinkedIndex.Item2].occupiedEntity.GetComponent<PlayerSnakeComponent>());
        }
    }
    #endregion
}
