﻿using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

	public Button buttonHand;

	public TextMeshProUGUI DiscardCount;
	public TextMeshProUGUI HandCount;
	public TextMeshProUGUI DrawCount;

    [FMODUnity.EventRef]
    public string drawSound;
    [FMODUnity.EventRef]
    public string shuffleSound;

    private Card[] InitialCards
	{
		get {
			CardData[] cardDatas = data.InitialCard;
			int nb = cardDatas.Length;
			Card[] cards = new Card[nb];
			for (int i = 0; i < nb; i++) {
				Card card = Instantiate(data.CardPrefab, transform).GetComponent<Card>();
				card.Init(cardDatas[i],true);
				card.RecTransform.anchoredPosition = new Vector2(0, 0);
				card.gameObject.SetActive(false);
				cards[i] = card;
			}
			return cards;
		}
	}

	internal bool isShuffling;

	/// <summary>
	/// NB of card draw each turn
	/// </summary>
	internal int DrawCardAtStartTurn;

	private List<Card> deck;
	private List<Card> hand;
	private List<Card> discard;

	public bool isDrawing = false;
	private int nbCardToDraw = 0;
	private float lastDrawCard = 0;
	internal bool MouseOver;
	#endregion

	#region Initialise
	void Start()
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
		if(!GameManager.Instance.isTuto) //Don't shuffle at start for tuto
			ShuffleDeck();
		UpdateShowCount();
	}
	#endregion

	private void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown("p")) {
			Draw(DrawCardAtStartTurn);
		}
#endif       
		//DRAW ALL CARD WITH ANIMATION
		if (isDrawing && !isShuffling) {
			lastDrawCard += Time.deltaTime;
			if (lastDrawCard >= data.DrawOneCardTime) {
				lastDrawCard = 0;
				DrawCoroutine();
			}
		}

		if (isShuffling) {
			lastDrawCard += Time.deltaTime;
			if (lastDrawCard >= data.DrawOneCardTime / 2f) {
				lastDrawCard = 0;
				ShuffleCoroutine();
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
		if (nbCardToDraw <= 0)
			isDrawing = false;
	}

	private void DrawOneCard()
	{
		if (deck.Count <= 0) {
			ShuffleDiscardStackInDeckStack();
			return;
		}

        //Draw Card
        FMODUnity.RuntimeManager.PlayOneShot(drawSound);
        nbCardToDraw--;
		var card = deck[0];
		card.Unselect();
		card.transform.SetAsLastSibling();
		card.Interactable = true;
		card.Return(true);
		card.gameObject.SetActive(true);
		UpdateShowCount();
        card.RecTransform.anchoredPosition = DrawStack.anchoredPosition;
		if (hand.Count >= data.MaxCardInHand) {
			DiscardCard(card);
		} else {
			deck.RemoveAt(0);
			hand.Add(card);
			OrganizeHand();
		}
	}

	private void ShuffleDiscardStackInDeckStack()
	{
		isShuffling = true;
	}

	private void ShuffleCoroutine()
	{
		if (discard.Count == 0) return;

		var card = discard[0];
		discard.RemoveAt(0);
		card.transform.SetAsLastSibling();
		card.Return(false);
		card.Show();
		card.RecTransform.DOAnchorPos(DrawStack.anchoredPosition, data.DrawOneCardTime * 3);
		card.Disapear(data.DrawOneCardTime * 3);
		deck.Add(card);
		UpdateShowCount();
		if (discard.Count == 0) {
			Invoke("EndShuffle", data.DrawOneCardTime * 3);
		}
	}

	private void EndShuffle()
	{
		isShuffling = false;
		UpdateShowCount();
		ShuffleDeck();
	}

	private void ShuffleDeck()
	{
        FMODUnity.RuntimeManager.PlayOneShot(shuffleSound);
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
		foreach (var item in hand) {
			item.Interactable = false;
		}
	}

	public void StartPlayerTurn()
	{
		Draw(DrawCardAtStartTurn);
		foreach (var item in hand) {
			item.Interactable = true;
		}
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
	public void DiscardCard(Card card)
	{
		deck.Remove(card);
		hand.Remove(card);
		discard.Add(card);
		card.RecTransform.DOAnchorPos(DiscardStack.anchoredPosition, data.DrawOneCardTime * 2);
		card.Discard(data.DrawOneCardTime * 2);

		OrganizeHand();
		UpdateShowCount();
		if (GameManager.Instance.isTuto)
		{
			TutoManager.Instance.Next();
			DrawOneCard();
		}

		///Show player movement again

		//if(GameManager.Instance.CardSelected != null)
		//{
		//	GameManager.Instance.CardSelected = null;
		//}
		//if(GameManager.Instance.PlayerSelected != null)
		//{
		//	MapManager.Instance.GenerateCaseMap(GameManager.Instance.PlayerSelected.gameObject, GameManager.Instance.PlayerSelected.actionLeft);
		//}
	}
	#endregion

	private void UpdateShowCount()
	{
		HandCount.text = hand.Count + " / " + data.MaxCardInHand;
		DrawCount.text = deck.Count.ToString();
		DiscardCount.text = discard.Count.ToString();
	}
}
