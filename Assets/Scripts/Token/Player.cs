using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : Character
{
	public PlayerData data;
	public GameObject playerHighlighter;
	public int actionLeft { get; private set; }
	private Sequence sequence;
	private bool isWalking;
	public List<Image> listActionPoints;
	public GameObject Selector;

    protected override void Start()
	{
		Init(data.nbMaxLP);
		ResetActionPoint();
		RefreshActionPointDisplay();
		foreach (var image in listActionPoints) {
			image.color = transparentLifeBar;
		}
		LifeBarFont.color = transparentLifeBar;
		LifeBarBack.color = transparentLifeBar;
		ArmorBar.color = transparentLifeBar;

		highlighter.SetActive(false);
		playerHighlighter.SetActive(false);
		base.Start();
	}

	private void Update()
	{
		if (isWalking) {
			if (sequence.Elapsed() >= sequence.Duration()) {
				StopWalk();
				isWalking = false;
			}
		}
	}

	internal override void Die()
	{
		PlayersManager.Instance.Kill(this);
	}

	internal void Unselect()
	{
		Selector.SetActive(false);
		MapManager.Instance.DestroyCaseMap();
	}

	internal void Select()
	{
		Selector.SetActive(true);

		if (GameManager.Instance.CardSelected == null) {
			MapManager.Instance.GenerateCaseMap(this, actionLeft);
		}
	}

	protected override void OnMouseDown()
	{
		base.OnMouseDown();
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
		for (int i = 0; i < actionLeft; i++) {
			listActionPoints[i].gameObject.SetActive(true);
		}
		for (int i = actionLeft; i < listActionPoints.Count; i++) {
			listActionPoints[i].gameObject.SetActive(false);
		}
		if (MapManager.Instance.caseList.Count > 0) {
			MapManager.Instance.DestroyCaseMap();
			MapManager.Instance.GenerateCaseMap(this, actionLeft);
		}
	}

	public void SetDestination(Vector2Int position, int moveCost)
	{
		StartWalk();
		MapManager.Instance.DestroyCaseMap();
		sequence = MapManager.Instance.Move(
			this, Pathfinding.Instance.findPath(
				MapManager.Instance.V3toV2I(transform.position), position));
		isWalking = true;
		actionLeft -= moveCost;
		Unselect();
		GameManager.Instance.PlayerSelected = null;
		RefreshActionPointDisplay();
	}

	private void OnMouseOver()
	{
		if (!EventSystem.current.IsPointerOverGameObject()) {

			foreach (var image in listActionPoints) {
				image.color = normalLifeBar;
			}
			LifeBarBack.color = normalLifeBar;
			LifeBarFont.color = normalLifeBar;
			ArmorBar.color = normalLifeBar;
		}
	}

	private void OnMouseExit()
	{
		foreach (var image in listActionPoints) {
			image.color = transparentLifeBar;
		}
		LifeBarBack.color = transparentLifeBar;
		LifeBarFont.color = transparentLifeBar;
		ArmorBar.color = transparentLifeBar;
	}
	internal override void Highlight(bool isTarget = true)
	{
		if (isTarget)
			base.Highlight();
		else
			playerHighlighter.SetActive(true);
	}
	public override void Delight()
	{
		base.Delight();
		playerHighlighter.SetActive(false);
	}
}
