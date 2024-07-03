using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    public string pName;
    public int level;
    public float atk;
    public float hpMax;
    public float hp;
    public float def;
    public float spd;
    public float atkSpd;
}
