using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : Tile
{
    [SerializeField] Material normalMaterial, offsetMaterial;

    public override void Init(Vector2 pos)
    {
        gridPosition = pos;

        bool isOffset = Mathf.Abs((pos.x + pos.y)) % 2 == 1;
        meshRenderer.material = isOffset ? offsetMaterial : normalMaterial;
    }
}
