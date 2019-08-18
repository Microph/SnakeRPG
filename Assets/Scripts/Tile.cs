using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Vector2 worldPosition;
    public Entity occupiedEntity;

    public Tile(Vector2 worldPosition, Entity occupiedEntity = null)
    {
        this.worldPosition = worldPosition;
        this.occupiedEntity = occupiedEntity;
    }
}
