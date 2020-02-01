using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/Card")]
public class CardData : ScriptableObject
{
	public string Name;
	public Sprite Image;
	public string Description;
	public int Cost;
}