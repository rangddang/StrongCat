using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
	public VariableJoystick joystick;

	private PlayerController player;
	private List<string> weapons = new List<string>();
	public List<Sprite> weaponImages = new List<Sprite>();
	private int[] upgrade = new int[3];
	private bool isPause = false;
	public Text[] buttonText = new Text[3];
	public Image[] buttonImage = new Image[3];
	public bool GameOver;
    public GameObject GameOverPanel;
	public GameObject PausePanel;
    public GameObject PauseButton;
	public GameObject LevelUpPanel;

	private float slowTime = 1;

	private void Start()
	{
		player = FindObjectOfType<PlayerController>();
		SetWeapons();
	}


	private void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Pause();
		}

		if (isPause)
		{
			joystick.gameObject.SetActive(false);
		}
		else
		{
			joystick.gameObject.SetActive(true);
		}

		if (GameOver)
        {
			isPause = true;
            PauseButton.SetActive(false);
            PausePanel.SetActive(false);
            GameOverPanel.SetActive(true);
            slowTime -= Time.deltaTime;
            if(slowTime > 0)
                Time.timeScale = slowTime;
        }
        else
        {
            GameOverPanel.SetActive(false);
        }
    }

    public void Pause()
    {
		isPause = true;
        PauseButton?.SetActive(false);
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continew()
    {
		isPause = false;
        PauseButton.SetActive(true);
		PausePanel.SetActive(false);
		Time.timeScale = 1;
	}


	public void ReStart()
    {
		slowTime = 1f;
        Time.timeScale = 1;
		SceneManager.LoadScene("InGame");
    }

	public void GoToMain()
	{
		slowTime = 1f;
		Time.timeScale = 1;
		SceneManager.LoadScene("Main");
	}

	public void Exit()
	{
		Application.Quit();
	}

	public void PlayerLevelUp()
	{
		isPause = true;
		Time.timeScale = 0f;
		PauseButton.SetActive(false);
		LevelUpPanel.SetActive(true);
	}


	public void GetItems()
	{
		isPause = false;
		player.levelupCount--;
		SetWeapons();
		player.SetItems();
		Time.timeScale = 1f;
		PauseButton.SetActive(true);
		LevelUpPanel.SetActive(false);
	}

	private void SetWeapons()
	{
		weapons.Clear();
		weapons.Add("총");
		weapons.Add("새");
		weapons.Add("마법지팡이");
		weapons.Add("번개");
		weapons.Add("파도");
		weapons.Add("로봇청소기");
		weapons.Add("거미줄");

		for (int i = 0; i < upgrade.Length; i++)
		{
			int rand = Random.Range(0, weapons.Count);
			upgrade[i] = GetIndexOfWeapons(rand);
			buttonText[i].text = "Lv." + player.weaponsLevel[GetIndexOfWeapons(rand)] + "->" + (player.weaponsLevel[GetIndexOfWeapons(rand)] + 1) + "\n" + weapons[rand];
			buttonImage[i].sprite = weaponImages[GetIndexOfWeapons(rand)];
			if(GetIndexOfWeapons(rand) == 4)
			{
				buttonImage[i].rectTransform.sizeDelta = new Vector2(150 * 1.4f, 150 * 0.8f);
			}
			else
			{
				buttonImage[i].rectTransform.sizeDelta = new Vector2(150, 150);
			}
			weapons.Remove(weapons[rand]);
		}
	}

	private int GetIndexOfWeapons(int rand)
	{
		int index = 0;
		switch (weapons[rand])
		{
			case "총":
				index = 0;
				break;
			case "새":
				index = 1;
				break;
			case "마법지팡이":
				index = 2;
				break;
			case "번개":
				index = 3;
				break;
			case "파도":
				index = 4;
				break;
			case "로봇청소기":
				index = 5;
				break;
			case "거미줄":
				index = 6;
				break;
		}
		return index;
	}

	public void GetItem1()
	{
		player.weaponsLevel[upgrade[0]]++;
	}

	public void GetItem2()
	{
		player.weaponsLevel[upgrade[1]]++;
	}

	public void GetItem3()
	{
		player.weaponsLevel[upgrade[2]]++;
	}

	public void GetItem4()
	{
		player.hp += player.maxHp * 0.4f;
		if(player.hp > player.maxHp)
			player.hp = player.maxHp;
	}
}
