using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Area", menuName = "Area")]
public class AreaObject : ScriptableObject
{
    public int areaNumber;
    public string areaName;
    public int gridSize;
    public int obstacles;
    public GameObject primaryTile;
    public GameObject secondaryTile;
    public GameObject menuObject;
}
