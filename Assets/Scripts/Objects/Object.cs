using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : Character
{
    public ObjectData data;

	int repairPoint;

	protected override void Start()
	{
		LifePoint = data.nbMaxLP;
		repairPoint = data.repairCount;
	}
	protected override void Die()
	{
		ObjectsManager.Instance.Kill(gameObject);
	}

	public void Repair(int value)
	{
		repairPoint -= value;
		if (repairPoint <= 0 && data.isObjective)
			//TODO Check if another objectif is necesarry
			Debug.Log("One Objectif Repaired");
	}
}
