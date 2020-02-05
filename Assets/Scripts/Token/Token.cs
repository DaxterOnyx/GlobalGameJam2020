﻿using UnityEngine;

public abstract class Token : MonoBehaviour
{
	public GameObject highlighter;

	protected int LifePoint;

	protected abstract void Start();

	internal abstract void Die();

	internal virtual void TakeDamage(int damage)
	{
		LifePoint -= damage;
		Debug.Log(damage + " Damages taken!");
		if (LifePoint <= 0)
			Die();
	}

	internal virtual void Highlight(bool isTarget = true)
	{
		highlighter.SetActive(true);
	}

	public virtual void Delight()
	{
		highlighter.SetActive(false);
	}

	protected virtual void OnMouseDown()
	{
		GameManager.Instance.SelectTarget(this);
	}
}