using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Character
{
    public MonsterData data;
    public GameObject canvas;

    protected override void Start()
    {
        LifePoint = data.nbMaxLP;
        highlighter.SetActive(false);
        canvas.SetActive(false);
		base.Start();
    }

	internal override void Die()
	{
		MonstersManager.Instance.Kill(this);
	}

	internal override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        LifeBarFont.fillAmount = (float)LifePoint / data.nbMaxLP;
    }

    private void OnMouseOver()
    {
        canvas.SetActive(true);
    }
    private void OnMouseExit()
    {
        canvas.SetActive(false);
        
    }
}
