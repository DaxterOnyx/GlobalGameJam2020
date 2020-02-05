using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/Token/MonsterData")]
public class MonsterData : CharacterData
{
    public int Strengh;
    public Target target;
public enum Target { Player, Objective };
}