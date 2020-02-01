using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CharacterData : ScriptableObject
{
    public int nbMaxLP;
}

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerData : CharacterData
{
    public int nbActionPoint;
}

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/MonsterData")]
public class MonsterData : CharacterData
{

}