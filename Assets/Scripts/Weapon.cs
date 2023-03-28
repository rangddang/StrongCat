using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Weapon : MonoBehaviour
{
	public float damage = 5f;
	[Header("Move")]
	public bool Move;
	public float speed = 20f;

	[Header("Knockback")]
	public bool Knockback;
	public float knockback = 5f;

	[Header("Penetration")]
	public bool Penetration;
	private int penetrationCount;
	public int penetrationMax;

	[Header("Destory")]
	public bool Destory;
	public float destoryTime;

	[Header("Stay")]
	public bool Stay;

	[Header("Bounce")]
	public bool Bounce;

	[Header("Chain")]
	public bool Chain;
	public int chain;
	public float slow = 2f;
	public GameObject spiderWeb;
	public List<Transform> chainTrans = new List<Transform>();
	public List<Vector3> chainPos = new List<Vector3>();
	private LineRenderer lineRenderer;

	private SummonEnemy summonEnemy;
	private Transform player;
	private Vector2 dir = Vector2.zero;
	private void Start()
	{
		player = FindObjectOfType<PlayerController>().transform;
		if (Destory)
		{
			Destroy(gameObject, destoryTime);
		}
		if (Bounce)
		{
			summonEnemy = FindObjectOfType<SummonEnemy>();
		}
		if (Chain)
		{
			summonEnemy = FindObjectOfType<SummonEnemy>();
		}
	}


	private void Update()
	{
		if (Move)
		{
			transform.Translate(Vector2.up * speed * Time.deltaTime);
		}
		if (Vector2.Distance(player.transform.position, transform.position) > 40f)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy") == true)
		{
			if (Penetration)
			{
				penetrationCount++;
			}
			if (Knockback)
			{
				dir = (collision.gameObject.transform.position - player.position).normalized;
			}
			BounceFunction();
			ChainFunction();
			collision.gameObject.GetComponent<Enemy>().Hit(damage, dir, knockback);
			
		}
		if (Penetration)
		{
			if (penetrationCount >= penetrationMax)
			{
				//speed = 0;
				Destroy(gameObject);
			}
		}
	}

	private void BounceFunction()
	{
		if (Bounce)
		{
			Vector3 newDir = Vector3.zero;
			if (summonEnemy.enemys.Count > 0 && summonEnemy.enemys[0] != null)
				newDir = summonEnemy.enemys[Random.Range(0, summonEnemy.enemys.Count)].position - transform.position;
			Quaternion rotate = Quaternion.LookRotation(forward: Vector3.forward, upwards: newDir);
			transform.rotation = rotate;
		}
	}

	private void ChainFunction()
	{
		if (Chain)
		{
			GameObject newWeb = Instantiate(spiderWeb, transform.position, Quaternion.identity);
			lineRenderer = newWeb.GetComponent<LineRenderer>();
			lineRenderer.positionCount = chain;
			Destroy(newWeb, 1);
			float distance;
			float currentDist;
			int targetIndex = -1;
			chainTrans[0] = transform;

			for (int i = 0; i < chain; i++)
			{
				distance = Mathf.Infinity;
				if(i != 0)
					chainPos.Add(chainTrans[i-1].position);
				else
					chainPos[i] = chainTrans[i].position;

				for (int j = 0; j < summonEnemy.enemys.Count; j++)
				{
					if (summonEnemy.enemys[j] != null)
					{
						int count = 0;
						for (int k = 0; k < i; k++)
							if (chainPos[k] == summonEnemy.enemys[j].position)
								count++;

						if(count == 0)
						{
							currentDist = Vector2.Distance(chainPos[i], summonEnemy.enemys[j].position);

							if (distance > currentDist)
							{
								targetIndex = j;
								distance = currentDist;
							}
						}
					}
				}
				if (i == 0)
				{
					chainTrans[0] = summonEnemy.enemys[targetIndex];
					chainPos[0] = chainTrans[0].position;
				}
				else
				{
					chainTrans.Add(summonEnemy.enemys[targetIndex]);
					chainPos[i] = chainTrans[i].position;
				}
				lineRenderer.SetPosition(i, chainPos[i]);
				chainTrans[i].gameObject.GetComponent<Enemy>().StartSlow(slow);
				chainTrans[i].gameObject.GetComponent<Enemy>().Hit(damage, dir, knockback);
			}
			
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (!Stay) return;

		if (collision.gameObject.CompareTag("Enemy") == true)
		{
			collision.gameObject.GetComponent<Enemy>().Hit(damage, dir, knockback);
		}
	}
}
