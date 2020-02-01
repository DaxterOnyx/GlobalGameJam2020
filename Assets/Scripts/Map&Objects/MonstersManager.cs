using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MonstersManager : CharactersManager
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
    void Start()
    {
        _instance = this;
        Initialize(data);
    }

    private void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            Vector2Int pos;
            MapManager.Instance.WhereIsObject(PlayersManager.Instance.Nearest(this.gameObject), out pos);
            if(pos == null)
            {
                Debug.LogError("Error : object not found!");
            }
            GotoDest(objectList[0],pos);
        }
    }

    public void GotoDest(GameObject gameObject,Vector2Int destination)
    {
        Sequence sequence = DOTween.Sequence();
        List<Vector2Int> path = Pathfinding.Instance.findPath(V3toV2I(gameObject.transform.position), destination);
        foreach (var item in path)
        {
            sequence.Append(gameObject.transform.DOMove(V2ItoV3(item), MapManager.Instance.data.moveDuration));
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
