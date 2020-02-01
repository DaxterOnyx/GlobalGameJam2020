using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Card : MonoBehaviour
{
	public CardData data;
	public RectTransform RecTransform;
	public Image Image;
	public TextMeshProUGUI Name;
	public TextMeshProUGUI Cost;
	public TextMeshProUGUI Description;
	public TextMeshProUGUI Range;

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
		Range.text = data.Range;
	}

	public void OnClick()
	{
		//TODO ACTION
		switch (data.Action) {
			case CardData.CardAction.Attack:
			case CardData.CardAction.Repair:
			default:
				Debug.LogError("Not Defined Action : " + data.Action.ToString());
				break;
		}
	}

	public void SetLastSibling()
	{
		Debug.Log("SetLastSibling");
		RecTransform.SetAsLastSibling();
	}

	internal void Discard(float time)
	{
		Invoke("ReelDiscard", time);
	}

	void ReelDiscard()
	{
		gameObject.SetActive(false);
	}
}