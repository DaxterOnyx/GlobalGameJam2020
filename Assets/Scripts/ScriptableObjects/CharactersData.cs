using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharactersData" , menuName = "Data/CharactersData")]
public class CharactersData : ScriptableObject
{
    public List<(GameObject, Vector2Int)> characterPositionList;
}
