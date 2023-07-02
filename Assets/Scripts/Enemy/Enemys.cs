using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Enemys
{
	public string name;
    public List<Enemy> enemy = new List<Enemy>();

	public Enemys(string name, List<Enemy> enemy)
	{
		this.name = name;
		this.enemy = enemy;
	}
}
