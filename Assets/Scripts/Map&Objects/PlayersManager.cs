public class PlayersManager : Location
{
	private static PlayersManager _instance;
	public static PlayersManager Instance
	{
		get {
			if (_instance == null)
				_instance = FindObjectOfType<PlayersManager>();
			return _instance;
		}
	}

	public PlayerManagerData data;

	void Start()
	{
		_instance = this;
		Initialize(data);
	}

	internal void StartPlayerTurn()
	{
		GameManager.Instance.IsGameLost();
		foreach (var item in objectList) {
			(item as Player).ResetActionPoint();
		}
	}

	public int PlayersLeft()
	{
		return objectList.Count;
	}

	public void HighlightPlayers(int cost)
	{
		foreach (var item in objectList)
		{
			if ((item as Player).actionLeft >= cost)
			{
				(item as Player).Highlight(false);
			}
		}
	}
}
