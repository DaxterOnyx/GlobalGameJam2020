using UnityEngine;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
	 , IPointerClickHandler, IPointerEnterHandler
	 , IPointerExitHandler
{
	private static PauseManager _instance;
	private bool isPaused;

	public static PauseManager Instance
	{
		get {
			if (_instance == null)
				_instance = FindObjectOfType<PauseManager>();
			return _instance;
		}
	}


	private void Start()
	{
		Init();
	}

	// Start is called before the first frame update
	internal void Init()
	{
		_instance = this;
		if (gameObject.activeSelf && !isPaused) {
			gameObject.SetActive(false);
			isPaused = false;
		}
	}

	public void Pause()
	{
		isPaused = !isPaused;
		if (isPaused) {
			gameObject.SetActive(true);
			//Time.timeScale = 0;
		} else {
			gameObject.SetActive(false);
			//Time.timeScale = 1;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		eventData.Use();
		return;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		eventData.Use();
		return;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		eventData.Use();
		return;
	}

	private void OnMouseOver()
	{
		Debug.LogWarning("I'm overed");
	}
}
