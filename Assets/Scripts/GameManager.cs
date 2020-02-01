using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public Card CardSelected;
	public Player PlayerSelected;
	public Location TargetSelected;
	private bool SelectingTarget;

	// Start is called before the first frame update
	void Start()
	{
		Instance = this;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0) && SelectingTarget) {
			//TODO ASK TO MAP MANAGER THE OBJECT CLIKED
			//TODO CHECK IF IS A TARGET OF THE CARD AND IN AREA OF CARD
			CardSelected.Action(PlayerSelected, TargetSelected);
			UnSelectTarget();
		}
	}

	public void SelectPlayer(Player selected)
	{
		if (PlayerSelected != null && selected != PlayerSelected)
			PlayerSelected.Unselect();
		PlayerSelected = selected;

		if (CardSelected != null)
			SelectTarget();
	}

	public void SelectCard(Card selected)
	{
		if (CardSelected != null && selected != CardSelected)
			CardSelected.Unselect();
		CardSelected = selected;
		if (PlayerSelected != null)
			SelectTarget();
	}

	private void SelectTarget()
	{
		//TODO SHOW POSSIBLE Targets
		SelectingTarget = true;
		throw new NotImplementedException();
	}

	private void UnSelectTarget()
	{
		//TODO HIDE POSSIBLE Targets
		SelectingTarget = false;
		throw new NotImplementedException();
	}
}
