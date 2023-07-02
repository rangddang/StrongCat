using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    private PlayerController player;
    public bool isTargeting;
	private Animator animator;
	public bool expOn;
	public Vector2 randVec;

	[Range(1, 3)]
	public int exp;
	public float speed = 7.5f;

    void Start()
    {
		player = FindObjectOfType<PlayerController>();
		animator = GetComponent<Animator>();
		animator.enabled = false;
		expOn = true;
		randVec = (Vector2)transform.position + (new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 2f);
	}

    private void Update()
    {
		if (expOn)
		{
			transform.position = Vector2.Lerp(transform.position, randVec, 5f * Time.deltaTime);
			if (Vector2.Distance(transform.position, randVec) > 0.2f)
				return;
			else
				expOn = false;
		}
        if (Vector2.Distance(transform.position, player.transform.position) < player.magnetSize && !isTargeting)
        {
            isTargeting = true;
			randVec = (Vector2)transform.position + (Vector2)((transform.position - player.transform.position)).normalized * 0.5f;
			expOn = true;


		}
        if (isTargeting)
        {
			Magnet();
		}
    }

    private void Magnet()
    {

        transform.position = Vector2.Lerp(transform.position, player.transform.position, speed * Time.deltaTime);


        if(Vector2.Distance(transform.position, player.transform.position) < 1f)
        {
			if (exp == 1)
			{
				player.exp += 2;
			}
			else if (exp == 2)
			{
				player.exp += 10;
			}
			else if (exp == 3)
			{
				player.exp += 50;
			}
			Destroy(gameObject);
		}
    }

	private void OnBecameVisible()
	{
		animator.enabled = true;
	}

	private void OnBecameInvisible()
	{
		animator.enabled = false;
	}
}
