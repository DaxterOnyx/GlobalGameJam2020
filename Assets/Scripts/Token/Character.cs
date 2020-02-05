using UnityEngine;
using UnityEngine.UI;

public abstract class Character : Token
{
	[SerializeField]
	protected Color transparentLifeBar;
	[SerializeField]
	protected Color normalLifeBar = Color.white;
	[SerializeField]
	protected Image LifeBarBack;
	[SerializeField]
	protected Image LifeBarFont;

	public Animator animator;

	[FMODUnity.EventRef]
	public string stepSound;
	protected override void Start()
	{
		highlighter.SetActive(false);
	}

	internal abstract override void Die();

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
	internal override void Highlight(bool isTarget = true)
	{
		highlighter.SetActive(true);
	}

	public virtual void Delight()
	{
		highlighter.SetActive(false);
	}

	#region Animator Control
	public void Punch()
	{
		animator.SetTrigger("Punch");
	}
	public void Hurt()
	{
		animator.SetTrigger("Hurt");
	}
	public void Shoot()
	{
		animator.SetTrigger("Shoot");
	}

	public void StartWalk()
	{
		//TODO MONSTE NOT MAKE SOUND ON WALK
		FMODUnity.RuntimeManager.PlayOneShot(stepSound);
		animator.SetBool("Walk", true);
	}

	public void StopWalk()
	{
		Debug.Log("Stop walking");
		animator.SetBool("Walk", false);
	}

	internal override void TakeDamage(int damage)
	{
		base.TakeDamage(damage);
		Hurt();
	}
	#endregion
}
