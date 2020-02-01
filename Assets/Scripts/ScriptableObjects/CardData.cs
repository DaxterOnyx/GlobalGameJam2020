using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/Card")]
public class CardData : ScriptableObject
{
	public string Name;
	public Sprite Image;
	public string Description;
	public int Cost;
	public CardAction Action;
	public string Range;

	public enum CardAction
	{
		Attack,
		Repair
	}
}
