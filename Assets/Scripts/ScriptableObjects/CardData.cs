using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/Card")]
public class CardData : ScriptableObject
{
	public Sprite Image;
	public string Description;
}