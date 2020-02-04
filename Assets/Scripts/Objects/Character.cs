using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData initialData;
	public GameObject highlighter;

	protected int LifePoint;

	protected virtual void Start()
	{
		LifePoint = initialData.nbMaxLP;
		highlighter.SetActive(false);
	}

	internal virtual void TakeDamage(int damage)
	{
		LifePoint -= damage;
		Debug.Log(damage + " Damages taken!");
		if (LifePoint <= 0)
			Die();
	}

	protected virtual void Die()
	{ 

	}

	protected virtual void OnMouseDown()
	{
		GameManager.Instance.SelectTarget(this);
	}

	public int GetCurrentLp()
	{
		return LifePoint;
	}

	/// <summary>
	/// Highlight The Character
	/// </summary>
	/// <param name="isTarget">Only need to be set as false when selection player, leave void else</param>
	public virtual void Highlight(bool isTarget = true)
	{
		highlighter.SetActive(true);
	}

	public virtual void Delight()
	{
		highlighter.SetActive(false);
	}
}
