using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Location : MonoBehaviour
{

    protected List<Token> objectList = new List<Token>();
    protected int startDist;
    // Start is called before the first frame update

    /// <summary>
    /// Instantiate object and add them to objectList
    /// </summary>
    public void Initialize(LocationData data)
    {
        foreach (var item in data.prefabCharacterPositionList)
        {
            Token obj = MapManager.Instance.CreateObject(item.prefab, item.position);
            objectList.Add(obj);
        }
        startDist = data.StartDist;
    }


    /// <summary>
    /// Delete the player from the game
    /// </summary>
    /// <param name="item"></param>
    public void Kill(Token item)
    {
        objectList.Remove(item);
        MapManager.Instance.DeleteObject(item);
    }


    /// <summary>
    /// Return the nearest Object from parameter
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Token Nearest(Token token)
    {
        Vector2Int position;
        MapManager.Instance.WhereIsToken(token, out position);
        Vector2Int playerPos;
        int c;
        (int, Token) smallestDist_Obj = (startDist,null);
        foreach (var item in objectList)
        {
            MapManager.Instance.WhereIsToken(item,out playerPos);
            c = Pathfinding.Instance.findPath(position, playerPos).Count;
            if (smallestDist_Obj.Item1 > c)
            {
                smallestDist_Obj = (c,item);
            }
        }
        return smallestDist_Obj.Item2;
    }

    public void HighlightTargets(Player player,int dist)
    {
        foreach (var item in objectList)
        {
            if (Pathfinding.Instance.PathLenght(
                MapManager.Instance.V3toV2I(player.transform.position),
                MapManager.Instance.V3toV2I(item.transform.position)) < dist)
                item.Highlight();
        }
    }
    public void DelightTargets()
    {
        foreach (var item in objectList)
        {
            item.GetComponent<Token>().Delight();
        }
    }

}
