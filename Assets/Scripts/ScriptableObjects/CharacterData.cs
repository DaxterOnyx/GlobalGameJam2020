using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CharacterData : ScriptableObject
{
    public int nbMaxLP;
    public int nbActionPoint;
}

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerData : CharacterData
{
	internal int Strengh;
	internal int FireGunDamage;
}

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/MonsterData")]
public class MonsterData : CharacterData
{
    public target target;
}

[CreateAssetMenu(fileName = "ObjectData", menuName = "Data/ObjectData")]
public class ObjectData : CharacterData
{
    public int repairCount;
    public bool isObjective;
}

public enum target { Player, Objective };