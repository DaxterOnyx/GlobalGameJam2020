using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : Character
{
	public MonsterData data;
	public GameObject canvas;

	protected override void Start()
	{
		Init(data.nbMaxLP);
		highlighter.SetActive(false);
		canvas.SetActive(false);
		base.Start();
	}

	internal override void Die()
	{
		MonstersManager.Instance.Kill(this);
	}

	private void OnMouseOver()
	{
		if (!EventSystem.current.IsPointerOverGameObject()) {
			canvas.SetActive(true);
		}
	}
	private void OnMouseExit()
	{
		canvas.SetActive(false);
	}
}
