using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Base : MonoBehaviour
{
    public PlayerData pData;
    public EnemyData eData;

    public UIManager um;

    private void Awake()
    {
        if (pData != null && pData.pName == "기지")
        {
            if ( pData.hp <= 0)
            {
                eData.level -= 2;
            }
            pData.hpMax = 1000 + pData.level * 200;
            pData.hp = pData.hpMax;
            pData.def = 10f + pData.level;
            pData.atk = 40 + pData.level * 5;
        }
        
        if (eData != null && eData.eName == "적기지")
        {
            if (eData.hp <= 0)
            {
                eData.level++;
            }
            eData.hpMax = 1000 + eData.level * 200;
            eData.hp = eData.hpMax;
            eData.def = 0f * eData.level;
        }
    }

    private void Update()
    {
        if (pData.hp <= 0)
        {
            GameManager.gm.isPlaying = false; 
            um.End_UI_Window(um.endWindow, um.txtResult, "패배");
        }
        else if (eData.hp <= 0)
        {
            GameManager.gm.isPlaying = false;
            um.End_UI_Window(um.endWindow, um.txtResult, "승리");
        }
    }
}
