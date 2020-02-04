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
			if (target is Player)
				SelectPlayer(target as Player);
		}
	}

	public void SelectPlayer(Player selected)
	{
		if (PlayerSelected != null && selected != PlayerSelected)
			PlayerSelected.Unselect();
		PlayerSelected = selected;
		PlayerSelected.Select();
		Debug.Log("Player Selected " + PlayerSelected.name);
		if (CardSelected != null)
			BeginSelectionTarget();
	}

	public void SelectCard(Card selected)
	{
		if (CardSelected != null) {
			if (CardSelected == selected) {
				CardSelected.Unselect();
				PlayerSelected.Select();
				return;
			}
			else
				CardSelected.Unselect();
		}
		CardSelected = selected;
		CardSelected.Select();
		Debug.Log("Card Selected " + CardSelected.name);

		if (PlayerSelected != null)
		{
			BeginSelectionTarget();
		}
			
	}

	internal void EndPlayerTurn()
	{
		if (CardSelected != null) {
			CardSelected.Unselect();
			CardSelected = null;
		}
	}

	public bool IsGameWin()
	{
		if (ObjectsManager.Instance.HowManyObjectivesLeft() == 0)
		{
			Debug.Log("Tu as Gagné!");
			winText.SetActive(true);
			return true;
		}
		return false;
	}
	
	public bool IsGameLost()
	{
		if (PlayersManager.Instance.PlayersLeft() == 0)
		{
			SceneManager.LoadScene("GameOver");
			return true;
		}
		return false;
	}

	private void BeginSelectionTarget()
	{
		if (CardSelected.data.Cost <= PlayerSelected.actionLeft) {
			//TODO SHOW POSSIBLE Targets

			//Hide deplacement Case
			MapManager.Instance.DestroyCaseMap();

			SelectingTarget = true;
		}
	}

	private void EndSelectionTarget()
	{
		//TODO HIDE POSSIBLE Targets
		SelectingTarget = false;
	}
}
