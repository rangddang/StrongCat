using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void BeforeSceneLoad()
	{
		Application.targetFrameRate = 60;
		QualitySettings.vSyncCount = 0;
	}

	public void GameStart()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("InGame");
	}

	public void Exit()
	{
		Application.Quit();
	}
}
