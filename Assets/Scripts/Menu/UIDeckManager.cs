using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDeckManager : MonoBehaviour
{
	private static UIDeckManager _instance;
	public static UIDeckManager Instance
	{
		get {
			if (_instance == null)
				_instance = FindObjectOfType<UIDeckManager>();
			return _instance;
		}
	}

	[SerializeField]
	public Dictionary<Card,int> deck;

	public 

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
