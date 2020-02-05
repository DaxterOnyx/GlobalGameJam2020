using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructuresManager : Location
{

    private static StructuresManager _instance;
    private List<Token> objectivesList = new List<Token>();
    public static StructuresManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<StructuresManager>();
            return _instance;
        }
    }
    public ObjectsManagerData data;
    void Awake()
    {
        _instance = this;
        Initialize(data);
    }

    public new void Initialize(LocationData data)
    {
        foreach (var item in data.prefabCharacterPositionList)
        {
            Token token = MapManager.Instance.CreateObject(item.prefab, item.position);
            objectList.Add(token);
            if(token.GetComponent<Structure>().data.isObjective)
            {
                objectivesList.Add(token);
            }
        }
        startDist = data.StartDist;
    }

    public Token NearestObjective(Token gameObject)
    {
        Vector2Int position;
        MapManager.Instance.WhereIsToken(gameObject, out position);
        Vector2Int playerPos;
        (float, Token) smallestDist_Obj = (startDist, null);
        foreach (var item in objectivesList)
        {
            MapManager.Instance.WhereIsToken(item, out playerPos);
            if (smallestDist_Obj.Item1 > Vector2Int.Distance(position, playerPos))
            {
                smallestDist_Obj = (Vector2Int.Distance(position, playerPos), item);
            }
        }
        return smallestDist_Obj.Item2;
    }

    public void RemoveObjective(Token token)
    {
        objectivesList.Remove(token);
    }

    public int HowManyObjectivesLeft()
    {
        return objectivesList.Count;
    }

    public void HighlightObjectives(GameObject player, int dist)
    {
        foreach (var item in objectivesList)
        {
            if (Pathfinding.Instance.PathLenght(
                MapManager.Instance.V3toV2I(player.transform.position),
                MapManager.Instance.V3toV2I(item.transform.position)) < dist)
                item.GetComponent<Character>().Highlight();
        }
    }
}
