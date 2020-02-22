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
    private List<Token> attakingMonsters;
    public float timeAdvretising;
    private float time;
    public GameObject advert;
    void Start()
    {
        _instance = this;
        Initialize(data);
    }
    #endregion

    #region Monster Turn Management

    private void Update()
    {
        //Show the advert
        if(time > 0)
        {
            advert.SetActive(true);
            time -= Time.deltaTime;
        }
        else if(advert.activeSelf)
        {
            advert.SetActive(false);
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
        attakingMonsters = new List<Token>();
        foreach (var item in objectList)
        {
            attakingMonsters.Add(item);
        }
        if (attakingMonsters.Count > 0)
            (attakingMonsters[0] as Monster).actions.StartAction();
        else
            TurnManager.Instance.NextTurn();
    }

    public void NextMonsterAction()
    {

        attakingMonsters.RemoveAt(0);
        if (attakingMonsters.Count > 0)
            (attakingMonsters[0] as Monster).actions.StartAction();
        else
            TurnManager.Instance.NextTurn();
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
