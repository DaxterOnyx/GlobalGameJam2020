using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float Speed;

	// Update is called once per frame
	void Update()
	{
		var x = Input.GetAxis("Horizontal");
		var y = Input.GetAxis("Vertical");

		transform.Translate(x * Speed, y * Speed, 0);
	}
}
