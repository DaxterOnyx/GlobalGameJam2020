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
	[Header("Value of the action (for defence per example)")]
	public int Value;

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
		Object,
		Objective
	}
}
