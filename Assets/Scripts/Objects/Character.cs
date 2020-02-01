﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData initialData;

	private int LifePoint;

	private void Start()
	{
		LifePoint = initialData.nbMaxLP;
	}

	internal virtual void TakeDamage(int damage)
	{
		LifePoint -= damage;
		if (LifePoint <= 0)
			Die();
	}

	private void Die()
	{
		Destroy(gameObject);
	}
}
