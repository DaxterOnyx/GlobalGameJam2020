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
			Mathf.Clamp(transform.position.x + x * Speed, -3.5f, 3.5f),
			Mathf.Clamp(transform.position.y + y * Speed, -5.5f, 4.5f),
			transform.position.z
			);
	}
}
