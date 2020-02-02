using System;

public class Player : Character
{
	public PlayerData data;
	protected override void Start()
	{
		LifePoint = data.nbMaxLP;
	}
	protected override void Die()
	{
		PlayersManager.Instance.Kill(gameObject);
	}
	internal void Unselect()
	{
		//TODO SHOW PLAYER
		throw new NotImplementedException();
	}

	internal void Select()
	{//TODO SHOW PLAYER
		throw new NotImplementedException();
	}

	internal void OnClick()
	{
		GameManager.Instance.SelectPlayer(this);
	}
}
