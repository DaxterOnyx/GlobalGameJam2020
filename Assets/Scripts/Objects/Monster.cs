using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    public MonsterData data;
    protected override void Start()
    {
        LifePoint = data.nbMaxLP;
    }
}
