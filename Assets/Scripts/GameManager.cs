using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public Camera MainCamera;

	internal Card CardSelected;
	internal Player PlayerSelected;
	internal Character TargetSelected;
	private bool SelectingTarget;

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
		if (PlayerSelected != null && selected == PlayerSelected)
			PlayerSelected.Unselect();
		PlayerSelected = selected;
		PlayerSelected.Select();
		Debug.Log("Player Selected " + PlayerSelected.name);
		if (CardSelected != null)
			BeginSelectionTarget();
	}

	public void SelectCard(Card selected)
	{
		if (CardSelected != null && selected != CardSelected)
			CardSelected.Unselect();
		CardSelected = selected;
		CardSelected.Select();
		Debug.Log("Card Selected " + CardSelected.name);

		if (PlayerSelected != null)
			BeginSelectionTarget();
	}

	internal void EndPlayerTurn()
	{
		if (CardSelected != null) {
			CardSelected.Unselect();
			CardSelected = null;
		}
	}

	internal void StartPlayerTurn()
	{

	}

	private void BeginSelectionTarget()
	{
		if (CardSelected.data.Cost <= PlayerSelected.actionLeft)
			//TODO SHOW POSSIBLE Targets
			SelectingTarget = true;
	}

	private void EndSelectionTarget()
	{
		//TODO HIDE POSSIBLE Targets
		SelectingTarget = false;
	}
}
