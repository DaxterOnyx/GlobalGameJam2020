using System.Collections.Generic;
using TMPro;
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
	public GameObject CountPrefab;
	public Vector3 CardOffset;
	public Vector3 CountOffset;

	[SerializeField]
	public List<CardCounter> deck;

	private List<Card> cards = new List<Card>();
	private List<int> counts = new List<int>();
	private List<TextMeshProUGUI> countsDisplay = new List<TextMeshProUGUI>();

	// Start is called before the first frame update
	void Start()
	{
		_instance = this;
		InitCards();
		ShowCards();
	}

	private void ShowCards()
	{
		//TODO SET AS A GOOD PLACE
		int count = cards.Count;
		for (int i = 0; i < count; i++) {
			Vector3 pos = CardPosition(i);
			cards[i].transform.position = pos+CardOffset;
			if (countsDisplay.Count < i) {
				//TODO add TextMeshProUGUI
				Debug.LogError("i dont know");
			}
			countsDisplay[i].text = "x" + counts[i];
			countsDisplay[i].transform.position = pos+CountOffset;
		}
	}

	private void InitCards()
	{
		//TODO IS SAD
		foreach (var item in countsDisplay) {
			Destroy(item.gameObject);
		}
		countsDisplay.Clear();
		counts.Clear();

		foreach (var item in cards) {
			Destroy(item.gameObject);
		}
		cards.Clear();

		foreach (CardCounter cardCounter in deck) {
			if (cardCounter.count == 0)
				continue;
			var goCard = Instantiate(CardPrefab, transform);
			var card = goCard.GetComponent<Card>();
			card.Init(cardCounter.card, false);
			cards.Add(card);
			var goCount = Instantiate(CountPrefab, transform);
			var count = goCount.GetComponent<TextMeshProUGUI>();
			count.text = "x" + cardCounter.count.ToString();
			counts.Add(cardCounter.count);
			countsDisplay.Add(count);
		}
	}

	private Vector3 CardPosition(int index)
	{
		int count = cards.Count;
		float cardSize = cards[index].CardSize;
		float delta = cardSize;

		float originPosition = transform.position.x;
		return new Vector2(originPosition + delta * (index - ((count - 1) / 2f)), transform.position.y);
	}

	public void AddCard(CardCounter[] newCards)
	{
		bool initCards = false;
		foreach (var item in newCards) {
			var founded = false;
			for (int i = 0; i < deck.Count && !founded; i++) {
				if (item.card.Name == deck[i].card.Name) {
					deck[i].count += item.count;
					founded = true;
					break;
				}
			}
			if (!founded) {
				initCards = true;
				deck.Add(new CardCounter(item.card,item.count));
			}
		}
		if (initCards)
			InitCards();
		ShowCards();
	}

	public void RemoveCard(CardCounter[] oldCards)
	{
		bool initCards = false;
		foreach (var item in oldCards) {
			var founded = false;
			var last = false;
			var index = -1;
			for (int i = 0; i < deck.Count && !founded; i++) {
				if (item.card.Name == deck[i].card.Name) {
					deck[i].count -= item.count;
					founded = true;
					last = deck[i].count <= 0;
					index = i;
					break;
				}
			}
			if (!founded) {
				Debug.LogError("Card not found.");
			}
			if (last) {
				deck.RemoveAt(index);
				initCards = true;
			}

		}
		if (initCards)
			InitCards();
		ShowCards();
	}
}
