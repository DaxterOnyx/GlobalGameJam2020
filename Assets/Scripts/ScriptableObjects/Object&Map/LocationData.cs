using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class LocationData : ScriptableObject
{
    public int StartDist;
    [SerializeField]
    public Prefab_Pos[] prefabCharacterPositionList;
}

[System.Serializable]
public struct Prefab_Pos
{
    public GameObject prefab;
    public Vector2Int position;
}

public struct Token_Pos
{
	public Token token;
	public Vector2Int position;
	private Vector2Int destination;

	public Token_Pos(Token token, Vector2Int destination) : this()
	{
		this.token = token;
		this.destination = destination;
	}
}
