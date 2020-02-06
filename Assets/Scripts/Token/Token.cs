using UnityEngine;

public abstract class Token : MonoBehaviour
{
	public GameObject highlighter;

	protected int MaxLifePoint;
	protected int LifePoint;
	protected int armorAmount;

	protected abstract void Start();

	protected virtual void Init(int maxLifePoint)
	{
		MaxLifePoint = maxLifePoint;
		LifePoint = MaxLifePoint;
		UpdateLifeDisplay();
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

	internal virtual void Heal(int amount)
	{
		LifePoint += amount;
		if (LifePoint > MaxLifePoint)
			LifePoint = MaxLifePoint;
		UpdateLifeDisplay();
	}

	internal virtual void TakeDamage(int damage)
	{
		LifePoint -= damage;
		Debug.Log(damage + " Damages taken!");
		if (LifePoint <= 0)
			Die();

		UpdateLifeDisplay();
	}

	internal abstract void UpdateLifeDisplay();
	internal abstract void Die();

}