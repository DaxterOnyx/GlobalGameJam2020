using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MonstersManager : Location
{
    private static MonstersManager _instance;
    public static MonstersManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<MonstersManager>();
            return _instance;
        }
    }
    public MonsterManagerData data;
    private float atkCount;
    private List<GameObj_Vect2> hitList;
    void Start()
    {
        _instance = this;
        Initialize(data);
    }

    private void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            MonsterTurn();
        }
        if(atkCount>0&&hitList.Count > 0)
        {
            MapManager.Instance.TryGetObjectByPos(hitList[0].vector).TakeDamage(hitList[0].obj.GetComponent<Monster>().data.Strengh);
        }

    }

    public void MonsterTurn()
    {
        foreach (var item in objectList)
        {
            if(item.GetComponent<Monster>().data.target == target.Objective)
            {
                Vector2Int pos;
                MapManager.Instance.WhereIsObject(ObjectsManager.Instance.NearestObjective(item),out pos);
                ActionGesture(item, pos);
            }
            else
            {
                Vector2Int pos;
                MapManager.Instance.WhereIsObject(PlayersManager.Instance.Nearest(item), out pos);
                ActionGesture(item, pos);
            }
        }
    }

    public void ActionGesture(GameObject gameObject,Vector2Int destination)
    {
        Sequence sequence = DOTween.Sequence();
        List<Vector2Int> path = Pathfinding.Instance.findPath(V3toV2I(gameObject.transform.position), destination);
        int moveCount = gameObject.GetComponent<Monster>().data.nbActionPoint;
        foreach (var item in path)
        {
            if (moveCount > 0)
            {
                sequence.Append(gameObject.transform.DOMove(V2ItoV3(item), MapManager.Instance.data.moveDuration));
                moveCount--;
            }
        }
        while (moveCount > 0)
        {
            sequence.Append(DOTween.To(() => atkCount, x => atkCount = x, 1, MapManager.Instance.data.moveDuration));
            GameObj_Vect2 hit;
            hit.obj = gameObject;
            hit.vector = destination;
            hitList.Add(hit);
        }
    }


    private Vector3 V2ItoV3(Vector2Int vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }

    private Vector2Int V3toV2I(Vector3 vector)
    {
        return new Vector2Int((int)vector.x, (int)vector.y);
    }
}
