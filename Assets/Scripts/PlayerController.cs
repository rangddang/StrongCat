using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Drawing;

public class PlayerController : MonoBehaviour
{
	private Animator anim;
	private SpriteRenderer spriteRenderer;
    private SummonEnemy summonEnemy;
	private GameManager gameManager;

	public float moveSpeed = 3.0f;
    private Vector3 dir;
	private Vector3 saveDir;

	public float magnetSize = 5f;
	//private float saveMagnet;

	public int[] weaponsLevel = new int[7];

	[Header("Gun")]
	public GameObject bullet;

	public int bulletSize = 1;
    public float gunDelay = 1f;
	[Range(0f, 360f)]
	public float bulletAngle = 10f;

	[Header("Bird")]
	public GameObject bird;
	public List<GameObject> birds;

	public int birdSize = 1;
	public float birdSpeed = 10f;
	public float birdRadius = 5f;
    private float birdDeg;
	private int birdCount;

	[Header("Wand")]
	public GameObject wand;

	public int wandSize = 1;
	public float wandDelay = 1f;

	[Header("Lightning")]
	public GameObject lightning;

	public int lightningSize = 1;
	public float lightningDelay = 1f;

	[Header("Wave")]
	public GameObject wave;

	public float waveSize = 1;
	public float waveDelay = 10f;

	[Header("RobotCleaner")]
	public GameObject robotCleaner;

	public int robotCleanerSize = 1;
	public float robotCleanerDelay = 5;

	[Header("Spider")]
	public GameObject spider;

	public int spiderSize = 1;
	public float spiderDelay = 1f;
	public int spiderChain = 4;

	[Header("Level")]
    public TextMeshProUGUI[] levelText;
    public Image expImage;
	public Image expBackImage;

	public int level = 1;
    public float maxExp = 10;
    public float exp = 0;
    public float addExp = 1.1f;

	public int levelupCount;
	private float levelupTime;

