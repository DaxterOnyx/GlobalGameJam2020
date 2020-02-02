using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<MapManager>();
            return _instance;
        }
    }

    public MapData data;
    public List<GameObject> caseList = new List<GameObject>();
    private Dictionary<GameObject, Vector2Int> dico = new Dictionary<GameObject, Vector2Int>();
    private void Awake()
    {
        _instance = this;
    }
    private void Update()
    {
        
    }

    public bool GenerateCaseMap(GameObject player, int maxCost)
    {
        RecCreationCase(V3toV2I(player.transform.position), V3toV2I(player.transform.position), maxCost, player);
        GameObject initCase = caseList[0];
        caseList.Remove(initCase);
        GameObject.Destroy(initCase);
        return true;
    }

    public void DestroyCaseMap()
    {
        foreach (var item in caseList)
        {
            GameObject.Destroy(item);
        }
        caseList.Clear();
    }
    public void RecCreationCase(Vector2Int pos, Vector2Int initialPos, int maxCost,GameObject player)
    {
        GameObject curCase;
        curCase = CreateCase(pos, initialPos, maxCost,player);
        if (curCase != null)
        {
            RecCreationCase(pos + new Vector2Int(0, 1), initialPos, maxCost,player);
            RecCreationCase(pos + new Vector2Int(1, 0), initialPos, maxCost,player);
            RecCreationCase(pos + new Vector2Int(0, -1), initialPos, maxCost,player);
            RecCreationCase(pos + new Vector2Int(-1, 0), initialPos, maxCost,player);
        }
    }

    public GameObject CreateCase(Vector2Int pos,Vector2Int initialPos,int maxCost, GameObject player)
    {
        GameObject curCase = null;
        if (!containByPos(caseList, pos) && (!CaseTaken(pos)||pos==initialPos))
        {
            if(CalculateCost(pos, initialPos) <= maxCost)
            {
                curCase = Instantiate(data.caseObject);
			    Vector3 vector3 = V2ItoV3(pos);
			    vector3.z = 10;
			    curCase.transform.position = vector3;

			    caseList.Add(curCase);
                curCase.GetComponent<CaseObject>().moveCost = CalculateCost(pos,initialPos);
                curCase.GetComponent<CaseObject>().SetPlayer(player);
                curCase.GetComponent<CaseObject>().UpdateMaterial();
            }

        }
        return curCase;
    }

    public int CalculateCost(Vector2Int posA, Vector2Int posB)
    {
        if(posA == posB)
        {
            return 0;
        }
        return  Mathf.CeilToInt(Pathfinding.Instance.findPath(posA,posB).Count/2) ;
    }
    public bool containByPos(List<GameObject> listObj, Vector2 pos)
    {
        foreach (var item in listObj)
        {
            if (V3toV2I(item.transform.position) == pos)
            {
                return true;
            }
        }
        return false;
    }



    /// <summary>
    /// Move the object to the given position, relative to its position, if it's not taken
    /// </summary>
    /// <param name="item">the </param>
    /// <param name="position"></param>
    /// <returns></returns>
    public Sequence Move(GameObject item, List<Vector2Int> path)
    {
        Sequence sequence = DOTween.Sequence();
        foreach (var position in path)
        {
            if (dico.ContainsValue(position))
            {
                Debug.LogWarning("Error: Current Position Already Taken!");
            }
            else
            {
                sequence.Append(item.transform.DOMove(V2ItoV3(position), data.moveDuration));
            }
        }
        dico.Remove(item);
        dico.Add(item, path[path.Count-1]);
        return sequence;
        
    }

    /// <summary>
    /// Return if the case is taken by something
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CaseTaken(Vector2Int pos)
    {
        return dico.ContainsValue(pos);
    }

    /// <summary>
    /// Return if an object is found, and where it is if found
    /// </summary>
    /// <param name="item">The item to find</param>
    /// <returns></returns>
    public bool WhereIsObject(GameObject item)
    {
        return dico.ContainsKey(item);
    }

    /// <summary>
    /// Return a game object by getting his position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Character TryGetObjectByPos(Vector2Int position)
    {
        Character gObj = null;
        foreach (var item in dico)
        {
            if (item.Value == position)
            {
                gObj = item.Key.GetComponent<Character>();
            }
        }
        return gObj;

    }

    /// <summary>
    /// Return if an object is found, and where it is if found
    /// </summary>
    /// <param name="item">The item to find</param>
    /// <param name="position"> The Vector2 of the position to return</param>
    /// <returns></returns>
    public bool WhereIsObject(GameObject item, out Vector2Int position)
    {
        if (dico.TryGetValue(item, out position))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Instantiate a new object at a given position if this position is free
    /// </summary>
    /// <param name="item">The GameObject you want to instantiate</param>
    /// <param name="position">The position on the grid</param>
    /// <returns>The game object instance</returns>
    public Transform CreateObject(GameObject item, Vector2Int position)
    {
        if (dico.ContainsValue(position))
        {
            Debug.LogError("Error: Can't create object, position already taken!");
            return null;
        }
        GameObject obj = Instantiate(item, transform);
        dico.Add(obj, position);
        obj.transform.position = V2ItoV3(position);
        Debug.Log("Object succefully created!");
        return obj.transform;
    }

    /// <summary>
    /// Delete the given GameObject from the game
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public void DeleteObject(GameObject item)
    {
        dico.Remove(item);
        Destroy(item);
    }

    /// <summary>
    /// Convert a Vector2Int in a Vector3
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public Vector3 V2ItoV3(Vector2Int vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }

    /// <summary>
    /// Convert a Vector3 in a Vector2Int
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public Vector2Int V3toV2I(Vector3 vector)
    {
        return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
    }
}
