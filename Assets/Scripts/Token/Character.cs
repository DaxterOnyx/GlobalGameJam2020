using DG.Tweening;
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
	[SerializeField]
	protected Image ArmorBar;

	public Animator animator;
	public float RotationSpeed = 0.1f;

	[FMODUnity.EventRef]
	public string stepSound;
	protected override void Start()
	{
		highlighter.SetActive(false);
	}



	public int GetCurrentLp()
	{
		return LifePoint;
	}

	public void LookAt(Token target)
	{
		LookAt(target.transform.position);
	}

	public void LookAt(Vector3 targetPos)
	{
		var dif = targetPos - transform.position;
		//inverse x because z is inverse and inverse x and y bcause it's work
		var angle = Mathf.Atan2(-dif.x, dif.y) * Mathf.Rad2Deg;
		var actualAngle = animator.transform.rotation.eulerAngles.z;
		//actor.animator.transform.rotation = Quaternion.Euler(0, 0, angle);
		animator.transform.DORotate(new Vector3(0, 0, angle), RotationSpeed);
	}

	protected  void UpdateArmorDisplay()
	{
		ArmorBar.fillAmount = (float)armorAmount / MaxLifePoint;
	}

	internal void AddArmor(int amount)
	{
		armorAmount += amount;
		UpdateArmorDisplay();
	}

	#region Animator Control
	public void Punch()
	{
		animator.SetTrigger("Punch");
	}
	public void Hurt()
	{
		animator.SetTrigger("Hurted");
	}
	public void Shoot()
	{
		animator.SetTrigger("Shoot");
	}

	public void StartWalk()
	{
        //TODO MONSTE NOT MAKE SOUND ON WALK
        FMODUnity.RuntimeManager.CreateInstance(stepSound);
		animator.SetBool("Walk", true);
	}

	public void StopWalk()
	{
		Debug.Log("Stop walking");
		animator.SetBool("Walk", false);
        

    }

	internal override void TakeDamage(int damage)
	{
		int trueDamage = damage;

		//ARMOR
		if (armorAmount > 0) {
			trueDamage -= armorAmount;
			armorAmount -= damage;
			if (armorAmount < 0)
				armorAmount = 0;
			UpdateArmorDisplay();
		}

		//Animation of damage
		Hurt();

		base.TakeDamage(trueDamage);
	}
	#endregion

	internal override void UpdateLifeDisplay()
	{
		LifeBarFont.fillAmount = (float)LifePoint / MaxLifePoint;
	}
}
