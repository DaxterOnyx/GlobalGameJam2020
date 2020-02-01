using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : Character
{
    public ObjectData data;

	int repairPoint;

	private void Start()
	{
		repairPoint = data.repairCount;
	}

	public void Repair(int value)
	{
		repairPoint -= value;
		if (repairPoint <= 0 && data.isObjective)
			//TODO Check if another objectif is necesarry
			Debug.Log("One Objectif Repaired");
	}
}
