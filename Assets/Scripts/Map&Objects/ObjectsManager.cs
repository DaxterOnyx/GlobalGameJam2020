using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager : Location
{

    private static ObjectsManager _instance;
    private List<GameObject> objectivesList = new List<GameObject>();
    public static ObjectsManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<ObjectsManager>();
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
        foreach (var item in data.characterPositionList)
        {
            Transform obj = MapManager.Instance.CreateObject(item.obj, item.vector);
            objectList.Add(obj.gameObject);
            if(obj.GetComponent<Object>().data.isObjective)
            {
                objectivesList.Add(obj.gameObject);
            }
        }
        startDist = data.StartDist;
    }

    public GameObject NearestObjective(GameObject gameObject)
    {
        Vector2Int position;
        MapManager.Instance.WhereIsObject(gameObject, out position);
        Vector2Int playerPos;
        (float, GameObject) smallestDist_Obj = (startDist, null);
        foreach (var item in objectivesList)
        {
            MapManager.Instance.WhereIsObject(item, out playerPos);
            if (smallestDist_Obj.Item1 > Vector2Int.Distance(position, playerPos))
            {
                smallestDist_Obj = (Vector2Int.Distance(position, playerPos), item);
            }
        }
        return smallestDist_Obj.Item2;
    }

    public void RemoveObjective(GameObject game)
    {
        objectivesList.Remove(game);
    }

    public int HowManyObjectivesLeft()
    {
        return objectivesList.Count;
    }
}
