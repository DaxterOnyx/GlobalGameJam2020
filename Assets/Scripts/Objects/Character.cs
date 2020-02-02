using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData initialData;

	protected int LifePoint;

	protected virtual void Start()
	{
		LifePoint = initialData.nbMaxLP;
	}

	internal virtual void TakeDamage(int damage)
	{
		LifePoint -= damage;
		Debug.Log(damage + "Damages taken!");
		if (LifePoint <= 0)
			Die();
	}

	protected virtual void Die()
	{ 

	}

	private void OnMouseDown()
	{
		GameManager.Instance.SelectTarget(this);
	}
}
