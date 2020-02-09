using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float Speed;
	private float minX, minY, maxX, maxY;

	private void Start()
	{
		minX = MapManager.Instance.data.CamMinX;
		minY = MapManager.Instance.data.CamMinY;
		maxX = MapManager.Instance.data.CamMaxX;
		maxY = MapManager.Instance.data.CamMaxY;
	}

	// Update is called once per frame
	void Update()
	{
		var x = Input.GetAxis("Horizontal");
		var y = Input.GetAxis("Vertical");


		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x + x * Speed, minX, maxX),
			Mathf.Clamp(transform.position.y + y * Speed, minY, maxY),
			transform.position.z
			);
	}
}
