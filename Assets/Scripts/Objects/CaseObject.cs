using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseObject : MonoBehaviour
{
    public Case curCase;
    private void Awake()
    {
        curCase = new Case();
    }
}
