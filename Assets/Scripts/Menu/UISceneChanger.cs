using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISceneChanger : MonoBehaviour
{
	public string SceneName;

	public void ChangeScene()
	{
		SceneManager.LoadScene(SceneName,LoadSceneMode.Single);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
