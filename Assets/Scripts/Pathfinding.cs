using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private static Pathfinding _instance;
    public static Pathfinding Instance { get { return _instance; } }
    private List<Case> openList;
    private List<Case> closedList;
    private List<Vector2Int> path;
    private CaseComp CaseC;

    private void Start()
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
        iCase = recPathfinding(finalPos);
        if(iCase == null)
        {
            Debug.LogError("Error: No path found!");
        }
        else
        {
            FulfillPath(iCase);
        }
        return path;
    }

    public void FulfillPath(Case curCase)
    {
        if(curCase.parent == null)
        {
            path.Add(curCase.position);
        }
        else
        {
            FulfillPath(curCase.parent);
            path.Add(curCase.position);
        }
    }

    public Case recPathfinding(Vector2Int finalPos)
    {
        openList.Sort(CaseC);
        Case curCase = openList[0];
        // Calculationg for neighbor cases
        if (ListContainsNodeAt(closedList, finalPos, out curCase))
        {
            return curCase;
        }
        else if (openList.Count == 0)
        {
            return null;
        }
        calculatingCase(curCase.position + new Vector2Int(0, 1), curCase, finalPos);
        calculatingCase(curCase.position + new Vector2Int(1, 0), curCase, finalPos);
        calculatingCase(curCase.position + new Vector2Int(0, -1), curCase, finalPos);
        calculatingCase(curCase.position + new Vector2Int(-1, 0), curCase, finalPos);
        return recPathfinding(finalPos);
    }

    public void calculatingCase (Vector2Int pos,Case parent,Vector2Int finalPos)
    {
        Case curCase;
        if(!(MapManager.Instance.CaseTaken(pos) || ListContainsNodeAt(closedList, pos)))
        {
            if (!ListContainsNodeAt(openList, pos,out curCase))
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

    public bool ListContainsNodeAt(List<Case> lCase,Vector2Int pos,out Case pCase)
    {
        foreach (var item in lCase)
        {
            if(item.position == pos)
            {
                pCase = item;
                return true;
            }
        }
        pCase = null;
        return false;
    }

    public void ChangeParent(Case curCase,Case parentCase)
    {
        curCase.parent = parentCase;
        curCase.g = parentCase.g + 1;
    }

    public Case CreateCase(Vector2Int position,Vector2Int finalPos,Case parent)
    {
        Case curCase = new Case();
        curCase.position = position;
        curCase.parent = parent;
        curCase.g = parent.g + 1;
        curCase.h =(int) Vector2Int.Distance(position,finalPos);
        curCase.f = curCase.g + curCase.h;
        return curCase;
    }
    
    
}

public class Case
{
    public Vector2Int position;
    public Case parent;
    public int f;
    public int g;
    public int h;
}
public class CaseComp : IComparer<Case>
{
    public int Compare (Case c1, Case c2)
    {
        return c1.f - c2.f;
    }
}

