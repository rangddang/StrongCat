using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
	public GameObject[] Items;
	public Transform player;
	public float spawnCoolDown = 1;
	public int spawnCount = 2;
	public float radius = 10;

	private void Awake()
	{
		player = GameObject.Find("Cat").GetComponent<Transform>();
	}

	private void Start()
    {
		StartCoroutine(Summon());
	}

	private IEnumerator Summon()
	{
		while (true)
		{
			//Debug.Log(enemyNumber);
			for (int i = 0; i < spawnCount; i++)
			{
					float rand = Random.Range(0f, 360f);
					rand = Mathf.Deg2Rad * rand;
					float x = radius * Mathf.Cos(rand);
					float y = radius * Mathf.Sin(rand);
					Instantiate(Items[Random.Range(0, Items.Length)], new Vector3(x, y, 0) + player.position, Quaternion.identity);
					yield return new WaitForSeconds(0.0001f);
			}
			yield return new WaitForSeconds(spawnCoolDown);
		}
	}
}