	[Header("Hp")]
	public float maxHp = 100;
    public float hp = 100;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
		summonEnemy = FindObjectOfType<SummonEnemy>();
		gameManager = FindObjectOfType<GameManager>();
	}

    private void Start()
    {
		SetItems();
	}


    private void Update()
    {
		if (hp <= 0)
		{
			gameManager.GameOver = true;
			return;
		}
		Move();
        PlayerLevelUI();
        Level();
		if (weaponsLevel[1] > 0 && birds[0] != null)
			Bird();
	}

    private void Move()
    {
		dir = new Vector3(gameManager.joystick.Horizontal, gameManager.joystick.Vertical, 0);
		//dir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;

		if (dir != Vector3.zero)
        {
			saveDir = dir;
            transform.position += dir * moveSpeed * Time.deltaTime;
            anim.SetBool("isRun", true);
        }
        else
        {
			anim.SetBool("isRun", false);
		}
        spriteRenderer.flipX = saveDir.x > 0;
    }

    private void Level()
    {
		levelupTime -= Time.deltaTime;
		for (int i = 0; i < levelText.Length; i++) {
			if (levelupTime > 0.5 && levelupCount > 0)
			{
				levelText[i].transform.localScale = Vector3.Lerp(levelText[i].transform.localScale, Vector3.one * 2.0f, 7.5f * Time.deltaTime);
			}
			else
			{
				levelText[i].transform.localScale = Vector3.Lerp(levelText[i].transform.localScale, Vector3.one, 15f * Time.deltaTime);
			}
		}
		if (exp >= maxExp)
		{
			level++;
			levelupCount++;
			exp -= maxExp;
			maxExp += addExp;
			addExp += 1.5f;
			levelupTime = 1;
		}
		else if (levelupTime <= 0 && levelupCount > 0)
		{
			gameManager.PlayerLevelUp();
		}
    }
	private void Bird()
	{
		birdDeg += Time.deltaTime * birdSpeed;
		if (birdDeg < 360)
		{
			for (int i = 0; i < birdSize; i++)
			{
				var rad = Mathf.Deg2Rad * (birdDeg + (i * (360 / birdSize)));
				var x = birdRadius * Mathf.Sin(rad);
				var y = birdRadius * Mathf.Cos(rad);
				birds[i].transform.position = transform.position + new Vector3(x, y);
				birds[i].transform.rotation = Quaternion.Euler(0, 0, (birdDeg + (i * (360 / birdSize))) * -1);
			}
		}
		else
		{
			birdDeg = 0;
		}
	}

    private void PlayerLevelUI()
    {
		for(int i = 0; i < levelText.Length; i++)
			levelText[i].text = "Lv." + level.ToString();
		expBackImage.fillAmount = exp/maxExp;
		expImage.fillAmount = Mathf.Lerp(expImage.fillAmount, exp/maxExp, Time.deltaTime * 5f);
    }


	private IEnumerator ShotGun()
    {
        Vector3 newDir;
		while (true)
        {
            newDir = saveDir;
			for (int i = 0; i < bulletSize; i++)
            {
                Quaternion rotate = Quaternion.LookRotation(forward: Vector3.forward, upwards: newDir);
                rotate = rotate * Quaternion.Euler(0, 0, Random.Range(-(bulletAngle / 2), (bulletAngle / 2)));
                GameObject newBullet = Instantiate(bullet, transform.position, rotate);
			}
            yield return new WaitForSeconds(gunDelay);
        }
	}

	private IEnumerator ShotWand()
	{
		Vector3 newDir = Vector3.zero;
		while (true)
		{
			for (int i = 0; i < wandSize; i++)
			{
				if (summonEnemy.enemys.Count == 1 && summonEnemy.enemys[0] == null) ;
				else
				{
					float distance = Mathf.Infinity;
					float currentDist;
					int targetIndex = -1;
					for (int j = 0; j < summonEnemy.enemys.Count; j++)
					{
						currentDist = Vector2.Distance(transform.position, summonEnemy.enemys[j].position);

						if (distance > currentDist)
						{
							targetIndex = j;
							distance = currentDist;
						}
					}

					newDir = summonEnemy.enemys[targetIndex].position - transform.position;
				}
				Quaternion rotate = Quaternion.LookRotation(forward: Vector3.forward, upwards: newDir);
				Instantiate(wand, transform.position, rotate);
				yield return new WaitForSeconds(wandDelay/5);

			}
			yield return new WaitForSeconds(wandDelay);
		}
	}

	private IEnumerator ShotLightning()
	{
		Vector3 pos = transform.position;
		while (true)
		{
			for (int i = 0; i < lightningSize; i++)
			{
				if (summonEnemy.enemys.Count == 1 && summonEnemy.enemys[0] == null) ;
				else
					pos = summonEnemy.enemys[Random.Range(0, summonEnemy.enemys.Count)].position;
				GameObject lightn = Instantiate(lightning, pos + (Vector3.down * -2.4f), Quaternion.identity);

				Destroy(lightn, 0.5f);
				yield return new WaitForSeconds(lightningDelay/50);
			}
			yield return new WaitForSeconds(lightningDelay);
		}
	}

	private IEnumerator ShotWave()
	{
		Vector3 newDir;
		while (true)
		{
			newDir = saveDir;
			Quaternion rotate = Quaternion.LookRotation(forward: Vector3.forward, upwards: newDir);
			GameObject newBullet = Instantiate(wave, transform.position, rotate);
			newBullet.transform.localScale = Vector3.one * waveSize;
			newBullet.GetComponent<Weapon>().damage = 10 + ((weaponsLevel[4] - 1) * 5);
			yield return new WaitForSeconds(waveDelay);
		}
	}

	private IEnumerator ShotRobotCleaner()
	{
		Vector3 newDir = Vector3.zero;
		while (true)
		{
			for (int i = 0; i < robotCleanerSize; i++)
			{
				if (summonEnemy.enemys.Count == 1 && summonEnemy.enemys[0] == null) ;
				else
					newDir = summonEnemy.enemys[Random.Range(0, summonEnemy.enemys.Count)].position - transform.position;
				Quaternion rotate = Quaternion.LookRotation(forward: Vector3.forward, upwards: newDir);
				Instantiate(robotCleaner, transform.position, rotate);
				yield return new WaitForSeconds(0.05f);
			}
			yield return new WaitForSeconds(robotCleanerDelay);
		}
	}

	private IEnumerator ShotSpider()
	{
		Vector3 newDir = Vector3.zero;
		while (true)
		{
			for (int i = 0; i < spiderSize; i++)
			{
				if (summonEnemy.enemys.Count == 1 && summonEnemy.enemys[0] == null) ;
				else
					newDir = summonEnemy.enemys[Random.Range(0, summonEnemy.enemys.Count)].position - transform.position;
				Quaternion rotate = Quaternion.LookRotation(forward: Vector3.forward, upwards: newDir);
				GameObject newSpider = Instantiate(spider, transform.position, rotate);
				newSpider.GetComponent<Weapon>().chain = spiderChain;
				yield return new WaitForSeconds(0.05f);
			}
			yield return new WaitForSeconds(spiderDelay);
		}
	}


	public void SetItems()
	{
		StopAllCoroutines();
		if (levelupCount > 0) return;

		if (weaponsLevel[0] > 0)
		{
			bulletSize = ((weaponsLevel[0] - 1) * 2) + 5;
			bulletAngle = ((weaponsLevel[0] - 1) * 5) + 30;
			gunDelay = 1 - ((weaponsLevel[0] - 1) * 0.025f);
			StartCoroutine(ShotGun());
		}
		if (weaponsLevel[1] > 0)
		{
			birdSize = weaponsLevel[1] + 1;
			birdRadius = ((weaponsLevel[1] - 1) * 0.15f) + 4;
			birdSpeed = -(((weaponsLevel[1]-1) * 5) + 160);
			for (; birdCount < birdSize; birdCount++)
			{
				if (birdCount == 0)
					birds[0] = Instantiate(bird, transform.position, Quaternion.identity);
				else
					birds.Add(Instantiate(bird, transform.position, Quaternion.identity));
				//birds[birdCount].GetComponent<SpriteRenderer>().color.r. = Random.Range(0, 10);
			}
		}
		if (weaponsLevel[2] > 0)
		{
			wandSize = weaponsLevel[2];
			wandDelay = 0.75f - ((weaponsLevel[2] - 1) * 0.0375f);
			StartCoroutine(ShotWand());
		}
		if (weaponsLevel[3] > 0)
		{
			lightningSize = weaponsLevel[3];
			lightningDelay = 10 - ((weaponsLevel[3] - 1) * 0.25f);
			StartCoroutine(ShotLightning());
		}
		if (weaponsLevel[4] > 0)
		{
			waveSize = 1 + (weaponsLevel[4] * 0.1f);
			StartCoroutine(ShotWave());
		}
		if (weaponsLevel[5] > 0)
		{
			robotCleanerSize = weaponsLevel[5];
			StartCoroutine(ShotRobotCleaner());
		}
		if (weaponsLevel[6] > 0)
		{
			spiderSize = weaponsLevel[6];
			spiderChain = 3 + (weaponsLevel[6] - 1);
			StartCoroutine(ShotSpider());
		}
	}
}
