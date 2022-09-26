using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Area", menuName = "Area")]
public class AreaObject : ScriptableObject
{
    public int areaNumber;
    public string areaName;
    public int starsToComplete;
    public int gridSize;
    public int stones;
    public int obstacles;
    public Tile primaryTile;
    public Tile secondaryTile;
    public int[] starValues;
    public GameObject menuObject;
    public Color backgroundColor;
}
