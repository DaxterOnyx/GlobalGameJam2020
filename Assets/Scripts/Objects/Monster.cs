using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Character
{
    public MonsterData data;
    public GameObject canvas;
    public Image lifeBar;
    protected override void Start()
    {
        LifePoint = data.nbMaxLP;
        canvas.SetActive(false);
    }
    protected override void Die()
    {
        MonstersManager.Instance.Kill(gameObject);
    }

    internal override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        lifeBar.fillAmount = (float)LifePoint / data.nbMaxLP;
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
