using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseItems : MonoBehaviour
{
    public List<Text> weaponsText = new List<Text>();
    private PlayerController player;

    private void Awake()
    {
		player = FindObjectOfType<PlayerController>();
	}

    private void Update()
    {
        for(int i = 0; i < 7; i++)
        {
            weaponsText[i].text = "Lv." + player.weaponsLevel[i];
        }
    }
}
