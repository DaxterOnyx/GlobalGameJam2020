using UnityEngine;

public class GridRenderer : MonoBehaviour
{
	public int nbColumn;
	public int nbLine;
	public Vector2 CellSize;
	public GameObject LineRenderer;

	// Start is called before the first frame update
	void Start()
	{
		for (float x = -(nbColumn+1) / 2f; x < (nbColumn) / 2f; x += CellSize.x) {
			var obj = Instantiate(LineRenderer, transform).GetComponent<LineRenderer>();
			obj.SetPosition(0, new Vector3(-50, x, 0));
			obj.SetPosition(1, new Vector3(50, x, 0));

		}

		for (float y = -(nbLine) / 2f; y <= (nbLine) / 2f; y += CellSize.y) {
			var obj = Instantiate(LineRenderer, transform).GetComponent<LineRenderer>();
			obj.SetPosition(0, new Vector3(y, -50, 0));
			obj.SetPosition(1, new Vector3(y, 50, 0));
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}