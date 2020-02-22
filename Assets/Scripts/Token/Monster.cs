using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : Character
{
	public MonsterData data;
	public GameObject canvas;
	public MonsterActions actions { get; private set; }
	protected override void Start()
	{
		Init(data.nbMaxLP);
		highlighter.SetActive(false);
		canvas.SetActive(false);
		base.Start();
		actions = GetComponent<MonsterActions>();
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
