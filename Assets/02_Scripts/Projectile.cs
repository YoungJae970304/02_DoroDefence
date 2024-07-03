using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Base playerBase;

    List<GameObject> enemys = new List<GameObject>();

    private void Awake()
    {
        playerBase = GameObject.Find("PlayerBase").GetComponent<Base>();
    }

    void Start()
    {
        if (gameObject.name.Contains("Arrow"))
        {
            Destroy(gameObject, 1.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && gameObject.name.Contains("ArrowRed"))
        {
            enemys.Add(collision.gameObject);
            if (enemys != null)
            {
                Enemy enemy = enemys[0].gameObject.GetComponent<Enemy>();
                float arrowDamage = playerBase.pData.atk - enemy.eDataInstance.def;
                enemy.eDataInstance.hp -= arrowDamage;

                // 화살을 파괴하여 추가 충돌을 방지
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("EnemyBase") && gameObject.name.Contains("ArrowRed"))
        {
            enemys.Add(collision.gameObject);
            if (enemys != null)
            {
                Base enemy = enemys[0].gameObject.GetComponent<Base>();
                float arrowDamage = playerBase.pData.atk - enemy.eData.def;
                enemy.eData.hp -= arrowDamage;

                // 화살을 파괴하여 추가 충돌을 방지
                Destroy(gameObject);
            }
        }

        else if (collision.gameObject.CompareTag("Enemy") && gameObject.name.Contains("MagicRed"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                float magicDamage = playerBase.pData.atk - enemy.eDataInstance.def - 10;
                enemy.eDataInstance.hp -= magicDamage;
            }
        }

        else if (collision.gameObject.CompareTag("EnemyBase") && gameObject.name.Contains("MagicRed"))
        {
            Base enemy = collision.gameObject.GetComponent<Base>();
            if (enemy != null)
            {
                float magicDamage = playerBase.pData.atk - enemy.eData.def - 10;
                enemy.eData.hp -= magicDamage;
            }
        }
        /*
        if (collision.gameObject.CompareTag("Enemy") && gameObject.name.Contains("ArrowRed"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                float arrowDamage = playerBase.pData.atk - enemy.eData.def;
                enemy.eDataInstance.hp -= arrowDamage;

                // 화살을 파괴하여 추가 충돌을 방지
                Destroy(gameObject);
            }
        }

        else if (collision.gameObject.CompareTag("Enemy") && gameObject.name.Contains("MagicRed"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                float magicDamage = playerBase.pData.atk - enemy.eData.def - 10;
                enemy.eData.hp -= magicDamage;
            }
        }
        */
    }
}
