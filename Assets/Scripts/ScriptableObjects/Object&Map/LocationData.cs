using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class LocationData : ScriptableObject
{
    public int StartDist;
    [SerializeField]
    public GameObj_Vect2[] characterPositionList;
}

[System.Serializable]
public struct GameObj_Vect2
{
    public GameObject obj;
    public Vector2Int vector;
}
