using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	#region Fields
	private static MapManager _instance;
	public static MapManager Instance
	{
		get {
			if (_instance == null)
				_instance = FindObjectOfType<MapManager>();
			return _instance;
		}
	}

	public MapData data;
	public List<CaseObject> caseList = new List<CaseObject>();
	private Dictionary<Token, Vector2Int> dico = new Dictionary<Token, Vector2Int>();
	public int minX { get; private set; } 
	public int minY { get; private set; }
	public int maxX { get; private set; }
	public int maxY { get; private set; }
	#endregion
	private void Awake()
	{
		_instance = this;
		minX = data.MapMinX; 
		minY = data.MapMinY; 
		maxX = data.MapMaxX; 
		maxY = data.MapMaxY;
	}
	private void Update()
	{

	}

	public bool GenerateCaseMap(Token player, int maxCost)
	{
		RecCreationCase(V3toV2I(player.transform.position), V3toV2I(player.transform.position), maxCost, player);
		CaseObject initCase = caseList[0];
		caseList.Remove(initCase);
		Destroy(initCase.gameObject);
		return true;
	}

	public void DestroyCaseMap()
	{
		foreach (var item in caseList) {
			Destroy(item.gameObject);
		}
		caseList.Clear();
	}
	public void RecCreationCase(Vector2Int pos, Vector2Int initialPos, int maxCost, Token player)
	{
		CaseObject curCase;
		curCase = CreateCase(pos, initialPos, maxCost, player);
		if (curCase != null) {
			RecCreationCase(pos + new Vector2Int(0, 1), initialPos, maxCost, player);
			RecCreationCase(pos + new Vector2Int(1, 0), initialPos, maxCost, player);
			RecCreationCase(pos + new Vector2Int(0, -1), initialPos, maxCost, player);
			RecCreationCase(pos + new Vector2Int(-1, 0), initialPos, maxCost, player);
		}
	}

	public CaseObject CreateCase(Vector2Int pos, Vector2Int initialPos, int maxCost, Token player)
	{
		CaseObject caseObject = null;
		if (!containByPos(caseList, pos) && (!CaseTaken(pos) || pos == initialPos) &&
			(pos.x <= maxX && pos.x >= minX && pos.y >= minY && pos.y <= maxY))//In the grid
		{
			if (CalculateCost(pos, initialPos) <= maxCost) {
				GameObject @case = Instantiate(data.caseObject,transform);
				Vector3 vector3 = V2ItoV3(pos);
				vector3.z = 10;
				@case.transform.position = vector3;

				caseObject = @case.GetComponent<CaseObject>();
				caseList.Add(caseObject);
				caseObject.moveCost = CalculateCost(pos, initialPos);
				caseObject.SetPlayer(player);
				caseObject.UpdateMaterial();
			}

		}
		return caseObject;
	}

	public int CalculateCost(Vector2Int posA, Vector2Int posB)
	{
		if (posA == posB) {
			return 0;
		}
		return Mathf.CeilToInt(Pathfinding.Instance.findPath(posA, posB).Count / 2);
	}
	public bool containByPos(List<CaseObject> listObj, Vector2 pos)
	{
		foreach (var item in listObj) {
			if (V3toV2I(item.transform.position) == pos) {
				return true;
			}
		}
		return false;
	}



	/// <summary>
	/// Move the object to the given position, relative to its position, if it's not taken
	/// </summary>
	/// <param name="item">the </param>
	/// <param name="position"></param>
	/// <returns></returns>
	public Sequence Move(Token item, List<Vector2Int> path)
	{
		Sequence sequence = DOTween.Sequence();
		foreach (var position in path) {
			if (dico.ContainsValue(position)) {
				Debug.LogWarning("Error: Current Position Already Taken!");
			} else {
				sequence.Append(item.transform.DOMove(V2ItoV3(position), data.moveDuration));
			}
		}
		dico.Remove(item);
		dico.Add(item, path[path.Count - 1]);// -1 because of segFault and -1 because last case is the player one
		return sequence;

	}

	/// <summary>
	/// Return if the case is taken by something
	/// </summary>
	/// <param name="pos"></param>
	/// <returns></returns>
	public bool CaseTaken(Vector2Int pos)
	{
		return dico.ContainsValue(pos);
	}

	/// <summary>
	/// Return if an object is found, and where it is if found
	/// </summary>
	/// <param name="item">The item to find</param>
	/// <returns></returns>
	public bool WhereIsObject(Token item)
	{
		return dico.ContainsKey(item);
	}

	/// <summary>
	/// Return the token by getting his position
	/// </summary>
	/// <param name="position"></param>
	/// <returns></returns>
	public Token TryGetTokenByPos(Vector2Int position)
	{
		foreach (var item in dico) {
			if (item.Value == position) {
				return item.Key;
			}
		}
		return null;
	}

	/// <summary>
	/// Return if an object is found, and where it is if found
	/// </summary>
	/// <param name="item">The item to find</param>
	/// <param name="position"> The Vector2 of the position to return</param>
	/// <returns></returns>
	public bool WhereIsToken(Token item, out Vector2Int position)
	{
		if (dico.TryGetValue(item, out position)) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Instantiate a new object at a given position if this position is free
	/// </summary>
	/// <param name="prefab">The GameObject you want to instantiate</param>
	/// <param name="position">The position on the grid</param>
	/// <returns>The game object instance</returns>
	public Token CreateObject(GameObject prefab, Vector2Int position)
	{
		if (dico.ContainsValue(position)) {
			Debug.LogError("Error: Can't create object, position already taken!");
			return null;
		}
		GameObject obj = Instantiate(prefab, transform);
		Token token = obj.GetComponent<Token>();
		dico.Add(token, position);
		obj.transform.position = V2ItoV3(position);
		Debug.Log("Object succefully created!");
		return token;
	}

	/// <summary>
	/// Delete the given GameObject from the game
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	public void DeleteObject(Token item)
	{
		dico.Remove(item);
		Destroy(item.gameObject);
	}

	/// <summary>
	/// Convert a Vector2Int in a Vector3
	/// </summary>
	/// <param name="vector"></param>
	/// <returns></returns>
	public Vector3 V2ItoV3(Vector2Int vector)
	{
		return new Vector3(vector.x, vector.y, 0);
	}

	/// <summary>
	/// Convert a Vector3 in a Vector2Int
	/// </summary>
	/// <param name="vector"></param>
	/// <returns></returns>
	public Vector2Int V3toV2I(Vector3 vector)
	{
		return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
	}
}
