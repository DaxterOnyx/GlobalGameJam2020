using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Object : Character
{
    public ObjectData data;
	public Image repairBar;
	public GameObject canvas;
	int repairPoint;

	protected override void Start()
	{
		canvas.SetActive(data.isObjective);
		LifePoint = data.nbMaxLP;
		repairPoint = 0;
		if(data.isObjective) UpdateRepair();
	}
	protected override void Die()
	{
		ObjectsManager.Instance.Kill(gameObject);
	}

	public void Repair(int value)
	{
		repairPoint += value;
		UpdateRepair();
		if (repairPoint >= data.repairCount && data.isObjective)
		{
			//TODO Check if another objectif is necesarry
			Debug.Log("One Objectif Repaired");
			ObjectsManager.Instance.RemoveObjective(gameObject);
			GameManager.Instance.IsGameWin();
		}
	}

	public void UpdateRepair()
	{
		repairBar.fillAmount = (float)repairPoint / data.repairCount;
	}
}
