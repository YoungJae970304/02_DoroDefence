using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    // �÷��̾��� ���¸� �ľ��ϱ� ���� ����ϴ� ������ ������
    Idle = 0,
    Move,
    Attack,
    Hit,
    Die,
}

public class Player : MonoBehaviour
{
    // ScriptableObject�� PlayerData�� ����ϱ� ���� ������ ����
    public PlayerData pData;

    // pData�� ���������� ������ ����
    public PlayerData pDataInstance;

    public GameObject arrow, magicB;

    // ���������� ���� ���¸� ����ϱ� ���� ���� ����
    PlayerState playerState;

    Animator anim;
    Rigidbody2D rig;
    SpriteRenderer sr;

    // ���� ���¸� �ľ��ϱ� ���� bool ����
    bool isAttacking = false;       //�� ���� ���ݻ���
    bool isBaseAttacking = false;    //�� ���� ���ݻ���
    //bool isHitting = false;         // �ǰ� ����

    float damage;
    //float previousHP;

    // �浹�� ���� ������� List
    List<GameObject> enemys = new List<GameObject>();

    // ĳ���� �̸��� ���� ���� �ʱ�ȭ, �ִϸ��̼ǰ� ������ٵ� �ʱ�ȭ
    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        rig = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();

        if (pData != null)
        {
            // pData�� ���ο� �ν��Ͻ��� ������ pDataInstance�� ����
            pDataInstance = ScriptableObject.CreateInstance<PlayerData>();

            // pData�� �����͸� pDataInstance�� ����
            pDataInstance.pName = pData.pName;
            pDataInstance.level = pData.level;
            pDataInstance.atk = pData.atk;
            pDataInstance.hpMax = pData.hpMax;
            pDataInstance.hp = pData.hp;
            pDataInstance.def = pData.def;
            pDataInstance.spd = pData.spd;
            pDataInstance.atkSpd = pData.atkSpd;

            // �̸��� ���� ���� �ʱ�ȭ
            switch (pData.pName)
            {
                case "����":
                    pDataInstance.atk = 10 + pData.level;
                    pDataInstance.hpMax = 200 + pData.level * 15;
                    pDataInstance.hp = pData.hpMax;
                    pDataInstance.def = 4 + pData.level;
                    pDataInstance.spd = 2f;
                    pDataInstance.atkSpd = 1.5f;
                    break;

                case "����":
                    //pDataInstance.atk = 5 + pData.level;
                    pDataInstance.atk = 0;
                    pDataInstance.hpMax = 400 + pData.level * 50;
                    pDataInstance.hp = pData.hpMax;
                    pDataInstance.def = 7 + pData.level;
                    pDataInstance.spd = 1;
                    pDataInstance.atkSpd = 4f;
                    break;

                case "�ü�":
                    pDataInstance.atk = 15 + pData.level;
                    pDataInstance.hpMax = 70 + pData.level * 10;
                    pDataInstance.hp = pData.hpMax;
                    pDataInstance.def = 2 + pData.level;
                    pDataInstance.spd = 1.5f;
                    pDataInstance.atkSpd = 1.5f;
                    break;

                case "������":
                    pDataInstance.atk = 10f + pData.level;
                    pDataInstance.hpMax = 70 + pData.level * 10;
                    pDataInstance.hp = pData.hpMax;
                    pDataInstance.def = 2 + pData.level;
                    pDataInstance.spd = 1.5f;
                    pDataInstance.atkSpd = 3f;
                    break;
            }
        }
    }
    private void Start()
    {
        // �����Ǹ� �켱�� �ٷ� �̵�
        ChangeState(PlayerState.Move);
    }

    // ������ �̸��� ���� ���� ���¸� �ٲٰ� �ش� ������ �ڷ�ƾ�� �����ϴ� �Լ�
    public void ChangeState(PlayerState newState)
    {
        // �÷��̾��� ���� ���°� ���� ���°� �ƴҶ� ���� ( ��, ���°� ���ϸ� ���� )
        if (playerState != newState)
        {
            // ���� ���� �ڷ�ƾ ����
            StopCoroutine(playerState.ToString());  
            //������ �ڿ� ToString()�� ����ϸ� 0,1,2�� �ƴ� �ش� �̸����� ��ȯ��

            // ���ο� ���·� ����
            playerState = newState;

            // ���� ������ �ڷ�ƾ ����
            StartCoroutine(playerState.ToString());
        }
    }

    private void Update()
    {
        // ���� ���¸� �ǽð����� �Ǵ��ϱ� ���� Update���� ���
        if (pDataInstance.hp <= 0)
        {
            // ���� hp�� 0�����϶� Die�� ���� ��ȯ
            ChangeState(PlayerState.Die);
        }
        else if ((isAttacking || isBaseAttacking))
        {
            // ���� �������϶� Attack�� ���� ��ȯ
            ChangeState(PlayerState.Attack);
        }
        else
        {
            // �� �ܿ��� �̵�
            ChangeState(PlayerState.Move);
        }
    }
    
    // �� ������ ���� �̵��ϴ� �ڷ�ƾ
    IEnumerator Move()
    {
        // �ش� ������ �ӵ��� ���������� �̵�
        rig.velocity = Vector2.right * pDataInstance.spd;
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

            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f 
            && !anim.IsInTransition(0));
            switch (pData.pName)
            {
                case "����":
                    SingleDamage();
                    break;
                case "����":
                    SingleDamage();
                    break;
                case "�ü�":
                    // ����ü ����, ����ü�� ������ ����� ��� ����ü�� ���ְ� ������
                    ArrowAtk();
                    break;
                case "������":
                    Magic();
                    AreaDamage();
                    break;
            }
            yield return new WaitForSeconds(pDataInstance.atkSpd);
        }
    }

    public void ArrowAtk()
    {
        Vector2 pos = transform.position;
        pos.y += 0.2f;
        GameObject arrows = Instantiate(arrow, pos, Quaternion.identity);

        StartCoroutine(ArrowDes(arrows));
    }
    IEnumerator ArrowDes(GameObject arrow)
    {
        if (isAttacking)
        {
            Transform enemyTransform = enemys[0].transform; // enemys �迭�� �� ĳ������ ����Ʈ��� ����
            while (arrow != null && enemyTransform != null)
            {
                // ���� ȭ��� �� ĳ���� ���� �Ÿ� ���
                float distance = Vector2.Distance(arrow.transform.position, enemyTransform.position);
                // ���� �Ÿ� ���Ϸ� �̵����� �� ȭ�� �ı�
                if (distance <= 0.6f)
                {
                    SingleDamage();
                    Destroy(arrow);
                    yield break; // �ڷ�ƾ ����
                }
                // ȭ���� �� ĳ���� �������� ���� �ӵ��� �̵���Ű��
                Vector2 movArrow = Vector2.MoveTowards(arrow.transform.position, 
                            enemyTransform.position + new Vector3(1f, 0.5f), 0.05f);
                arrow.transform.position = movArrow;
                yield return null; // ���� �����ӱ��� ��ٸ�
            }
        }
        if (isBaseAttacking)
        {
            Base ebaseC = GameManager.gm.ebase.GetComponent<Base>();
            while (arrow != null && ebaseC != null)
            {
                // ���� ȭ��� �� ĳ���� ���� �Ÿ� ���
                float distance = Vector2.Distance(arrow.transform.position, 
                                    ebaseC.gameObject.transform.position);
                // ���� �Ÿ� ���Ϸ� �̵����� �� ȭ�� �ı�
                if (distance <= 0.6f)
                {
                    SingleDamage();
                    Destroy(arrow);
                    yield break; // �ڷ�ƾ ����
                }
                // ȭ���� �� ĳ���� �������� ���� �ӵ��� �̵���Ű��
                Vector2 movArrow = Vector2.MoveTowards(arrow.transform.position, 
                                    ebaseC.gameObject.transform.position, 0.05f);
                arrow.transform.position = movArrow;
                yield return null; // ���� �����ӱ��� ��ٸ�
            }  
        }
    }

    public void Magic()
    {
        if (enemys.Count != 0)
        {
            Vector2 pos = enemys[0].transform.position;
            GameObject magic = Instantiate(magicB, pos, Quaternion.identity);
            StartCoroutine(MagicDes(magic));
        }
        else if (enemys.Count == 0)
        {
            Base ebaseC = GameManager.gm.ebase.GetComponent<Base>();
            Vector2 pos = ebaseC.transform.position;
            pos.x -= 1f;
            GameObject magic = Instantiate(magicB, pos, Quaternion.identity);
            StartCoroutine(MagicDes(magic));
        }
    }

    IEnumerator MagicDes(GameObject paticleOb)
    {
        paticleOb.SetActive(true);
        ParticleSystem particleSystem = paticleOb.GetComponent<ParticleSystem>();

        yield return new WaitUntil(() => !particleSystem.isPlaying);
        Destroy(paticleOb);
    }

    public void SingleDamage()
    {
        if (isBaseAttacking)
        {
            Base ebaseC = GameManager.gm.ebase.GetComponent<Base>();
            damage = pDataInstance.atk - ebaseC.eData.def;
            if (damage <= 0)
            {
                damage = 0;
            }
            ebaseC.eData.hp -= damage;
        }
        if (isAttacking && enemys.Count > 0)
        {
            damage = pDataInstance.atk - enemys[0].GetComponent<Enemy>().eDataInstance.def;
            if (damage <= 0)
            {
                damage = 0;
            }
            enemys[0].GetComponent<Enemy>().eDataInstance.hp -= damage;
        }
    }

    public void AreaDamage()
    {
        if (isBaseAttacking)
        {
            Base ebaseC = GameManager.gm.ebase.GetComponent<Base>();
            damage = pDataInstance.atk - ebaseC.eData.def;
            if (damage <= 0)
            {
                damage = 0;
            }
            ebaseC.eData.hp -= damage;
        }
        if (isAttacking)
        {
            foreach (GameObject enemy in enemys)
            {
                damage = pDataInstance.atk - enemy.GetComponent<Enemy>().eDataInstance.def;
                if (damage <= 0)
                {
                    damage = 0;
                }
                enemy.GetComponent<Enemy>().eDataInstance.hp -= damage;
            }
        }
    }

    /*
    IEnumerator Hit()
    {
        isHitting = true;   
        // bool ������ �������ַ� ���� ���� �ش� �ڷ�ƾ�� ���������� �۵��ǳ� ���Ŀ� ������ ���ϰ� �׳� ���� �Ѿ����
        Debug.Log("�¾���");
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        // �ȵǴ� ������ �� �ڷ�ƾ�� �����ϴ� ���� �ٸ� �ڷ�ƾ�� ����Ǿ� �� �ڷ�ƾ�� �ߴܵ�.
        //yield return new WaitForEndOfFrame(); �̰ŷ� �ϸ� ������ �ǳ� �ʹ� ���� ������ ������ ����
        sr.color = Color.red;
        Debug.Log("���� �������� ����");
        isHitting = false;
    }
    */

    IEnumerator Die()
    {
        rig.velocity = Vector2.zero;
        anim.SetTrigger("doDie");
        yield return new WaitForSeconds(0.2f);  // �����ð��� ���� ������ ����� ��ٸ��� �ʰ� �ٷ� �װԵ�
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemys.Add(collision.gameObject);

            isAttacking = true;
        }
        else if (collision.gameObject.CompareTag("EnemyBase"))
        {
            //enemys.Add(collision.gameObject);

            isBaseAttacking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemys.Remove(collision.gameObject);

            if (enemys.Count == 0)
            {
                isAttacking = false;
            }
        }

        else if (collision.gameObject.CompareTag("EnemyBase"))
        {
            //enemys.Remove(collision.gameObject);

            isBaseAttacking = false;
        }
    }
}
