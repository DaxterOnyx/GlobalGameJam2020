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
    private List<GameObject> attakingMonsters;
    private bool monsterTurn;
    private bool actionCompleted = true;
    private Sequence sequence;
    public float timeAdvretising;
    private float time;
    public GameObject advert;
    void Start()
    {
        hitList = new List<GameObj_Vect2>();
        _instance = this;
        Initialize(data);
        monsterTurn = false;
    }

    private void Update()
    {
        if(time > 0)
        {
            advert.SetActive(true);
            time -= Time.deltaTime;
        }
        else if(advert.activeSelf)
        {
            advert.SetActive(false);
        }
        if (monsterTurn)
        {
            if (attakingMonsters.Count > 0 && actionCompleted)
            {
                actionCompleted = false;
                GameObject curMonster = attakingMonsters[0];
                attakingMonsters.RemoveAt(0);
                if (curMonster.GetComponent<Monster>().data.target == target.Objective)
                {
                    //Target is Destroy Object
                    Vector2Int pos;
                    MapManager.Instance.WhereIsObject(ObjectsManager.Instance.NearestObjective(curMonster), out pos);
                    sequence = ActionGesture(curMonster, pos);
                }
                else
                {
                    //Target is players
                    Vector2Int pos;
                    MapManager.Instance.WhereIsObject(PlayersManager.Instance.Nearest(curMonster), out pos);
                    sequence = ActionGesture(curMonster, pos);
                }
            }
            else if (sequence.Elapsed()>= sequence.Duration())
            {
                actionCompleted = true;
            }
            
            if (attakingMonsters.Count == 0 && actionCompleted)
            {
                monsterTurn = false;
                TurnManager.Instance.NextTurn();
            }
        }
        //Hit the player only when in contact
        if (atkCount > 0 && hitList.Count > 0)
        {
            if (MapManager.Instance.TryGetObjectByPos(hitList[0].vector) != null)
            {
                MapManager.Instance.TryGetObjectByPos(hitList[0].vector).TakeDamage(hitList[0].obj.GetComponent<Monster>().data.Strengh);
            }
            hitList.RemoveAt(0);
            atkCount = 0;
        }
        


    }

    public void MonsterTurn()
    {
        foreach (var item in data.retardedSpawns)
        {
            if (item.turn == TurnManager.Instance.Turn)
            {
                time = timeAdvretising;
                SpawnMonster(item.newObject);
            }
        }
        monsterTurn = true;
        attakingMonsters = new List<GameObject>();
		//TODO SET TIME TO ADD ANIMATIOn OR SE THIS IN UPDATE
        foreach (var item in objectList)
        {
            attakingMonsters.Add(item);
            
        }
    }

    public void SpawnMonster(GameObj_Vect2 item)
    {
        Transform obj = MapManager.Instance.CreateObject(item.obj, item.vector);
        objectList.Add(obj.gameObject);
    }

    public Sequence ActionGesture(GameObject gameObject,Vector2Int destination)
    {
        int moveCount = gameObject.GetComponent<Monster>().data.nbActionPoint;
        List<Vector2Int> pathComplete = Pathfinding.Instance.findPath(MapManager.Instance.V3toV2I(gameObject.transform.position), destination);
        List<Vector2Int> finalpath = new List<Vector2Int>();
        int lifePointTarget = MapManager.Instance.TryGetObjectByPos(destination).GetComponent<Character>().GetCurrentLp();
        for(int i =0; i <= Mathf.Min(moveCount*2, pathComplete.Count - 1); i++)
        {
            if(i != pathComplete.Count - 1) //Remove last case when get to player
            {
                finalpath.Add(pathComplete[i]);
            }
            
        }
        Sequence sequence = MapManager.Instance.Move(gameObject, finalpath);
        moveCount -= Mathf.FloorToInt((pathComplete.Count - 2 )/ 2) ; // - 2 : fisrt and last case (initial case and player case)
        while (moveCount > 0 && lifePointTarget > 0)
        {

            sequence.Append(DOTween.To(() => atkCount, x => atkCount = x, 1, 0));
            GameObj_Vect2 hit;
            hit.obj = gameObject;
            hit.vector = destination;
            hitList.Add(hit);
            moveCount--;
            lifePointTarget -= gameObject.GetComponent<Monster>().data.Strengh;
        }
        return sequence;
    }
}

[System.Serializable]
public struct retardedSpawn
{
    public int turn;
    public GameObj_Vect2 newObject;
}
