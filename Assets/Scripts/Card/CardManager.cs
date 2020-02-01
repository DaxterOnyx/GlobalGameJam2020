using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;

public class CardManager : MonoBehaviour
{
	private static CardManager _instance;
	public static CardManager Instance { get { return _instance; } }

	#region Fields
	public ManagerCardData data;

	[Header("UI Variables")]
	public RectTransform DiscardStack;
	public RectTransform HandStack;
	public RectTransform DrawStack;

	public TextMeshProUGUI DiscardCount;
	public TextMeshProUGUI HandCount;
	public TextMeshProUGUI DrawCount;

	private Card[] InitialCards
	{
		get {
			//TODO ADD CARDS OF PLAYERS 
			CardData[] cardDatas = data.InitialCard;
			int nb = cardDatas.Length;
			Card[] cards = new Card[nb];
			for (int i = 0; i < nb; i++) {
				Card card = Instantiate(data.CardPrefab, transform).GetComponent<Card>();
				card.Init(cardDatas[i]);
				card.RecTransform.anchoredPosition = new Vector2(0, 0);
				card.gameObject.SetActive(false);
				cards[i] = card;
			}
			return cards;
		}
	}

	public bool isShufflingi;

	/// <summary>
	/// NB of card draw each turn
	/// </summary>
	internal int DrawCardAtStartTurn;

	private List<Card> deck;
	private List<Card> hand;
	private List<Card> discard;

	private bool isDrawing = false;
	private int nbCardToDraw = 0;
	private float lastDrawCard = 0;
	#endregion

	#region Initialise
	void Awake()
	{
		_instance = this;
		InvokeDeck();
	}

	private void InvokeDeck()
	{
		deck = new List<Card>(InitialCards);
		hand = new List<Card>();
		discard = new List<Card>();
		DrawCardAtStartTurn = data.DrawCardAtStartTurn;
		ShuffleDeck();
		UpdateShowCount();
	}
	#endregion

	private void Update()
	{
		//TODO REMOVE DEBUG 
		if (Input.GetKeyDown(KeyCode.Space)) {
			Draw(DrawCardAtStartTurn);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
			Debug.Break();

		//DRAW ALL CARD WITH ANIMATION
		if (isDrawing &&  !isShufflingi) {
			lastDrawCard += Time.deltaTime;
			if (lastDrawCard >= data.DrawOneCardTime) {
				lastDrawCard = 0;
				DrawCoroutine();
			}
		}

	}

	#region Draw Gestion
	private void Draw(int nbCard)
	{
		Debug.Log("Draw " + nbCard + " cards");

		if (nbCard <= 0) {
			Debug.LogWarning("None card to draw.");
			return;
		}
		isDrawing = true;
		nbCardToDraw += nbCard;
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
			DiscardAtDeck();
		}

		//Draw Card
		var card = deck[0];
		card.transform.SetAsLastSibling();
		card.gameObject.SetActive(true);
		deck.RemoveAt(0);
		UpdateShowCount();
		card.RecTransform.anchoredPosition = DrawStack.anchoredPosition;
		if (hand.Count >= data.MaxCardInHand) {
			DiscardCard(card);
		} else {
			hand.Add(card);
			OrganizeHand();
		}
	}

	private void DiscardAtDeck()
	{
		isShufflingi = true;
		Invoke("EndShuffle", data.DrawOneCardTime*3);
	}

	private void EndShuffle()
	{
		isShufflingi = false;
		deck.AddRange(discard);
		UpdateShowCount();
		discard.Clear();
		ShuffleDeck();
	}

	private void ShuffleDeck()
	{
		var newDeck = new List<Card>();
		while (deck.Count > 0) {
			int index = Random.Range(0, deck.Count);
			newDeck.Add(deck[index]);
			deck.RemoveAt(index);
		}
		deck = newDeck;
	}
	#endregion

	#region Hand Gestion
	internal int IndexInHand(Card card)
	{
		return hand.IndexOf(card);
	}

	public Vector2 PositionInHand(int index)
	{
		int count = hand.Count;
		float cardSize = data.CardSize;
		float delta = cardSize;
		float HandSize = HandStack.rect.width;
		if (count * cardSize > HandSize)
			delta = HandSize / count;

		float originPosition = HandStack.anchoredPosition.x;
		return new Vector2(originPosition + delta * (index - ((count - 1) / 2f)), HandStack.anchoredPosition.y);
	}

	public void EndPlayerTurn()
	{
	}

	public void StartPlayerTurn()
	{
		Draw(DrawCardAtStartTurn);
	}
	public void OrganizeHand()
	{
		UpdateShowCount();
		for (int i = 0; i < hand.Count; i++) {
			Vector2 nextPos = PositionInHand(i);
			hand[i].RecTransform.DOAnchorPos(nextPos, data.DrawOneCardTime);
		}
	}
	#endregion

	#region Discard Gestion
	private void DiscardCard(Card card)
	{
		discard.Add(card);
		card.RecTransform.DOAnchorPos(DiscardStack.anchoredPosition, data.DrawOneCardTime * 2);
		card.Discard(data.DrawOneCardTime * 2);
		UpdateShowCount();
	}
	#endregion

	private void UpdateShowCount()
	{
		HandCount.text = hand.Count + " / " + data.MaxCardInHand;
		DrawCount.text = deck.Count.ToString();
		DiscardCount.text = discard.Count.ToString();
	}
}
