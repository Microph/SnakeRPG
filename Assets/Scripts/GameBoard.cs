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

    public void SpawnPlayer_StartGame()
    {
        playerCharacterHead = ResourceManager.Instance.GeneratePlayerSnake(true, 1, 4);
        playerSnakeComponent = playerCharacterHead.GetComponent<PlayerSnakeComponent>();
        MovePlayerSnakePart(playerCharacterHead, FacingDirection.Right);
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
        //throw new NotImplementedException();
    }

    public void SpawnAHero()
    {
        //throw new NotImplementedException();
    }

    public void SpawnAnItem()
    {
        //throw new NotImplementedException();
    }

    public void UpdatePlayerSnake()
    {
        //TODO: verify to-be tile before move
        MovePlayerSnakePart(playerCharacterHead, playerCharacterHead.lastMoveFacingDirection);
    }

    private void MovePlayerSnakePart(Character aPart, FacingDirection toBe_FacingDirection)
    {
        int iRow = 0, iCol = 0;
        switch (playerCharacterHead.lastMoveFacingDirection)
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

        PlayerSnakeComponent snakeComponent = aPart.GetComponent<PlayerSnakeComponent>();
        int toBeRow = snakeComponent.currentRow + iRow;
        int toBeCol = snakeComponent.currentCol + iCol;
        tiles[toBeRow, toBeCol].occupiedEntity = aPart;
        tiles[snakeComponent.currentRow, snakeComponent.currentCol].occupiedEntity = null;
        aPart.transform.position = tiles[toBeRow, toBeCol].worldPosition;
        snakeComponent.currentRow = toBeRow;
        snakeComponent.currentCol = toBeCol;

        if (snakeComponent.nextLinkedPartRow != -1)
        {
            MovePlayerSnakePart((Character)tiles[snakeComponent.nextLinkedPartRow, snakeComponent.nextLinkedPartCol].occupiedEntity, aPart.lastMoveFacingDirection);
        }
        aPart.lastMoveFacingDirection = toBe_FacingDirection;
    }

    //--------------------------------Utility------------------------------------------//
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

    public float GetTileSize()
    {
        return zeroOneTilePos.position.x - zeroZeroTilePos.position.x;
    }
}
