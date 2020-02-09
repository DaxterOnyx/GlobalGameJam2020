using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Structure : Token
{
    public StructureData data;
	public Image repairBar;
	public GameObject objectiveBar;
	public GameObject lifeBar;
	public Image lifeBarFont;
	int repairPoint;

	protected override void Start()
	{
		lifeBar.SetActive(false);
		LifePoint = data.nbMaxLP;
		repairPoint = 0;
		if (data.isObjective)
		{
			UpdateRepair();
		}
		highlighter.SetActive(false);
		objectiveBar.SetActive(false);
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
		objectiveBar.SetActive(data.isObjective);
		if (LifePoint < MaxLifePoint)
			lifeBar.SetActive(true);
	}
	private void OnMouseExit()
	{
		if (data.isObjective)
		{
			objectiveBar.SetActive(false);
		}
		lifeBar.SetActive(false);
	}

	internal override void UpdateLifeDisplay()
	{
		lifeBarFont.fillAmount = (float)LifePoint / MaxLifePoint;
	}
}
