using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Vector2 worldPosition;
    public IEntity occupiedEntity;

    public Tile(Vector2 worldPosition, IEntity occupiedEntity = null)
    {
        this.worldPosition = worldPosition;
    }
}
