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

	// Start is called before the first frame update
	void Start()
	{
		if (data == null) {
			Debug.LogError("WHERE IS MY DATA, WHO AM I ?");
			return;
		}

		//TODO PLAYER IMAGE DISPLAY
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

		//TODO Dsiplay card on hover name of card
	}

	// Update is called once per frame
	void Update()
	{

	}
}
