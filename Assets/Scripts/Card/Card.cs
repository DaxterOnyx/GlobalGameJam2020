using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
	public CardData data;
	public RectTransform RecTransform;
	public Image Image;
	public TextMeshProUGUI Name;
	public TextMeshProUGUI Cost;
	public TextMeshProUGUI Description;
	public TextMeshProUGUI Range;

	private void Start()
	{
		if (data != null)
			Init(data);
	}
	public void Init(CardData data)
	{
		this.data = data;
		gameObject.name = data.Name;
		Image.sprite = data.Image;
		Name.text = data.Name;
		Cost.text = data.Cost.ToString();
		Description.text = data.Description;
		Range.text = data.Range.ToString();
	}

	internal void Action(Player actor, Character target)
	{
		switch (data.Action) {
			case CardData.CardAction.CaC:
				actor.Kick();
				target.TakeDamage(actor.data.Strengh);
				break;
			case CardData.CardAction.Armor:
				//TODO ADD ARMOR EFFECT
				//TODO ADD VISUAL EFFECT
			case CardData.CardAction.Heal:
				//TODO ADD VISUAL EFFECT
				//TODO REMOVE HARC VALUE
				target.TakeDamage(-2);
				break;
			case CardData.CardAction.Attack:
				actor.Shoot();
				target.TakeDamage(actor.data.FireGunDamage);
				break;
			case CardData.CardAction.Repair:
				//TODO REMOVE Hard Value
				(target as Object).Repair(25);
				break;
			default:
				Debug.LogError("Not Defined Action : " + data.Action.ToString());
				break;
		}
	}

	public bool IsValableTarget(Player actor, Character target)
	{
		bool isValable = false;
		//Check distance
		MapManager.Instance.WhereIsObject(target.gameObject, out var targetPos);
		MapManager.Instance.WhereIsObject(actor.gameObject, out var actorPos);

		if (Pathfinding.Instance.findPath(actorPos, targetPos).Count > data.Range)
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
					if (target is Object)
						isValable = true;
					break;
				default:
					Debug.LogError("Not Defined Type : " + data.targetTypes[i].ToString());

					break;
			}
		}

		return isValable;
	}

	internal void Unselect()
	{
		//TODO SHOW TO PLAYER
	}

	internal void Select()
	{
		//TODO SHOW TO PLAYER
	}

	public void OnClick()
	{
		//TODO SHOW TO PLAYER
		GameManager.Instance.SelectCard(this);
	}

	public void SetLastSibling()
	{
		Debug.Log("SetLastSibling");
		RecTransform.SetAsLastSibling();
	}

	internal void Discard(float time)
	{
		Invoke("ReelDiscard", time);
	}

	void ReelDiscard()
	{
		gameObject.SetActive(false);
	}
}