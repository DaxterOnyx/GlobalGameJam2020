﻿using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
	public PlayerData data;
	public GameObject playerHighlighter;
	public Animator animator;
	public GameObject Selector;
	public int actionLeft { get; private set; }
	private Sequence sequence;
	private bool isWalking;
	public Image lifeBar;
	public List<GameObject> listActionPoints;
	public Color transparentLifeBar;
	private Color normalLifeBar = Color.white;
    [FMODUnity.EventRef]
    public string stepSound;
	

    protected override void Start()
	{
		LifePoint = data.nbMaxLP;
		ResetActionPoint();
		RefreshActionPointDisplay();
		foreach (var item in GetComponentsInChildren<Image>())
		{
			item.color = transparentLifeBar;
		}
		highlighter.SetActive(false);
		playerHighlighter.SetActive(false);
	}

	private void Update()
	{
		if (isWalking)
		{
			if (sequence.Elapsed()>=sequence.Duration())
			{
				StopWalk();
				isWalking = false;

			}
		}
	}

	protected override void Die()
	{
		PlayersManager.Instance.Kill(gameObject);
	}
	internal override void TakeDamage(int damage)
	{
		Hurt();
		base.TakeDamage(damage);
		lifeBar.fillAmount =(float) LifePoint / data.nbMaxLP;
	}

	internal void Unselect()
	{
		//TODO SHOW PLAYER
		Selector.SetActive(false);
		MapManager.Instance.DestroyCaseMap();
	}

	internal void Select()
	{
		//TODO SHOW PLAYER
		Selector.SetActive(true);
		if(GameManager.Instance.CardSelected == null)
		{
			MapManager.Instance.GenerateCaseMap(gameObject, actionLeft);
		}
	}

	protected override void OnMouseDown()
	{
		base.OnMouseDown();
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
		Debug.Log("Stop walking");
		animator.SetBool("Walk", false);
	}

	public void StartWalk()
	{
        FMODUnity.RuntimeManager.PlayOneShot(stepSound);
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
		for (int i = 0; i < actionLeft; i++)
		{
			listActionPoints[i].SetActive(true);
		}
		for (int i = actionLeft; i < listActionPoints.Count; i++)
		{
			listActionPoints[i].SetActive(false);
		}
		if (MapManager.Instance.caseList.Count > 0)
		{
			MapManager.Instance.DestroyCaseMap();
			MapManager.Instance.GenerateCaseMap(gameObject, actionLeft);
		}
	}

	public void SetDestination(Vector2Int position,int moveCost)
	{
		StartWalk();
		MapManager.Instance.DestroyCaseMap();
		sequence = MapManager.Instance.Move(
			gameObject, Pathfinding.Instance.findPath(
				MapManager.Instance.V3toV2I(transform.position), position));
		isWalking = true;
		actionLeft -= moveCost;
		Unselect();
		RefreshActionPointDisplay();
	}

	private void OnMouseOver()
	{
		foreach (var item in GetComponentsInChildren<Image>())
		{
			item.color = normalLifeBar;
		}
	}

	private void OnMouseExit()
	{
		foreach (var item in GetComponentsInChildren<Image>())
		{
			item.color = transparentLifeBar;
		}
	}
	public override void Highlight(bool isTarget = true)
	{
		if (!isTarget)
			base.Highlight(isTarget);
		else
			playerHighlighter.SetActive(true);
	}
	public override void Delight()
	{
		base.Delight();
		playerHighlighter.SetActive(false);
	}
}
