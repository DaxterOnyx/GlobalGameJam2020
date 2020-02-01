using System;

public class Player : Character
{
	public PlayerData data;

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
