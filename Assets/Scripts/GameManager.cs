using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public Camera MainCamera;

	internal Card CardSelected;
	internal Player PlayerSelected;
	internal Character TargetSelected;
	private bool SelectingTarget;
	public GameObject winText;
	// Start is called before the first frame update
	void Start()
	{
		Instance = this;
	}

	// Update is called once per frame
	public void SelectTarget(Character target)
	{
		if (SelectingTarget &&
			//check if is a target of the card and in area of card
			CardSelected.IsValableTarget(PlayerSelected, target)) {
			TargetSelected = target;
			CardSelected.ActionCard(PlayerSelected, TargetSelected);
			EndSelectionTarget();
		} else {
			// Select target is called when you click on a Character
			if (target is Player)
				SelectPlayer(target as Player);
		}
	}

	public void SelectPlayer(Player selected)
	{
		if (PlayerSelected != null && selected == PlayerSelected) {
			PlayerSelected.Unselect();
			PlayersManager.Instance.DelightTargets();
			PlayerSelected = null;
			if (CardSelected != null) {
				EndSelectionTarget();
				PlayersManager.Instance.HighlightPlayers(CardSelected.data.Cost);
			}

		} else {
			if (PlayerSelected != null) {
				PlayerSelected.Unselect();
			}
			PlayersManager.Instance.DelightTargets();
			PlayerSelected = selected;
			PlayerSelected.Select();
			Debug.Log("Player Selected " + PlayerSelected.name);
			if (CardSelected != null)
				BeginSelectionTarget();
		}

	}

	public void SelectCard(Card selected)
	{
		if (CardSelected != null) {
			//have previous card
			if (PlayerSelected != null) {
				//hide previous target of previous card
				PlayersManager.Instance.DelightTargets();
			}
			if (selected == CardSelected) {
				//unselect Actual Card
				CardSelected.Unselect();
				CardSelected = null;
				if (PlayerSelected != null) {
					//player can move
					MapManager.Instance.GenerateCaseMap(PlayerSelected.gameObject, PlayerSelected.actionLeft);
				}
				// cant select target without card selected			
				EndSelectionTarget();
				return;
			} else {
				//Select another card
				CardSelected.Unselect();
			}
		}

		//Select the card
		CardSelected = selected;
		CardSelected.Select();
		Debug.Log("Card Selected " + CardSelected.name);

		if (PlayerSelected == null) {
			PlayersManager.Instance.HighlightPlayers(CardSelected.data.Cost);
		} else {
			MapManager.Instance.DestroyCaseMap();
			BeginSelectionTarget();
		}
	}

	internal void EndPlayerTurn()
	{
		if (CardSelected != null) {
			CardSelected.Unselect();
			CardSelected = null;
		}
		PlayerSelected = null;
		TargetSelected = null;
	}

	public bool IsGameWin()
	{
		if (ObjectsManager.Instance.HowManyObjectivesLeft() == 0) {
			Debug.Log("Tu as Gagné!");
			winText.SetActive(true);
			return true;
		}
		return false;
	}

	public bool IsGameLost()
	{
		if (PlayersManager.Instance.PlayersLeft() == 0) {
			SceneManager.LoadScene("GameOver");
			return true;
		}
		return false;
	}

	private void BeginSelectionTarget()
	{
		if (CardSelected.data.Cost <= PlayerSelected.actionLeft) {
			if (CardSelected.data.Cost <= PlayerSelected.actionLeft) {
				//TODO SHOW POSSIBLE Targets

				//Hide deplacement Case
				MapManager.Instance.DestroyCaseMap();

				SelectingTarget = true;
				//
				foreach (var item in CardSelected.data.targetTypes) {
					switch (item) {
						case CardData.TargetType.Player:
							PlayersManager.Instance.HighlightTargets(PlayerSelected.gameObject, CardSelected.data.Range);
							break;
						case CardData.TargetType.Monster:
							MonstersManager.Instance.HighlightTargets(PlayerSelected.gameObject, CardSelected.data.Range);
							break;
						case CardData.TargetType.Object:
							ObjectsManager.Instance.HighlightTargets(PlayerSelected.gameObject, CardSelected.data.Range);
							break;
						case CardData.TargetType.Objective:
							ObjectsManager.Instance.HighlightObjectives(PlayerSelected.gameObject, CardSelected.data.Range);
							break;
						case CardData.TargetType.Himself:
							EndSelectionTarget();
							CardSelected.ActionCard(PlayerSelected, PlayerSelected);
							break;
						default:
							Debug.LogError("target type not defined");
							break;
					}
				}
			}

		}
	}

	private void EndSelectionTarget()
	{
		//TODO HIDE POSSIBLE Targets
		SelectingTarget = false;
		PlayersManager.Instance.DelightTargets();
		MonstersManager.Instance.DelightTargets();
		ObjectsManager.Instance.DelightTargets();
	}
}
