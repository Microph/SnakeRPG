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

    //--------------------------------Spawning------------------------------------------//
    public void SpawnPlayer_StartGame()
    {
        playerCharacterHead = ResourceManager.Instance.GeneratePlayerSnake(true, 1, 4);
        playerSnakeComponent = playerCharacterHead.GetComponent<PlayerSnakeComponent>();
        MoveASnakePart(playerCharacterHead, FacingDirection.Right, 1, 5);
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
    //--------------------------------Spawning------------------------------------------//

    public void UpdateBoardState()
    {
        //TODO: verify to-be tile before move
        Tuple<int, int> toBeIndex = CalculateToBeIndex(playerCharacterHead, playerSnakeComponent.facingDirectionFromInput);
        if (tiles[toBeIndex.Item1, toBeIndex.Item2].occupiedEntity is Character characterCheck)
        {
            //TODO: Get the ally to back next tick instead
            Character stashedHero = characterCheck;
            stashedHero.spriteRenderer.flipX = playerCharacterHead.spriteRenderer.flipX;
            int stashedRow = playerSnakeComponent.currentRow, stashedCol = playerSnakeComponent.currentCol;

            MoveASnakePart(playerCharacterHead, playerSnakeComponent.facingDirectionFromInput, toBeIndex.Item1, toBeIndex.Item2);

            tiles[stashedRow, stashedCol].occupiedEntity = stashedHero;
            stashedHero.transform.position = tiles[stashedRow, stashedCol].worldPosition;
            stashedHero.lastMoveFacingDirection = playerCharacterHead.lastMoveFacingDirection;
            PlayerSnakeComponent.RotateSnakePart(stashedHero, stashedHero.lastMoveFacingDirection);
        }
        else
        {
            MoveASnakePart(playerCharacterHead, playerSnakeComponent.facingDirectionFromInput, toBeIndex.Item1, toBeIndex.Item2);
        }
    }

    private void MoveASnakePart(Character aPart, FacingDirection toBe_FacingDirection, int toBeRow, int toBeCol)
    {
        PlayerSnakeComponent snakeComponent = aPart.GetComponent<PlayerSnakeComponent>();
        tiles[toBeRow, toBeCol].occupiedEntity = aPart;
        tiles[snakeComponent.currentRow, snakeComponent.currentCol].occupiedEntity = null;
        snakeComponent.currentRow = toBeRow;
        snakeComponent.currentCol = toBeCol;

        //Update Position and Rotation
        aPart.transform.position = tiles[toBeRow, toBeCol].worldPosition;
        PlayerSnakeComponent.RotateSnakePart(aPart, toBe_FacingDirection);

        //Recursion
        if (snakeComponent.nextLinkedPartRow != -1)
        {
            Tuple<int, int> toBeIndex = CalculateToBeIndex(aPart, snakeComponent.facingDirectionFromInput);
            MoveASnakePart((Character)tiles[snakeComponent.nextLinkedPartRow, snakeComponent.nextLinkedPartCol].occupiedEntity, aPart.lastMoveFacingDirection, toBeIndex.Item1, toBeIndex.Item2);
        }
        aPart.lastMoveFacingDirection = toBe_FacingDirection;
    }

    //--------------------------------Utility------------------------------------------//
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
        int toBeRow = snakeComponent.currentRow + iRow;
        int toBeCol = snakeComponent.currentCol + iCol;

        return new Tuple<int, int>(toBeRow, toBeCol);
    }
    //--------------------------------Utility------------------------------------------//
}
