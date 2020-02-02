using System;
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
	void Update()
	{
		if (Input.GetMouseButtonDown(0) && SelectingTarget) {
			//Ask at map map manager the object clicked
			var target = MapManager.Instance.TryGetObjectByPos(MapManager.Instance.V3toV2I(MainCamera.ScreenToWorldPoint(Input.mousePosition)));
			//check if is a target of the card and in area of card
			if (target != null && CardSelected.IsValableTarget(PlayerSelected, target)) {

				CardSelected.Action(PlayerSelected, TargetSelected);
				UnSelectTarget();
			}
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
			SelectTarget();
	}

	public void SelectCard(Card selected)
	{
		if (CardSelected != null && selected != CardSelected)
			CardSelected.Unselect();
		CardSelected = selected;
		CardSelected.Select();
		Debug.Log("Card Selected " + CardSelected.name);

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
