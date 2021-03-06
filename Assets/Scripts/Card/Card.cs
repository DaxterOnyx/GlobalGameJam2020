﻿using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour
	 , IPointerClickHandler, IPointerEnterHandler
	 , IPointerExitHandler
{
	public CardData data;
	public RectTransform RecTransform;
	internal float CardSize { get { return RecTransform.rect.width * 3; } }
	public Image Image;
	public TextMeshProUGUI Name;
	public TextMeshProUGUI Cost;
	public TextMeshProUGUI Description;
	public TextMeshProUGUI Range;
	public Animator animator;

	public GameObject Face;
	public GameObject Back;

	delegate void Action();
	/*List<(float,*/
	Action/*)>*/ actions;
	float actionTimer;

	[FMODUnity.EventRef]
	public string touchSound;
	[FMODUnity.EventRef]
	public string gunSound;
	[FMODUnity.EventRef]
	public string punchSound;
    [FMODUnity.EventRef]
    public string repairSound;

    private bool interactable;
	private bool inPlay;
	private bool isInit;

	public bool Interactable
	{
		get { return interactable; }
		set {
			if (!value)
				Unselect();
			interactable = value;
		}
	}

	private void Start()
	{
		//actions = new List<(float, Action)>();
		if (data != null && !isInit)
			Init(data, true);
	}

	public void Init(CardData data, bool inPlay)
	{
		isInit = true;
		this.data = data;
		gameObject.name = data.Name;
		Image.sprite = data.Image;
		Name.text = data.Name;
		Cost.text = data.Cost.ToString();
		Description.text = data.Description;
		Range.text = data.Range.ToString();
		this.inPlay = inPlay;
	}

	private void Update()
	{
		//if (actions.Count > 0) {
		//for (int i = 0; i < actions.Count; i++) {
		//actions[i].Item1 = Time.deltaTime;
		if (actions != null) {

			actionTimer -= Time.deltaTime;
			if (actionTimer <= 0) {
				actions();
				actions = null;
			}
		}
		//}
		//}
	}

	internal void ActionCard(Player actor, Token target)
	{
		if (data.Cost > actor.actionLeft) {
			Debug.LogError("Not enough action point");
			return;
		}

		actor.UseActionPoint(data.Cost);

		actor.LookAt(target);

		switch (data.Action) {
			case CardData.CardAction.CaC:
				FMODUnity.RuntimeManager.PlayOneShot(punchSound);
				actor.Punch();
				target.TakeDamage(actor.data.Strengh);
				break;
			case CardData.CardAction.Armor:
				actor.AddArmor(actor.data.Defence);
				break;
			case CardData.CardAction.Heal:
				target.Heal(data.Value);
				break;
			case CardData.CardAction.Attack:
				FMODUnity.RuntimeManager.PlayOneShot(gunSound);
				actor.Shoot();
				target.TakeDamage(actor.data.FireGunDamage);
				break;
			case CardData.CardAction.Repair:
				(target as Structure).Repair(actor.data.Repair);
                FMODUnity.RuntimeManager.PlayOneShot(repairSound);
                break;
			default:
				Debug.LogError("Not Defined Action : " + data.Action.ToString());
				break;
		}

		CardManager.Instance.DiscardCard(this);
	}

	public bool IsValableTarget(Player actor, Token target)
	{
		bool isValable = false;
		//Check distance
		if (!MapManager.Instance.WhereIsToken(target, out var targetPos)
		 || !MapManager.Instance.WhereIsToken(actor, out var actorPos))
			return false;

		if (actorPos != targetPos && Pathfinding.Instance.findPath(actorPos, targetPos).Count - 1 > data.Range)
			return false;

		//Check target match
		for (int i = 0; i < data.targetTypes.Length && !isValable; i++) {
			switch (data.targetTypes[i]) {
				case CardData.TargetType.Player:
					if (target is Player)
						isValable = true;
					break;
				case CardData.TargetType.Himself:
					if (target == actor)
						isValable = true;
					break;
				case CardData.TargetType.Monster:
					if (target is Monster)
						isValable = true;
					break;
				case CardData.TargetType.Object:
					if (target is Structure)
						isValable = true;
					break;
				case CardData.TargetType.Objective:
					if (target is Structure && StructuresManager.Instance.IsObjective(target as Structure))
						isValable = true;
					break;
				default:
					Debug.LogError("Not Defined Type : " + data.targetTypes[i].ToString());

					break;
			}
		}

		return isValable;
	}


	/// <summary>
	/// Use on selected card to unselect it.
	/// Generate DeplacementCase if one player is selected.
	/// </summary>
	internal void Unselect()
	{
		if (!inPlay)
			return;
		if (GameManager.Instance.CardSelected == this) {
			//Selected
			GameManager.Instance.CardSelected = null;
			if (GameManager.Instance.PlayerSelected == null) {
				//No player Selected

			} else {
				//One player is selected
				//Authorize player to move
				MapManager.Instance.GenerateCaseMap(GameManager.Instance.PlayerSelected, GameManager.Instance.PlayerSelected.actionLeft);
			}
		} else {
			//NOT Selected
			Debug.LogWarning("Unselect not selected card.");
		}

		animator.SetBool("Over", false);
		animator.SetBool("Selected", false);
	}

	internal void Select()
	{
		if (!inPlay)
			return;
		animator.SetBool("Selected", true);
	}

	internal void Discard(float time)
	{
		Unselect();
		actions = ReelDiscard;
		actionTimer = time;
		Disapear(time);
	}

	public void Disapear(float time)
	{
		actions = Disapear;
		actionTimer = time;
	}

	private void Disapear()
	{
		gameObject.SetActive(false);
	}

	public void Show()
	{
		gameObject.SetActive(true);

	}

	void ReelDiscard()
	{
		Return(false);
	}

	public void Return(bool faced)
	{
		Face.SetActive(faced);
		Back.SetActive(!faced);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		eventData.Use();
		if (!inPlay)
			return;
		FMODUnity.RuntimeManager.PlayOneShot(touchSound);
		animator.SetTrigger("Click");
		RecTransform.SetAsLastSibling();
		if (Interactable)
			GameManager.Instance.SelectCard(this);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		eventData.Use();
		if (!inPlay)
			return;
		FMODUnity.RuntimeManager.PlayOneShot(touchSound);
		CardManager.Instance.MouseOver = true;
		RecTransform.SetAsLastSibling();
		animator.SetBool("Over", true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		eventData.Use();
		if (!inPlay)
			return;
		CardManager.Instance.MouseOver = false;
		animator.SetBool("Over", false);
	}
}