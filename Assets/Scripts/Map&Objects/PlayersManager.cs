using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : CharactersManager
{
    private static PlayersManager _instance;
    public static PlayersManager Instance { get { return _instance; } }
    public PlayerManagerData data;
    void Start()
    {
        _instance = this;
        Initialize(data);
    }

}
