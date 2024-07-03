using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public enum EnemyState
{
    // �÷��̾��� ���¸� �ľ��ϱ� ���� ����ϴ� ������ ������
    Idle = 0,
    Move,
    Attack,
    Hit,
    Die,
}

public class Enemy : MonoBehaviour
{
    public EnemyData eData, eBase;

    public EnemyData eDataInstance;

    public GameObject mold;

    // ���������� ���� ���¸� ����ϱ� ���� ���� ����
    EnemyState enemyState;

    Animator anim;
    Rigidbody2D rig;

    // ���� ���¸� �ľ��ϱ� ���� bool ����
    bool isAttacking = false;       //�� ���� ���ݻ���
    bool isBaseAttacking = false;    //�� ���� ���ݻ���

    float damage;

    // �浹�� ���� ������� List
    List<GameObject> players = new List<GameObject>();

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        rig = this.GetComponent<Rigidbody2D>();

        if (eData != null)
        {
            // eData�� ���ο� �ν��Ͻ��� ������ eDataInstance�� ����
            eDataInstance = ScriptableObject.CreateInstance<EnemyData>();

            eData.level = eBase.level;
            eDataInstance.name = eData.eName;
            eDataInstance.level = eData.level;
            eDataInstance.atk = eData.atk;
            eDataInstance.hpMax = eData.hpMax;
            eDataInstance.hp = eData.hp;
            eDataInstance.def = eData.def;
            eDataInstance.spd = eData.spd;
            eDataInstance.atkSpd = eData.atkSpd;

            switch (eData.eName)
            {
                case "���η�":
                    eDataInstance.atk = 10 + eData.level;
                    eDataInstance.hpMax = 50 + eData.level * 15;
                    eDataInstance.hp = eData.hpMax;
                    eDataInstance.def = 2 + eData.level;
                    eDataInstance.spd = 3f;
                    eDataInstance.atkSpd = 1f;
                    break;

                case "�巹��ũ":
                    eDataInstance.atk = 50 + eData.level;
                    eDataInstance.hpMax = 200 + eData.level * 20;
                    eDataInstance.hp = eData.hpMax;
                    eDataInstance.def = 5 + eData.level;
                    eDataInstance.spd = 2.5f;
                    eDataInstance.atkSpd = 2f;
                    break;

                case "�ƽ���":
                    eDataInstance.atk = 75 + eData.level;
                    eDataInstance.hpMax = 250 + eData.level * 30;
                    eDataInstance.hp = eData.hpMax;
                    eDataInstance.def = 7.5f + eData.level;
                    eDataInstance.spd = 2f;
                    eDataInstance.atkSpd = 2.5f;
                    break;

                case "���ö�":
                    eDataInstance.atk = 100 + eData.level;
                    eDataInstance.hpMax = 300 + eData.level * 40;
                    eDataInstance.hp = eData.hpMax;
                    eDataInstance.def = 10 + eData.level;
                    eDataInstance.spd = 1.5f;
                    eDataInstance.atkSpd = 3f;
                    break;
            }
        }
    }

    // ������ �̸��� ���� ���� ���¸� �ٲٰ� �ش� ������ �ڷ�ƾ�� �����ϴ� �Լ� ( FSM )
    public void ChangeState(EnemyState newState)
    {
        // �÷��̾��� ���� ���°� ���� ���°� �ƴҶ� ���� ( ��, ���°� ���ϸ� ���� )
        if (enemyState != newState)
        {
            // ���� ���� �ڷ�ƾ ����
            StopCoroutine(enemyState.ToString());
            //������ �ڿ� ToString()�� ����ϸ� 0,1,2�� �ƴ� �ش� �̸����� ��ȯ��

            // ���ο� ���·� ����
            enemyState = newState;

            // ���� ������ �ڷ�ƾ ����
            StartCoroutine(enemyState.ToString());
        }
    }

    private void Start()
    {
        // �����Ǹ� �켱�� �ٷ� �̵�
        ChangeState(EnemyState.Move);
    }

    private void Update()
    {
        // ���� ���¸� �ǽð����� �Ǵ��ϱ� ���� Update���� ���
        if (eDataInstance.hp <= 0)
        {
            // ���� hp�� 0�����϶� Die�� ���� ��ȯ
            ChangeState(EnemyState.Die);
        }
        else if (isAttacking || isBaseAttacking)
        {
            // ���� �������϶� Attack�� ���� ��ȯ
            ChangeState(EnemyState.Attack);
        }
        else
        {
            // �� �ܿ��� �̵�
            ChangeState(EnemyState.Move);
        }
        //���� �� ���� �ִϸ��̼��� �����ٸ� ChangeState(PlayerState.Hit)
    }

    // �� ������ ���� �̵��ϴ� �ڷ�ƾ
    IEnumerator Move()
    {
        // �ش� ������ �ӵ��� ���������� �̵�
        rig.velocity = Vector2.left * eDataInstance.spd;
        anim.SetTrigger("doMove");
        while (true)
        {
            yield return null;
        }
    }

    // ���� ������ �����ϴ� �ڷ�ƾ
    IEnumerator Attack()
    {
        rig.velocity = Vector2.zero;

        while (true)
        {
            anim.SetTrigger("doAttack");

            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !anim.IsInTransition(0));
            switch (eData.eName)
            {
                case "���η�":
                    SingleDamage();
                    break;
                case "�巹��ũ":
                    SingleDamage();
                    break;
                case "�ƽ���":
                    SingleDamage();
                    break;
                case "���ö�":
                    AreaDamage();
                    break;
            }
            yield return new WaitForSeconds(eDataInstance.atkSpd);
        }
    }

    public void SingleDamage()
    {
        if (isBaseAttacking)
        {
            Base pbaseC = GameManager.gm.pbase.GetComponent<Base>();

            damage = eDataInstance.atk - pbaseC.pData.def;

            if (damage <= 0)
            {
                damage = 0;
            }

            pbaseC.pData.hp -= damage;
        }

        if (isAttacking && players.Count > 0)
        {
            damage = eDataInstance.atk - players[0].GetComponent<Player>().pDataInstance.def;

            if (damage <= 0)
            {
                damage = 0;
            }

            players[0].GetComponent<Player>().pDataInstance.hp -= damage;
        }
    }

    public void AreaDamage()
    {
        if (isBaseAttacking)
        {
            Base pbaseC = GameManager.gm.pbase.GetComponent<Base>();

            damage = eDataInstance.atk - pbaseC.pData.def;

            if (damage <= 0)
            {
                damage = 0;
            }

            pbaseC.pData.hp -= damage;
        }


        if (isAttacking)
        {
            foreach (GameObject player in players)
            {
                damage = eDataInstance.atk - player.GetComponent<Player>().pDataInstance.def;

                if (damage <= 0)
                {
                    damage = 0;
                }

                player.GetComponent<Player>().pDataInstance.hp -= damage;
            }
        }
    }

    IEnumerator Hit()
    {
        anim.SetTrigger("doHit");
        while (true)
        {
            yield return null;
        }
    }

    IEnumerator Die()
    {
        rig.velocity = Vector2.zero;
        GameObject go = Instantiate(mold, this.transform.position + new Vector3(0.5f, 1.5f), Quaternion.identity);
        

        
        switch (eData.eName)
        {
            case "���η�":
                GameManager.gm.mold += 100;
                break;
            case "�巹��ũ":
                GameManager.gm.mold += 300;
                break;
            case "�ƽ���":
                GameManager.gm.mold += 900;
                break;
            case "���ö�":
                GameManager.gm.mold += 2700;
                break;
            default:
                Debug.Log(" �̸��� �� �ް� ���� ");
                break;
        }
        anim.SetTrigger("doDie");
        yield return new WaitForSeconds(0.2f);  // �����ð��� ���� ������ ����� ��ٸ��� �ʰ� �ٷ� �װԵ�
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            players.Add(collision.gameObject);

            isAttacking = true;
        }
        else if (collision.gameObject.CompareTag("PlayerBase"))
        {
            //players.Add(collision.gameObject);

            isBaseAttacking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            players.Remove(collision.gameObject);

            if (players.Count == 0)
            {
                isAttacking = false;
            }
        }

        else if (collision.gameObject.CompareTag("PlayerBase"))
        {
            //players.Remove(collision.gameObject);

            isBaseAttacking = false;
        }
    }
}
