using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	public Transform Player;
	public Vector2 offset;

	// Update is called once per frame
	void Update()
	{
		Vector3 pos = Camera.main.WorldToScreenPoint(new Vector3(Player.position.x + offset.x, Player.position.y + offset.y, Player.position.z));
		transform.position = new Vector3(pos.x, pos.y , pos.z);
	}
}
