using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MonstersManager : Location
{

    #region Fields
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
    private List<Token_Pos> hitList;
    private List<Token> attakingMonsters;
    private bool monsterTurn;
    private bool actionCompleted = true;
    private Sequence sequence;
    public float timeAdvretising;
    private float time;
    public GameObject advert;
    void Start()
    {
        hitList = new List<Token_Pos>();
        _instance = this;
        Initialize(data);
        monsterTurn = false;
    }
    #endregion

    #region Monster Turn Management

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
                Token curMonster = attakingMonsters[0];
                attakingMonsters.RemoveAt(0);
                if (curMonster.GetComponent<Monster>().data.target == target.Objective)
                {
                    //Target is Destroy Object
                    Vector2Int pos;
                    MapManager.Instance.WhereIsToken(StructuresManager.Instance.NearestObjective(curMonster), out pos);
                    sequence = DefineActions(curMonster, pos);
                }
                else
                {
                    //Target is players
                    Vector2Int pos;
                    MapManager.Instance.WhereIsToken(PlayersManager.Instance.Nearest(curMonster), out pos);
                    sequence = DefineActions(curMonster, pos);
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
            if (MapManager.Instance.TryGetTokenByPos(hitList[0].position) != null)
            {
                MapManager.Instance.TryGetTokenByPos(hitList[0].position).TakeDamage((hitList[0].token as Monster).data.Strengh);
            }
            hitList.RemoveAt(0);
            atkCount = 0;
        }
        


    }

    public void StartMonsterTurn()
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
        attakingMonsters = new List<Token>();
		//TODO SET TIME TO ADD ANIMATIOn OR SE THIS IN UPDATE
        foreach (var item in objectList)
        {
            attakingMonsters.Add(item);
            
        }
    }

    /// <summary>
    /// Will create the DOTween sequence 
    /// of the monster
    /// </summary>
    /// <param name="token">the monster</param>
    /// <param name="destination">The monster's target (player or objective)</param>
    /// <returns></returns>
    public Sequence DefineActions(Token token,Vector2Int destination)
    {
        int moveCount = (token as Monster).data.nbActionPoint;
        List<Vector2Int> pathComplete = Pathfinding.Instance.findPath(MapManager.Instance.V3toV2I(token.transform.position), destination);
        List<Vector2Int> finalpath = new List<Vector2Int>();
        int lifePointTarget = MapManager.Instance.TryGetTokenByPos(destination).GetComponent<Character>().GetCurrentLp();
        
        ///Movement calculation
        

        for(int i =0; i <= Mathf.Min(moveCount*2, pathComplete.Count - 1); i++)
        {
            if(i != pathComplete.Count - 1) //Remove last case when get to player
            {
                finalpath.Add(pathComplete[i]);
            }
            
        }
        Sequence sequence = MapManager.Instance.Move(token, finalpath);

        ///Attack claculation

        moveCount -= Mathf.FloorToInt((pathComplete.Count - 2 )/ 2) ; // - 2 : fisrt and last case (initial case and player case)
        while (moveCount > 0 && lifePointTarget > 0)
        {

            sequence.Append(DOTween.To(() => atkCount, x => atkCount = x, 1, 0));

            hitList.Add(new Token_Pos(token,destination));

            moveCount--;
            lifePointTarget -= token.GetComponent<Monster>().data.Strengh;
        }
        return sequence;
    }

    #endregion

    public void SpawnMonster(Prefab_Pos item)
    {
        Token token = MapManager.Instance.CreateObject(item.prefab, item.position);
        objectList.Add(token);
    }

}

[System.Serializable]
public struct retardedSpawn
{
    public int turn;
    public Prefab_Pos newObject;
}
