using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour
	 , IPointerClickHandler, IPointerEnterHandler
	 , IPointerExitHandler
{
	public CardData data;
	public RectTransform RecTransform;
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

	private bool interactable;

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

	internal void ActionCard(Player actor, Character target)
	{
		if (data.Cost > actor.actionLeft) {
			Debug.LogError("Not enough action point");
			return;
		}

		actor.UseActionPoint(data.Cost);

		//TODO MOVE IN CHARACTER
		#region look at the target
		var dif = target.transform.position - actor.animator.transform.position;
		//inverse x because z is inverse and inverse x and y bcause it's work
		var angle = Mathf.Atan2(-dif.x, dif.y) * Mathf.Rad2Deg;
		var actualAngle = actor.animator.transform.rotation.eulerAngles.z;
		//actor.animator.transform.rotation = Quaternion.Euler(0, 0, angle);
		//TODO Remove hard value
		actor.animator.transform.DORotate(new Vector3(0, 0, angle), 0.1f);
		#endregion

		switch (data.Action) {
			case CardData.CardAction.CaC:
                FMODUnity.RuntimeManager.PlayOneShot(punchSound);
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
                FMODUnity.RuntimeManager.PlayOneShot(gunSound);
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

		CardManager.Instance.DiscardCard(this);
	}

	public bool IsValableTarget(Player actor, Character target)
	{
		bool isValable = false;
		//Check distance
		if (!MapManager.Instance.WhereIsObject(target.gameObject, out var targetPos)
		 || !MapManager.Instance.WhereIsObject(actor.gameObject, out var actorPos))
			return false;

		if (actorPos != targetPos && Pathfinding.Instance.findPath(actorPos, targetPos).Count-1 > data.Range)
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
		if (GameManager.Instance.CardSelected == this)
			GameManager.Instance.CardSelected = null;
		animator.SetBool("Selected", false);
	}

	internal void Select()
	{
		animator.SetBool("Selected", true);
	}

	internal void Discard(float time)
	{
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
		FMODUnity.RuntimeManager.PlayOneShot(touchSound);
		animator.SetTrigger("Click");
		RecTransform.SetAsLastSibling();
		if (Interactable)
			GameManager.Instance.SelectCard(this);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		FMODUnity.RuntimeManager.PlayOneShot(touchSound);
		CardManager.Instance.MouseOver = true;
		RecTransform.SetAsLastSibling();
		animator.SetBool("Over", true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		CardManager.Instance.MouseOver = false;
		animator.SetBool("Over", false);
	}
}