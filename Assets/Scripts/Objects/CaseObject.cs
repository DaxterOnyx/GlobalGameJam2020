using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseObject : MonoBehaviour
{
    public int moveCost;
    private GameObject player;
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
        player.GetComponent<Player>().SetDestination(
            MapManager.Instance.V3toV2I(transform.position),moveCost);
    }
    public void SetPlayer(GameObject obj)
    {
        player = obj;
    }
}
