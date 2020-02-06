using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Structure : Token
{
    public StrutureData data;
	public Image repairBar;
	public GameObject canvas;
	int repairPoint;

	protected override void Start()
	{
		LifePoint = data.nbMaxLP;
		repairPoint = 0;
		if (data.isObjective)
		{
			UpdateRepair();
		}
		highlighter.SetActive(false);
		canvas.SetActive(false);
	}

	internal override void Die()
	{
		StructuresManager.Instance.Kill(this);
	}

	public void Repair(int value)
	{
		repairPoint += value;
		UpdateRepair();
		if (repairPoint >= data.repairCount && data.isObjective)
		{
			//TODO Check if another objectif is necesarry
			Debug.Log("One Objectif Repaired");
			StructuresManager.Instance.RemoveObjective(this);
			GameManager.Instance.IsGameWin();
		}
	}

	public void UpdateRepair()
	{
		repairBar.fillAmount = (float)repairPoint / data.repairCount;
	}

	private void OnMouseOver()
	{
		canvas.SetActive(data.isObjective);
	}
	private void OnMouseExit()
	{
		if (data.isObjective)
		{
			canvas.SetActive(false);
		}
	}

	internal override void UpdateLifeDisplay()
	{
		//TODO SHOW LIFE STRUCTURE
		throw new System.NotImplementedException();
	}
}
