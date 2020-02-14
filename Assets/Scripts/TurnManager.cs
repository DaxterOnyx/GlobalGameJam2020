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

    [FMODUnity.EventRef]
    public string endTurnSound;
    [FMODUnity.EventRef]
    public string startTurnSound;

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
            if (Turn != 0)
            {
                FMODUnity.RuntimeManager.PlayOneShot(endTurnSound);
            }
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot(startTurnSound);
            }
            TurnDisplay.color = Color.black;
			TurnButton.interactable = true;
			EndMonsterTurn();
			StartPlayerTurn();
		} else {
            FMODUnity.RuntimeManager.PlayOneShot(startTurnSound);
            TurnDisplay.color = Color.green;
			TurnButton.interactable = false;
			EndPlayerTurn();
			StartMonsterTurn();
		}
		if (GameManager.Instance.isTuto)
		{
			TutoManager.Instance.Next();
			if(Turn==1)
				MonstersManager.Instance.SpawnMonster(TutoManager.Instance.monsterTuto);
		}
	}

	private void StartMonsterTurn()
	{
		//TODO Start monster Turn
		MonstersManager.Instance.StartMonsterTurn();
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
