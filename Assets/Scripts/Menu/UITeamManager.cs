using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITeamManager : MonoBehaviour
{
	private static UITeamManager _instance;
	public static UITeamManager Instance
	{
		get {
			if (_instance == null)
				_instance = FindObjectOfType<UITeamManager>();
			return _instance;
		}
	}

	public RectTransform PlayersRef;
	public TextMeshProUGUI counter;
	public Button Play; 

	public GameObject PlayerPrefab;
	public List<UIPlayerDisplay> players;


    // Start is called before the first frame update
    void Start()
    {
		_instance = this;

    }

    // Update is called once per frame
    void Update()
    {
		//TODO Count the number of player selected not in Update
		int count = 0;
		foreach (var item in players) {
			if (item.isSelected)
				count++;
		}

		//TODO CHEAT IS HERE
		counter.text = count + "/2";


		if (count == 2)
			Play.interactable = true;
		else
			Play.interactable = false;
    }
}
