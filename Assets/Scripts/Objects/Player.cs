using System;
using UnityEngine;

public class Player : Character
{
	public PlayerData data;

	public Animator animator;

	protected override void Start()
	{
		LifePoint = data.nbMaxLP;
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
		throw new NotImplementedException();
	}

	internal void Select()
	{//TODO SHOW PLAYER
		throw new NotImplementedException();
	}

	internal void OnClick()
	{
		GameManager.Instance.SelectPlayer(this);
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

}
