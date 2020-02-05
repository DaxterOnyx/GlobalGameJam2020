using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CaseObject : MonoBehaviour
{
    public int moveCost;
    private Token player;
    private SpriteRenderer sprt;
    private void Awake()
    {
        sprt = GetComponent<SpriteRenderer>();
    }
    public void UpdateMaterial()
    {
        sprt.material.SetFloat("_ActionCost", moveCost);
    }

    private void OnMouseDown()
    {
		if(!CardManager.Instance.MouseOver)
        player.GetComponent<Player>().SetDestination(
            MapManager.Instance.V3toV2I(transform.position),moveCost);
    }
    public void SetPlayer(Token token)
    {
        player = token;
    }
}
