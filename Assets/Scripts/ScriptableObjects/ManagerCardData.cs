using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManagerCardData", menuName = "Data/ManagerCard")]
public class ManagerCardData : ScriptableObject
{
	public int MaxCardInHand;
	public GameObject CardPrefab;

}
