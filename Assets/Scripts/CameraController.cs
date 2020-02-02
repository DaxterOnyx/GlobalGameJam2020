using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float Speed;

	// Update is called once per frame
	void Update()
	{
		var x = Input.GetAxis("Horizontal");
		var y = Input.GetAxis("Vertical");

		//TODO remove hard value
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x + x * Speed, -5f, 5f),
			Mathf.Clamp(transform.position.y + y * Speed, -8f, 6f),
			transform.position.z
			);
	}
}
