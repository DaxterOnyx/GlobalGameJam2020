using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName ="Data/MapData")]
public class MapData : ScriptableObject
{
    [Header("Camera limits")]
    public float CamMinX;
    public float CamMinY, CamMaxX, CamMaxY;
    [Header("Map limits")]
    public int MapMinX;
    public int MapMinY, MapMaxX, MapMaxY;
}
