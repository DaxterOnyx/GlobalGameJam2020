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
    private Dictionary<GameObject, Vector2Int> dico = new Dictionary<GameObject, Vector2Int>();
    private void Awake()
    {
        _instance = this;
    }

    /// <summary>
    /// Move the object to the given position, relative to its position, if it's not taken
    /// </summary>
    /// <param name="item">the </param>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool MoveObject(GameObject item, Vector2Int position)
    {
        if (dico.ContainsValue(position))
        {
            Debug.LogError("Error: Current Position Already Taken!");
            return false;
        }
        else
        {
            dico.Remove(item);
            dico.Add(item, position + V3toV2I(item.transform.position));
            item.transform.DOMove(V2ItoV3(position) + item.transform.position, data.moveDuration);
            return true;
        }
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
    public GameObject TryGetObjectByPos(Vector2Int position)
    {
        GameObject gObj = null;
        foreach (var item in dico)
        {
            if (item.Value == position)
            {
                gObj = item.Key;
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
    private Vector3 V2ItoV3(Vector2Int vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }

    /// <summary>
    /// Convert a Vector3 in a Vector2Int
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    private Vector2Int V3toV2I(Vector3 vector)
    {
        return new Vector2Int((int)vector.x, (int)vector.y);
    }
}
