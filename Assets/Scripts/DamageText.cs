using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float destoryTime = 1;
    private float countTime;
    public float damage;
    private TextMeshPro damageText;
    public int speed = 5;
    private Vector3 upPos;
    private bool isDown;
    private float waitSec;
    private float upSize = 2;

    private void Start()
    {
		damageText = GetComponent<TextMeshPro>();
		damageText.text = ((int)damage).ToString("");
        upPos = transform.position + Vector3.up * upSize + Vector3.right * Random.Range(-1f, 1f);
        waitSec = destoryTime - 0.2f;
        Destroy(gameObject, destoryTime);
    }

    private void Update()
    {
        countTime += Time.deltaTime;
        if (Vector3.Distance(transform.position, upPos) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, upPos, speed * Time.deltaTime);
        }
        else if(!isDown && countTime > waitSec)
        {
			upPos = transform.position + Vector3.down * upSize;
            isDown = true;
		}
    }

}
