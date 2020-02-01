using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
	public CardData data;
	public RectTransform RecTransform;
	public Image Image;
	public Text Name;
	public Text Cost;
	public Text Description;

	private void Start()
	{
		if (data != null)
			Init(data);
	}
	public void Init(CardData data)
	{
		this.data = data;
		gameObject.name = data.Name;
		Image.sprite = data.Image;
		Name.text = data.Name;
		Cost.text = data.Cost.ToString();
		Description.text = data.Description;
	}

	private void Update()
	{
		
	}
}