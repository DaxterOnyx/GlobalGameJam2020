using System;
using TMPro;
using UnityEngine;

public class Player : Character
{
	public PlayerData data;

	public Animator animator;
	public GameObject Selector;
	public int actionLeft;
	public TextMeshProUGUI ActionDisplay;

	protected override void Start()
	{
		LifePoint = data.nbMaxLP;
		ResetActionPoint();
		RefreshActionPointDisplay();
	}
	protected override void Die()
	{
		PlayersManager.Instance.Kill(gameObject);
	}
	internal override void TakeDamage(int damage)
	{
		Hurt();
		base.TakeDamage(damage);
	}

	internal void Unselect()
	{
		//TODO SHOW PLAYER
		Selector.SetActive(false);
	}

	internal void Select()
	{
		//TODO SHOW PLAYER
		Selector.SetActive(true);
	}

	protected override void OnMouseDown()
	{
		base.OnMouseDown();
		MapManager.Instance.GenerateCaseMap(MapManager.Instance.V3toV2I(transform.position), actionLeft);
	}

	public void Kick()
	{
		animator.SetTrigger("Kick");
	}
	public void Hurt()
	{
		animator.SetTrigger("Hurt");
	}
	public void Shoot()
	{
		animator.SetTrigger("Shoot");
	}
	public void StopWalk()
	{
		animator.SetBool("Walk", false);
	}

	public void StartWalk()
	{
		animator.SetBool("Walk", true);
	}

	public void ResetActionPoint()
	{
		actionLeft = data.nbActionPoint;
		RefreshActionPointDisplay();
	}

	public void UseActionPoint(int nb)
	{
		if (actionLeft < nb)
			Debug.LogError("Not enough action point.");

		actionLeft -= nb;
		RefreshActionPointDisplay();
	}

	private void RefreshActionPointDisplay()
	{
		ActionDisplay.text = actionLeft.ToString();
	}
}
