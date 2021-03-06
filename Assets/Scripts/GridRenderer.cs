﻿using UnityEngine;

public class GridRenderer : MonoBehaviour
{
	public int nbColumn;
	public int nbLine;
	public Vector2 CellSize;
	public GameObject LineRenderer;

	// Start is called before the first frame update
	void Start()
	{
		for (float x = -50.5f ; x < 50; x += CellSize.x) {
			var obj = Instantiate(LineRenderer, transform).GetComponent<LineRenderer>();
			obj.SetPosition(0, new Vector3(-50, x, transform.position.z));
			obj.SetPosition(1, new Vector3(50, x, transform.position.z));

		}

		for (float y = -50.5f; y <= 50; y += CellSize.y) {
			var obj = Instantiate(LineRenderer, transform).GetComponent<LineRenderer>();
			obj.SetPosition(0, new Vector3(y, -50, transform.position.z));
			obj.SetPosition(1, new Vector3(y, 50, transform.position.z));
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}