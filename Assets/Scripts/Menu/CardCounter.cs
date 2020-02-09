[System.Serializable]
public struct CardCounter
{
	public CardData card;
	public int count;

	public CardCounter(CardData card, int count)
	{
		this.card = card;
		this.count = count;
	}
}