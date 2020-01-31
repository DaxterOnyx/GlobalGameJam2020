using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
	public CardData data;
	public Image Image;
	public Text Name;
	public Text Cost;
	public Text Description;
	private void Start()
	{
		Image.sprite = data.Image;
		Name.text = data.Name;
		Cost.text = data.Cost.ToString();
		Description.text = data.Description;
	}

	private void Update()
	{
		
	}
}