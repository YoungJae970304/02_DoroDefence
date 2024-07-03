using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public enum EnemyState
{
    // 플레이어의 상태를 파악하기 위해 사용하는 열거형 변수들
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

    // 열거형으로 만든 상태를 사용하기 위해 변수 선언
    EnemyState enemyState;

    Animator anim;
    Rigidbody2D rig;

    // 공격 상태를 파악하기 위한 bool 변수
    bool isAttacking = false;       //적 유닛 공격상태
    bool isBaseAttacking = false;    //적 기지 공격상태

    float damage;

    // 충돌한 적을 담기위한 List
    List<GameObject> players = new List<GameObject>();

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        rig = this.GetComponent<Rigidbody2D>();

        if (eData != null)
        {
            // eData의 새로운 인스턴스를 생성해 eDataInstance에 저장
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
                case "도로롱":
                    eDataInstance.atk = 10 + eData.level;
                    eDataInstance.hpMax = 50 + eData.level * 15;
                    eDataInstance.hp = eData.hpMax;
                    eDataInstance.def = 2 + eData.level;
                    eDataInstance.spd = 3f;
                    eDataInstance.atkSpd = 1f;
                    break;

                case "드레이크":
                    eDataInstance.atk = 50 + eData.level;
                    eDataInstance.hpMax = 200 + eData.level * 20;
                    eDataInstance.hp = eData.hpMax;
                    eDataInstance.def = 5 + eData.level;
                    eDataInstance.spd = 2.5f;
                    eDataInstance.atkSpd = 2f;
                    break;

                case "맥스웰":
                    eDataInstance.atk = 75 + eData.level;
                    eDataInstance.hpMax = 250 + eData.level * 30;
                    eDataInstance.hp = eData.hpMax;
                    eDataInstance.def = 7.5f + eData.level;
                    eDataInstance.spd = 2f;
                    eDataInstance.atkSpd = 2.5f;
                    break;

                case "라플라스":
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

    // 열거형 이름에 따라 현재 상태를 바꾸고 해당 상태의 코루틴을 실행하는 함수 ( FSM )
    public void ChangeState(EnemyState newState)
    {
        // 플레이어의 직전 상태가 현재 상태가 아닐때 실행 ( 즉, 상태가 변하면 실행 )
        if (enemyState != newState)
        {
            // 이전 상태 코루틴 종료
            StopCoroutine(enemyState.ToString());
            //열거형 뒤에 ToString()을 사용하면 0,1,2가 아닌 해당 이름으로 반환됨

            // 새로운 상태로 변경
            enemyState = newState;

            // 현재 상태의 코루틴 실행
            StartCoroutine(enemyState.ToString());
        }
    }

    private void Start()
    {
        // 생성되면 우선은 바로 이동
        ChangeState(EnemyState.Move);
    }

    private void Update()
    {
        // 현재 상태를 실시간으로 판단하기 위해 Update에서 사용
        if (eDataInstance.hp <= 0)
        {
            // 현재 hp가 0이하일때 Die로 상태 변환
            ChangeState(EnemyState.Die);
        }
        else if (isAttacking || isBaseAttacking)
        {
            // 현재 공격중일때 Attack로 상태 변환
            ChangeState(EnemyState.Attack);
        }
        else
        {
            // 그 외에는 이동
            ChangeState(EnemyState.Move);
        }
        //만약 적 공격 애니메이션이 끝났다면 ChangeState(PlayerState.Hit)
    }

    // 적 기지를 향해 이동하는 코루틴
    IEnumerator Move()
    {
        // 해당 유닛의 속도로 오른쪽으로 이동
        rig.velocity = Vector2.left * eDataInstance.spd;
        anim.SetTrigger("doMove");
        while (true)
        {
            yield return null;
        }
    }

    // 적과 만나면 공격하는 코루틴
    IEnumerator Attack()
    {
        rig.velocity = Vector2.zero;

        while (true)
        {
            anim.SetTrigger("doAttack");

            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !anim.IsInTransition(0));
            switch (eData.eName)
            {
                case "도로롱":
                    SingleDamage();
                    break;
                case "드레이크":
                    SingleDamage();
                    break;
                case "맥스웰":
                    SingleDamage();
                    break;
                case "라플라스":
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
            case "도로롱":
                GameManager.gm.mold += 100;
                break;
            case "드레이크":
                GameManager.gm.mold += 300;
                break;
            case "맥스웰":
                GameManager.gm.mold += 900;
                break;
            case "라플라스":
                GameManager.gm.mold += 2700;
                break;
            default:
                Debug.Log(" 이름을 못 받고 있음 ");
                break;
        }
        anim.SetTrigger("doDie");
        yield return new WaitForSeconds(0.2f);  // 지연시간을 주지 않으면 모션을 기다리지 않고 바로 죽게됨
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
