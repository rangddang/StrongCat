//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;
//using Unity.VisualScripting;
////using Unity.Mathematics;
//using UnityEngine;

//public class Test : MonoBehaviour
//{
//	public GameObject[] exp;
//	public GameObject hitDamageText;

//	private SummonEnemy summonEnemy;
//	private Transform target;
//	private SpriteRenderer spriteRenderer;
//	private Collider2D collider2d;
//	private Animator animator;


//	public float currentSpeed = 1.0f;
//	public float damage = 5;
//	public float knockbackResistance;
//	public bool Jump = false;
//	public float jumpSpeed;
//	public float hp = 10;
//	public float damageDelay = 0.5f;
//	[SerializeField] bool isBoss;

//	private bool isDead;
//	private float countTime;
//	private float slowSize;
//	private float saveSpeed;

//	private void Awake()
//	{
//		spriteRenderer = GetComponent<SpriteRenderer>();
//		collider2d = GetComponent<Collider2D>();
//		animator = GetComponent<Animator>();
//		saveSpeed = currentSpeed;
//	}

//	private void Start()
//	{
//		target = FindObjectOfType<PlayerController>().transform;
//		summonEnemy = FindObjectOfType<SummonEnemy>();
//		animator.enabled = false;
//		if (Jump)
//		{
//			StartCoroutine(StartJump());
//		}
//	}

//	private void Update()
//	{
//		if (hp <= 0 && !isDead)
//		{
//			StartCoroutine(Dead());
//			isDead = true;
//		}
//		if (!isDead)
//		{
//			Movement();
//		}
//		if (Vector2.Distance(target.position, transform.position) > 40f && !isBoss)
//		{
//			if (summonEnemy.enemys.Count > 1)
//			{
//				summonEnemy.enemys.Remove(transform);
//			}
//			else
//			{
//				summonEnemy.enemys[0] = null;
//			}
//			summonEnemy.enemyNumber--;

//			Destroy(gameObject);
//		}
//		else if (!isDead)
//		{
//			collider2d.enabled = true;
//		}
//	}

//	private void Movement()
//	{
//		transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
//		spriteRenderer.flipX = target.position.x > transform.position.x;
//	}

//	public void Hit(float damageReceived, Vector3 knockbackDir, float knockback)
//	{
//		hp -= damageReceived;
//		transform.position += knockbackDir * (knockback / (1 + knockbackResistance));
//		GameObject damageText = Instantiate(hitDamageText, transform.position, Quaternion.identity);
//		damageText.GetComponent<DamageText>().damage = damageReceived;
//	}

//	public void StartSlow(float slow)
//	{
//		slowSize = slow;
//		StopCoroutine("Slow");
//		StartCoroutine("Slow");
//	}

//	private IEnumerator Slow()
//	{
//		float slowSpeed = currentSpeed / slowSize;

//		currentSpeed /= slowSpeed;
//		yield return new WaitForSeconds(1f);
//		currentSpeed = saveSpeed;
//	}

//	private IEnumerator Dead()
//	{
//		animator.enabled = false;
//		collider2d.enabled = false;
//		currentSpeed = 0;
//		transform.rotation = Quaternion.Euler(0, 0, 90);
//		yield return new WaitForSeconds(0.4f);
//		int rand = Random.Range(0, exp.Length);
//		if (exp[rand] != null)
//			Instantiate(exp[rand], transform.position, Quaternion.identity);
//		if (isBoss)
//		{
//			for (int i = 0; i < 40; i++)
//			{
//				rand = Random.Range(0, exp.Length);
//				if (exp[rand] != null)
//					Instantiate(exp[rand], transform.position, Quaternion.identity);
//			}
//		}
//		if (summonEnemy.enemys.Count > 1)
//		{
//			summonEnemy.enemys.Remove(transform);
//		}
//		else
//		{
//			summonEnemy.enemys[0] = null;
//		}
//		summonEnemy.enemyNumber--;

//		Destroy(gameObject);
//	}

//	private IEnumerator StartJump()
//	{
//		while (true)
//		{
//			yield return new WaitForSeconds(5f);
//			//rigid.velocity = (target.position - transform.position).normalized * jumpSpeed;
//		}
//	}

//	private void MoveTo(Vector3 dir)
//	{
//		StopCoroutine("MoveToCo");
//		StartCoroutine("MoveToCo");
//	}

//	private IEnumerator MoveToCo()
//	{
//		while (true)
//		{
//			//if ()
//			{

//			}
//			yield return null;
//		}
//	}

//	private void OnCollisionEnter2D(Collision2D collision)
//	{
//		if (collision.gameObject.CompareTag("Player") != true) return;

//		countTime = damageDelay;
//	}

//	private void OnCollisionStay2D(Collision2D collision)
//	{
//		if (collision.gameObject.CompareTag("Player") != true) return;

//		countTime += Time.deltaTime;

//		if (countTime >= damageDelay)
//		{
//			countTime -= damageDelay;
//			collision.gameObject.GetComponent<PlayerController>().hp -= damage;
//		}
//	}

//	private void OnCollisionExit2D(Collision2D collision)
//	{
//		if (collision.gameObject.CompareTag("Player") != true) return;

//		countTime = 0;
//	}

//	private void OnBecameVisible()
//	{
//		animator.enabled = true;
//	}

//	private void OnBecameInvisible()
//	{
//		animator.enabled = false;
//	}

//}
