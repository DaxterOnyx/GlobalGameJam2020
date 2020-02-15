using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralData", menuName = "Data/GeneralData")]
public class GeneralData : ScriptableObject
{
    public float moveDuration;
    public GameObject caseObject;
    public GameObject objectiveUI;
}
