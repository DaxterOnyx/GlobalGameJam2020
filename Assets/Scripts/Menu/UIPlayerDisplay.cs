using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerDisplay : MonoBehaviour
{
	public PlayerData data;

	public Image ImageDisplay;
	public TextMeshProUGUI NameDisplay;
	public TextMeshProUGUI ActionDisplay;
	public TextMeshProUGUI LifeDisplay;
	public TextMeshProUGUI StrenghDisplay;
	public TextMeshProUGUI GunFireDisplay;
	public TextMeshProUGUI CardListDisplay;

	public GameObject selecter;

	internal bool isSelected = false;

	// Start is called before the first frame update
	void Start()
	{
		if (data == null) {
			Debug.LogError("WHERE IS MY DATA, WHO AM I ?");
			return;
		}

		NameDisplay.text = data.name;
		LifeDisplay.text = data.nbMaxLP.ToString();
		ActionDisplay.text = data.nbActionPoint.ToString();
		StrenghDisplay.text = data.Strengh.ToString();
		GunFireDisplay.text = data.FireGunDamage.ToString();

		string cards = "";
		foreach (CardCounter cardCounter in data.CardAddinTeam) {
			cards += cardCounter.card.Name + " x " + cardCounter.count + "\n";
		}
		CardListDisplay.text = cards;

		//TODO Display card on hover name of card

		selecter.SetActive(false);
	}

	public void OnClick()
	{
		isSelected = !isSelected;
		if (isSelected) {
			selecter.SetActive(true);

			Debug.Log(data.CardAddinTeam.Length);
			UIDeckManager.Instance.AddCard(data.CardAddinTeam);
		}
		else {
			selecter.SetActive(false);
			Debug.Log(data.CardAddinTeam.Length);
			UIDeckManager.Instance.RemoveCard(data.CardAddinTeam);
		}
	}
}
