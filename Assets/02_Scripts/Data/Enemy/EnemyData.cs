using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string eName;
    public int level;
    public float atk;
    public float hpMax;
    public float hp;
    public float def;
    public float spd;
    public float atkSpd;
}
