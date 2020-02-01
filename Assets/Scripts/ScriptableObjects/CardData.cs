using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/Card")]
public class CardData : ScriptableObject
{
	public string Name;
	public Sprite Image;
	public string Description;
	public int Cost;
	public CardAction Action;
	public int Range;
	public TargetType[] targetTypes;

	public enum CardAction
	{
		Attack,
		Repair,
		CaC,
		Armor,
		Heal
	}

	public enum TargetType
	{
		Player,
		Himself,
		Monster,
		Object
	}
}
