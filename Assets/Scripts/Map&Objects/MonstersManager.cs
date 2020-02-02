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
        hitList = new List<GameObj_Vect2>();
        _instance = this;
        Initialize(data);
    }

    private void Update()
    {
        if(atkCount>0&&hitList.Count > 0)
        {
            if (MapManager.Instance.TryGetObjectByPos(hitList[0].vector)!=null)
            {
                MapManager.Instance.TryGetObjectByPos(hitList[0].vector).TakeDamage(hitList[0].obj.GetComponent<Monster>().data.Strengh);
            }
            hitList.RemoveAt(0);
        }
        if (atkCount > 0)
        {
            atkCount -= Time.deltaTime;
        }

    }

    public void MonsterTurn()
    {
		//TODO SET TIME TO ADD ANIMATIOn OR SE THIS IN UPDATE
        foreach (var item in objectList)
        {
            if(item.GetComponent<Monster>().data.target == target.Objective)
            {
			//Target is Destroy Object
                Vector2Int pos;
                MapManager.Instance.WhereIsObject(ObjectsManager.Instance.NearestObjective(item),out pos);
                ActionGesture(item, pos);
            }
            else
            {
				//Target is players
                Vector2Int pos;
                MapManager.Instance.WhereIsObject(PlayersManager.Instance.Nearest(item), out pos);
                ActionGesture(item, pos);
            }
        }

		TurnManager.Instance.NextTurn();
    }

    public void ActionGesture(GameObject gameObject,Vector2Int destination)
    {
        int moveCount = gameObject.GetComponent<Monster>().data.nbActionPoint;
        List<Vector2Int> pathComplete = Pathfinding.Instance.findPath(MapManager.Instance.V3toV2I(gameObject.transform.position), destination);
        List<Vector2Int> finalpath = new List<Vector2Int>();
        for(int i =0; i <= Mathf.Min(moveCount, pathComplete.Count - 1); i++)
        {
            finalpath.Add(pathComplete[i]);
        }
        Sequence sequence = MapManager.Instance.Move(gameObject, finalpath);
        moveCount -= pathComplete.Count;
        while (moveCount > 0)
        {
            sequence.Append(DOTween.To(() => atkCount, x => atkCount = x, 1, MapManager.Instance.data.moveDuration));
            GameObj_Vect2 hit;
            hit.obj = gameObject;
            hit.vector = destination;
            hitList.Add(hit);
            moveCount--;
        }
    }
}
