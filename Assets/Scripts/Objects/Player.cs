using System;

public class Player : Character
{
	public PlayerData data;
	protected override void Start()
	{
		LifePoint = data.nbMaxLP;
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
