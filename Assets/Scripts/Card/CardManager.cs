using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	private CardManager _instance;
	public CardManager Instance { get { return _instance; } }

	public ManagerCardData data;

	private List<Card> deck;
	private List<Card> hand;
	private List<Card> discard;

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
