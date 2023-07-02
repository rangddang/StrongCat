using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] protected GameObject hitDamageText;
	protected Transform target;
	protected SummonEnemy summon;

	[Header("State")]
	[SerializeField] protected List<GameObject> items = new List<GameObject>();
	[SerializeField] protected float damage;
	[SerializeField] protected float hp;
	[SerializeField] protected float currentMoveSpeed;
	[SerializeField] protected float knockbackResistance;
	[SerializeField] protected float damageDelay;

	protected Animator anim;
	protected SpriteRenderer spriteRenderer;
	protected Collider2D collider;

	private float maxHp;
	protected float saveMoveSpeed;
	protected bool isDead = false;

	private Vector3 knockbackDir;
	private float knockbackPower;
	private float slowSize;

	private void Awake()
	{
		summon = FindObjectOfType<SummonEnemy>();
		target = GameObject.FindGameObjectWithTag("Player").transform;
		anim = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		collider = GetComponent<Collider2D>();
		maxHp = hp;
		saveMoveSpeed = currentMoveSpeed;
	}

	private void Start()
	{
		Respawn();
	}

	protected virtual void Update()
	{
		if (isDead) return;

		Move();
	}

	public void Respawn()
	{
		isDead = false;
		hp = maxHp;
		anim.enabled = true;
		collider.enabled = true;
	}

	private void Move()
	{
		transform.position += (target.position - transform.position).normalized * currentMoveSpeed * Time.deltaTime;
		spriteRenderer.flipX = target.position.x > transform.position.x;
		Pushback();
	}

	private void Pushback()
	{
		foreach (Enemys enemys in summon.enemys)
		{
			foreach(Enemy enemy in enemys.enemy)
			{

			}
		}

		//foreach (Enemys e in summon.enemys)
		//{
		//	for (int i = 0; i < summon.enemys.Length; i++)
		//	{
		//		if (e.enemy[i] == this) return;

		//		if (Vector3.Distance(transform.position, e.enemy[i].transform.position) < e.enemy[i].GetComponent<CapsuleCollider2D>().size.x * e.enemy[i].transform.localScale.x)
		//		{
		//			Knockback((e.enemy[i].transform.position - transform.position).normalized, 3f);
		//		}
		//	}
		//}
	}

	public void Hit(float hitDamage)
	{
		hp -= hitDamage;
		if(hp <= 0)
		{
			Dead();
		}
	}

	public void Knockback(Vector3 dir, float power)
	{
		knockbackDir = dir;
		knockbackPower = power;
		StopCoroutine("PushOut");
		StartCoroutine("PushOut");
	}

	public void Slow(float slow)
	{
		slowSize = slow;
		StopCoroutine("StartSlow");
		StartCoroutine("StartSlow");
	}

	protected void Dead()
	{
		isDead = true;
		hp = 0;
		anim.enabled = false;
		collider.enabled = false;
		DestoryObject();
	}

	private void DestoryObject()
	{
		gameObject.SetActive(false);
	}

	private IEnumerator StartSlow()
	{
		currentMoveSpeed = slowSize;
		yield return new WaitForSeconds(0);
		currentMoveSpeed = saveMoveSpeed;
	}

	private IEnumerator PushOut()
	{
		Vector3 saveDir = knockbackDir;
		float savePower = knockbackPower;

		Vector3 currentKnock = saveDir * savePower;

		while (true)
		{
			currentKnock = Vector3.Lerp(currentKnock, Vector3.zero, Time.deltaTime * 10);
			transform.position += currentKnock;
			if(Vector3.Distance(currentKnock, Vector3.zero) < 0.01f)
			{
				yield break;
			}
			yield return null;
		}
	}
}
