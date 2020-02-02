using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private static Pathfinding _instance;
    public static Pathfinding Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<Pathfinding>();
            return _instance;
        }
    }
    private List<Case> openList;
    private List<Case> closedList;
    private List<Vector2Int> path;
    public int LimitLoop;

    private void Awake()
    {
        _instance = this;
        openList = new List<Case>();
        closedList = new List<Case>();
        path = new List<Vector2Int>();
    }

    public List<Vector2Int> findPath(Vector2Int initialPos, Vector2Int finalPos)
    {
        openList.Clear();
        closedList.Clear();
        path.Clear();


        Case iCase = CreateCase(initialPos, finalPos, null);
        openList.Add(iCase);
        iCase = recPathfinding(finalPos,0);
        if (iCase == null)
        {
            Debug.LogError("Error: No path found!");
        }
        else
        {
            FulfillPath(iCase.parent);
        }
        return path;
    }

    public void FulfillPath(Case curCase)
    {
        if (curCase.parent == null)
        {
            path.Add(curCase.position);
        }
        else
        {
            FulfillPath(curCase.parent);
            path.Add(curCase.position);
        }
    }

    public Case recPathfinding(Vector2Int finalPos, int counter)
    {
        Case curCase = caseWithSmallestF(openList);
        // Calculationg for neighbor cases
        if (ListContainsNodeAt(closedList, finalPos, ref curCase))
        {
            return curCase;
        }
        else if (openList.Count == 0 || counter>LimitLoop)
        {
            Debug.Log("No path Found");
            return null;
        }
        closedList.Add(curCase);
        openList.Remove(curCase);
        calculatingCase(curCase.position + new Vector2Int(0, 1), curCase, finalPos);
        calculatingCase(curCase.position + new Vector2Int(1, 0), curCase, finalPos);
        calculatingCase(curCase.position + new Vector2Int(0, -1), curCase, finalPos);
        calculatingCase(curCase.position + new Vector2Int(-1, 0), curCase, finalPos);
        return recPathfinding(finalPos,counter+1);
    }

    public void calculatingCase(Vector2Int pos, Case parent, Vector2Int finalPos)
    {
        Case curCase = new Case();
        if (!((MapManager.Instance.CaseTaken(pos) && finalPos!=pos) || ListContainsNodeAt(closedList, pos)))
        {
            if (!ListContainsNodeAt(openList, pos, ref curCase))
            {
                curCase = CreateCase(pos, finalPos, parent);
                openList.Add(curCase);
            }
            else
            {
                if (curCase.g > parent.g + 1)
                {
                    ChangeParent(curCase, parent);
                }
            }
        }
    }
    // Case Gesture
    public bool ListContainsNodeAt(List<Case> lCase, Vector2Int pos)
    {
        foreach (var item in lCase)
        {
            if (item.position == pos)
            {
                return true;
            }
        }
        return false;
    }
    
    public Case caseWithSmallestF(List<Case> list)
    {
        Case retCase = list[0];
        foreach (var item in list)
        {
            if (item.f < retCase.f)
            {
                retCase = item;
            }
        }
        return retCase;
    }

    public bool ListContainsNodeAt(List<Case> lCase, Vector2Int pos, ref Case pCase)
    {
        foreach (var item in lCase)
        {
            if (item.position == pos)
            {
                pCase = item;
                return true;
            }
        }
        return false;
    }

    public void ChangeParent(Case curCase, Case parentCase)
    {
        curCase.parent = parentCase;
        if (parentCase == null)
        {
            curCase.g = 0;
        }
        else
        {
            curCase.g = parentCase.g + 1;
            curCase.f = curCase.g + curCase.h;
        }
    }

    public Case CreateCase(Vector2Int position, Vector2Int finalPos, Case parent)
    {
        Case curCase = new Case();
        curCase.position = position;
        curCase.parent = parent;
        if(parent == null)
        {
            curCase.g = 0;
        }
        else
        {
            curCase.g = parent.g + 1;
        }
        curCase.h = (int) Mathf.Pow(Vector2Int.Distance(position, finalPos),2);
        curCase.f = curCase.g + curCase.h;
        return curCase;
    }


}

[System.Serializable]
public class Case
{
    public Vector2Int position;
    public Case parent;
    public int f;
    public int g;
    public int h;
}

