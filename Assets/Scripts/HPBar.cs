using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private PlayerController player;
    public Image hpBar;
    public Image hp;
	public Image hpLerp;


	void Start()
    {
		player = FindObjectOfType<PlayerController>();
	}

    
    void Update()
    {
        hp.fillAmount = player.hp/player.maxHp;
        hpLerp.fillAmount = Mathf.Lerp(hpLerp.fillAmount, player.hp/player.maxHp, Time.deltaTime * 5f);

        hpBar.rectTransform.position = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(0, 1.5f, 0));
	}
}
