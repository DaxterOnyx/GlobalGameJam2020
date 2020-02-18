using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Token/PlayerData")]
public class PlayerData : CharacterData
{
	public int Strengh;
    public int FireGunDamage;
	public int Defence;
	public int Repair;
	//TODO ADD PROFILE IMAGE
	public CardCounter[] CardAddinTeam;
}
