using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryTile : Tile
{
    public override void Init(Vector2 pos)
    {
        gridPosition = pos;
        occupied = true;
    }
}
