using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
    }
}
