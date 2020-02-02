using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
	public static TurnManager Instance { get; private set; }

	[HideInInspector]
	public int Turn = 0;
	[HideInInspector]
	public bool PlayerTurn = false;

	public TextMeshProUGUI TurnDisplay;
	public Button TurnButton;

	private void Start()
	{
		Instance = this;
	}

	public void NextTurn()
	{
		if (!PlayerTurn) {
			Turn++;
			TurnDisplay.text = "Tour " + Turn;
		}

		PlayerTurn = !PlayerTurn;


		if (PlayerTurn) {
			TurnDisplay.color = Color.white;
			TurnButton.interactable = true;
			EndMonsterTurn();
			StartPlayerTurn();
		} else {
			TurnDisplay.color = Color.green;
			TurnButton.interactable = false;
			EndPlayerTurn();
			StartMonsterTurn();
		}
	}

	private void StartMonsterTurn()
	{
		//TODO Start monster Turn
		MonstersManager.Instance.MonsterTurn();
	}

	private void EndMonsterTurn()
	{
		//TODO End monster Turn
	}


	private void StartPlayerTurn()
	{
		//TODO Start Player Turn
		PlayersManager.Instance.StartPlayerTurn();
		CardManager.Instance.StartPlayerTurn();
	}

	private void EndPlayerTurn()
	{
		//TODO End player Turn
		CardManager.Instance.EndPlayerTurn();
		GameManager.Instance.EndPlayerTurn();

	}

}
