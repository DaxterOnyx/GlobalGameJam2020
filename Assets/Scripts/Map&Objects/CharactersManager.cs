using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{

    public CharacterManagerData data;
    private List<GameObject> objectList = new List<GameObject>();
    // Start is called before the first frame update

    /// <summary>
    /// Instantiate object and add them to objectList
    /// </summary>
    public virtual void Initialize()
    {
        foreach (var item in data.characterPositionList)
        {
            objectList.Add(MapManager.Instance.CreateObject(item.obj, item.vector));
        }
    }


    /// <summary>
    /// Delete the player from the game
    /// </summary>
    /// <param name="item"></param>
    public void Kill(GameObject item)
    {
        objectList.Remove(item);
        MapManager.Instance.DeleteObject(item);
    }


    /// <summary>
    /// Return the nearest player from this position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject Nearest(GameObject gameObject)
    {
        Vector2Int position;
        MapManager.Instance.WhereIsObject(gameObject, out position);
        Vector2Int playerPos;
        (float, GameObject) smallestDist_Obj = (data.StartDist,null);
        foreach (var item in objectList)
        {
            MapManager.Instance.WhereIsObject(item,out playerPos);
            if (smallestDist_Obj.Item1 > Vector2Int.Distance(position, playerPos))
            {
                smallestDist_Obj = (Vector2Int.Distance(position, playerPos), item);
            }
        }
        return smallestDist_Obj.Item2;
    }



}
