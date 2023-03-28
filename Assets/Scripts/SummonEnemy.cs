using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SummonEnemy : MonoBehaviour
{
    public GameObject[] summoner;
    public GameObject Boss;
	public Transform player;
	public float summonCoolDown = 1;
    private int TimeCount = 0;
    public int summonCount = 2;
    public float radius = 10;
    public int enemyNumber;
    public int numberOfEnemy = 1000;
    public List<Transform> enemys = new List<Transform>();

	private void Awake()
    {
		player = FindObjectOfType<PlayerController>().transform;
	}

    void Start()
    {
        StartCoroutine(Summon());
        StartCoroutine(SummonTime());
    }

    private IEnumerator Summon()
    {
        while (true)
        {
            //Debug.Log(enemyNumber);
            for (int i = 0; i < summonCount; i++)
            {
                if (numberOfEnemy > enemyNumber)
                {
                    enemyNumber++;
                    float rand = Random.Range(0f, 360f);
                    rand = Mathf.Deg2Rad * rand;
                    float x = radius * Mathf.Cos(rand);
                    float y = radius * Mathf.Sin(rand);
                    GameObject enemy = Instantiate(summoner[Random.Range(0, summoner.Length)], new Vector3(x, y, 0) + player.position, Quaternion.identity);
                    if (enemys[0] != null)
                    {
                        enemys.Add(enemy.transform);
                    }
                    else
                    {
                        enemys[0] = enemy.transform;

					}
					yield return new WaitForSeconds(0.0001f);
                }
			}
			yield return new WaitForSeconds(summonCoolDown);
		}
    }

    private IEnumerator SummonTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(60);
            summonCount++;
            TimeCount++;

            if (TimeCount % 5 == 0)
            {
                for (int i = 0; i < TimeCount/5; i++)
                {
                    enemyNumber++;
                    float rand = Random.Range(0f, 360f);
                    rand = Mathf.Deg2Rad * rand;
                    float x = radius * Mathf.Cos(rand);
                    float y = radius * Mathf.Sin(rand);
                    GameObject boss = Instantiate(Boss, new Vector3(x, y, 0) + player.position, Quaternion.identity);
                    boss.GetComponent<Enemy>().hp += ((TimeCount/5) - 1) * 500;
                    if (enemys[0] != null)
                    {
                        enemys.Add(boss.transform);
                    }
                    else
                    {
                        enemys[0] = boss.transform;
                    }
                    yield return new WaitForSeconds(0.0001f);
                }
                summonCount += 3;
            }
		}
    }
}
