using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseObject : MonoBehaviour
{
    public int moveCost;
    private SpriteRenderer sprt;
    private void Awake()
    {
        sprt = GetComponent<SpriteRenderer>();
    }
    public void UpdateMaterial()
    {
        sprt.material.SetFloat("Cost", moveCost);
    }
}
