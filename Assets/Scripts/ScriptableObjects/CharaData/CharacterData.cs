using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CharacterData : ScriptableObject
{
    public int nbMaxLP;
    public int nbActionPoint;
}

public enum target { Player, Objective };