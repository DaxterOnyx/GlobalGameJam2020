using UnityEngine;

public class Card : MonoBehaviour
{
	public CardData data;
	public SpriteRenderer ImageRenderer;
	private void Start()
	{
		ImageRenderer.sprite = data.Image;
	}

	private void Update()
	{
		
	}
}