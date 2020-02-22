using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterActions : MonoBehaviour
{
    #region Fields
    private Monster monster;
    private Vector2Int targetPos;
    private Token target;
    private Sequence sequence;
    private int atkCount;
    private List<Vector2Int> hitList;
    public float atkTime;
    private float timer;
    private void Start()
    {
        monster = this.GetComponent<Monster>();
        targetPos = new Vector2Int();
        hitList = new List<Vector2Int>();
        /*foreach (AnimationClip clip in monster.animator.runtimeAnimatorController.animationClips)
        {
            if(clip.name == "_Punch")
            {
                atkTime = clip.length + 0.2f;
            }
        }*/
    }
    #endregion

    void Update()
    {
        if (sequence != null)
        {
            //Hit the player only when in contact
            if (sequence.Elapsed() >= sequence.Duration())
            {
                if(timer <= 0)
                {
                    if (hitList.Count > 0)
                    {
                        target = MapManager.Instance.TryGetTokenByPos(hitList[0]);
                        if (target != null)
                        {
                            monster.Punch();
                            monster.LookAt(target);

                            target.TakeDamage(monster.data.Strengh);
                            GameManager.Instance.IsGameLost();
                        }
                        hitList.RemoveAt(0);
                        timer = atkTime;
                    }
                    else
                        EndAction();
                }
                else
                    timer -= Time.deltaTime;

            }
        }
    }

    public void StartAction()
    {
        if(monster.data.target == MonsterData.Target.Objective)
        {
            MapManager.Instance.WhereIsToken(StructuresManager.Instance.NearestObjective(monster), out targetPos);
            sequence = DefineActions(targetPos);
        }
        else
        {
            MapManager.Instance.WhereIsToken(PlayersManager.Instance.Nearest(monster), out targetPos);
            sequence = DefineActions(targetPos);
        }
    }

    public void EndAction()
    {
        timer = 0;
        sequence = null;
        targetPos = new Vector2Int();
        target = null;
        hitList = new List<Vector2Int>();
        MonstersManager.Instance.NextMonsterAction();
    }


    /// <summary>
    /// Will create the DOTween sequence 
    /// of the monster
    /// </summary>
    /// <param name="destination">The monster's target (player or objective)</param>
    /// <returns></returns>
    private Sequence DefineActions(Vector2Int destination)
    {
        if(destination == new Vector2Int())
            return null;
        int moveCount = monster.data.nbActionPoint;
        List<Vector2Int> pathComplete = Pathfinding.Instance.findPath(MapManager.Instance.V3toV2I(monster.transform.position), destination);
        List<Vector2Int> finalpath = new List<Vector2Int>();
        int lifePointTarget = (MapManager.Instance.TryGetTokenByPos(destination) as Character).GetCurrentLp();

        ///Movement calculation


        for (int i = 0; i <= Mathf.Min(moveCount * 2, pathComplete.Count - 1); i++)
        {
            if (i != pathComplete.Count - 1) //Remove last case when get to player
            {
                finalpath.Add(pathComplete[i]);
            }

        }
        Sequence sequence = MapManager.Instance.Move(monster, finalpath);

        ///Attack claculation

        moveCount -= Mathf.FloorToInt((pathComplete.Count - 2) / 2); // - 2 : fisrt and last case (initial case and player case)
        while (moveCount > 0 && lifePointTarget > 0)
        {
            hitList.Add(destination);
            moveCount--;
            lifePointTarget -= monster.data.Strengh;
        }
        return sequence;
    }
}
