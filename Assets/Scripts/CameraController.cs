using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float Speed;
	private float minX, minY, maxX, maxY;

	private void Start()
	{
		minX = MapManager.Instance.mapData.CamMinX;
		minY = MapManager.Instance.mapData.CamMinY;
		maxX = MapManager.Instance.mapData.CamMaxX;
		maxY = MapManager.Instance.mapData.CamMaxY;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		var x = Mathf.Clamp(Input.GetAxis("Horizontal") + CamControl.Instance.axis.x,-1,1);
		var y = Mathf.Clamp(Input.GetAxis("Vertical") + CamControl.Instance.axis.y,-1,1);


		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x + x * Speed, minX, maxX),
			Mathf.Clamp(transform.position.y + y * Speed, minY, maxY),
			transform.position.z
			);
	}
}
