using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap : MonoBehaviour
{
    private Transform player;
    private float x;
    private float y;
    private float sX;
    private float sY;
    public float mX = 3;
    public float mY = 3;

    
    void Start()
    {
		player = FindObjectOfType<PlayerController>().transform;
	}

    
    void Update()
    {
        x = player.position.x - sX;
        y = player.position.y - sY;

        if(x > mX)
        {
            sX += mX;
        }
		else if (x < -mX)
		{
			sX -= mX;
		}
		if (y > mY)
		{
            sY += mY;
		}
		else if (y < -mY)
		{
			sY -= mY;
		}

        transform.position = new Vector3(sX, sY, 0);
	}
}
