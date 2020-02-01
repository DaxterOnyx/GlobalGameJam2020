using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	private static CardManager _instance;
	public static CardManager Instance { get { return _instance; } }

	public Card[] InitialCard
	{
		get {
			//TODO ADD CARDS OF PLAYERS 
			CardData[] cardDatas = data.InitialCard;
			int nb = cardDatas.Length;
			Card[] cards = new Card[nb];
			for (int i = 0; i < nb; i++) {
				Card card = Instantiate(data.CardPrefab, transform).GetComponent<Card>();
				card.Init(cardDatas[i]);
				card.RecTransform.anchoredPosition = DrawStack.anchoredPosition;
				card.gameObject.SetActive(false);
				cards[i] = card;
			}
			return cards;
		}
	}

	public ManagerCardData data;

	public RectTransform DiscardStack;
	public RectTransform HandStack;
	public RectTransform DrawStack;

	private List<Card> deck;
	private List<Card> hand;
	private List<Card> discard;

	internal int DrawCardAtStartTrun;
	private bool isDrawing = false;
	private int nbCardToDraw = 0;
	private float lastDrawCard = 0;

	void Awake()
	{
		_instance = this;
		InvokeDeck();
	}

	private void InvokeDeck()
	{
		deck = new List<Card>(InitialCard);
		hand = new List<Card>();
		discard = new List<Card>();
		DrawCardAtStartTrun = data.DrawCardAtStartTurn;
	}

	internal int IndexInHand(Card card)
	{
		return hand.IndexOf(card);
	}

	public Vector3 PositionInHand(int index)
	{
		int count = hand.Count;
		float cardSize = data.CardSize;
		float delta = cardSize;
		float HandSize = HandStack.rect.width;
		if (count * cardSize > HandSize)
			delta = HandSize / count;

		float originPosition = HandStack.anchoredPosition.x;
		return new Vector3(originPosition - delta * (index-((count-1)/2f)), HandStack.anchoredPosition.y, 0);
	}

	public void EndPlayerTurn()
	{
	}

	public void StartPlayerTurn()
	{
		Draw(DrawCardAtStartTrun);
	}

	private void Draw(int nbCard)
	{
		if (nbCard <= 0) {
			Debug.LogWarning("None card to draw.");
			return;
		}
		isDrawing = true;
		nbCardToDraw += nbCard;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space)) {
			Debug.Log("Draw 5 cards");
			Draw(5);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
			Debug.Break();

		if (isDrawing) {
			lastDrawCard += Time.deltaTime;
			if (lastDrawCard >= data.DrawOneCardTime) {
				lastDrawCard = 0;
				DrawCoroutine();
			}
		}

	}
	private void DrawCoroutine()
	{
		DrawOneCard();
		nbCardToDraw--;
		if (nbCardToDraw <= 0)
			isDrawing = false;
	}

	private void DrawOneCard()
	{
		if (deck.Count <= 0) {
			//TODO Shuffle Discard

		}

		//Draw Card
		var card = deck[0];
		card.gameObject.SetActive(true);
		deck.RemoveAt(0);
		hand.Add(card);
		OganizeHand();
	}

	public void OganizeHand()
	{
		//TODO REMOVE HARD VALUE
		for (int i = 0; i < hand.Count; i++) {
			hand[i].RecTransform.DOAnchorPos(PositionInHand(i), 0.2f);
		}
	}

	private void DiscardCard(Card card)
	{
		hand.Remove(card);
		discard.Add(card);
	}

	
}
