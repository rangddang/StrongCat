using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
	private PlayerController player;
	private bool isTargeting;
	private bool comeon;
	private Vector2 randVec;
	private GameObject[] exps;

	public float speed = 7.5f;

	void Start()
	{
		player = FindObjectOfType<PlayerController>();
	}

	private void Update()
	{
		if (comeon)
		{
			transform.position = Vector2.Lerp(transform.position, randVec, 5f * Time.deltaTime);
			if (Vector2.Distance(transform.position, randVec) > 0.2f)
				return;
			else
				comeon = false;
		}
		if (Vector2.Distance(transform.position, player.transform.position) < player.magnetSize && !isTargeting)
		{
			isTargeting = true;
			randVec = (Vector2)transform.position + (Vector2)((transform.position - player.transform.position) * 0.5f);
			comeon = true;
		}
		if (isTargeting)
		{
			Magnetic();
		}
	}

	private void Magnetic()
	{

		transform.position = Vector2.Lerp(transform.position, player.transform.position, speed * Time.deltaTime);


		if (Vector2.Distance(transform.position, player.transform.position) < 1f)
		{
			exps = GameObject.FindGameObjectsWithTag("Exp");
			for(int i = 0; i < exps.Length; i++)
			{
				exps[i].GetComponent<Exp>().isTargeting = true;
				exps[i].GetComponent<Exp>().randVec = (Vector2)exps[i].transform.position + (Vector2)((exps[i].transform.position - player.transform.position)).normalized * 0.5f;
				exps[i].GetComponent<Exp>().expOn = true;
			}
			Destroy(gameObject);
		}
	}
}
