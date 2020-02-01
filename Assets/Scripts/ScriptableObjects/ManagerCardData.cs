using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManagerCardData", menuName = "Data/ManagerCard")]
public class ManagerCardData : ScriptableObject
{
	public int MaxCardInHand;
	public int DrawCardAtStartTurn;
	public GameObject CardPrefab;
	public float DrawOneCardTime;
	public float CardSize;
	public CardData[] InitialCard;
}
