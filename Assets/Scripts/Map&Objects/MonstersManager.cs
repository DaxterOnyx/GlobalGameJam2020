using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersManager : CharactersManager
{
    private static MonstersManager _instance;
    public static MonstersManager Instance { get { return _instance; } }

    void Start()
    {
        _instance = this;
        Initialize();
    }

}
