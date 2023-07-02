using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEnemy : MonoBehaviour
{
    [SerializeField] private List<GameObject> summoner = new List<GameObject>();
    [SerializeField] private List<GameObject> boss = new List<GameObject>();

    [SerializeField] public List<Enemys> enemys;


}