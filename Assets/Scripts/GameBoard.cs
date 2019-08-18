using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameBoard : MonoBehaviour
{
    public Tile[,] tiles;
    public Transform zeroZeroTilePos, zeroOneTilePos;

    private PlayerSnake playerSnakeHead;

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
        Character startingPlayerCharacter = ResourceManager.Instance.GeneratePlayerSnake(true);
        tiles[1, 5].occupiedEntity = startingPlayerCharacter;

        playerSnakeHead = startingPlayerCharacter.GetComponent<PlayerSnake>();
        playerSnakeHead.transform.position = tiles[1, 5].worldPosition;
    }

    //Fix first position to prevent spawning right next to player
    public void SpawnEnemy_StartGame()
    {
        Character newEnemy = ResourceManager.Instance.GenerateEnemy();
        tiles[7, 11].occupiedEntity = newEnemy;
        newEnemy.transform.position = tiles[7, 11].worldPosition;
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
