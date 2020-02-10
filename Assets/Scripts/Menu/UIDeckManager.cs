using System.Collections.Generic;
using UnityEngine;

public class UIDeckManager : MonoBehaviour
{
	private static UIDeckManager _instance;
	public static UIDeckManager Instance
	{
		get {
			if (_instance == null)
				_instance = FindObjectOfType<UIDeckManager>();
			return _instance;
		}
	}

	public GameObject CardPrefab;

	[SerializeField]
	public List<CardCounter> deck;

	public

	// Start is called before the first frame update
	void Start()
	{
		_instance = this;

		List<Card> cards = new List<Card>();
		foreach (CardCounter cardCounter in deck) {
			for (int i = 0; i < cardCounter.count; i++) {
				var go = Instantiate(CardPrefab);
				var card = go.GetComponent<Card>();
				card.Init(cardCounter.card);
				cards.Add(card);
			}
		}
		//TODO SET AS A GOOD PLACE
	}

	// Update is called once per frame
	void Update()
	{

	}
}
