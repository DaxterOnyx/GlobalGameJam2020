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
	public RectTransform CardRef;
	public GameObject CardPrefab;

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

		List<Card> cards = new List<Card>();
		foreach (CardCounter cardCounter in data.CardAddinTeam) {
			for (int i = 0; i < cardCounter.count; i++) {
				var go = Instantiate(CardPrefab);
				var card = go.GetComponent<Card>();
				card.Init(cardCounter.card);
				cards.Add(card);
			}
		}


	}

	// Update is called once per frame
	void Update()
	{

	}
}
