using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    // 플레이어의 상태를 파악하기 위해 사용하는 열거형 변수들
    Idle = 0,
    Move,
    Attack,
    Hit,
    Die,
}

public class Player : MonoBehaviour
{
    // ScriptableObject인 PlayerData를 사용하기 위해 변수로 선언
    public PlayerData pData;

    // pData를 개별적으로 저장할 변수
    public PlayerData pDataInstance;

    public GameObject arrow, magicB;

    // 열거형으로 만든 상태를 사용하기 위해 변수 선언
    PlayerState playerState;

    Animator anim;
    Rigidbody2D rig;
    SpriteRenderer sr;

    // 공격 상태를 파악하기 위한 bool 변수
    bool isAttacking = false;       //적 유닛 공격상태
    bool isBaseAttacking = false;    //적 기지 공격상태
    //bool isHitting = false;         // 피격 상태

    float damage;
    //float previousHP;

    // 충돌한 적을 담기위한 List
    List<GameObject> enemys = new List<GameObject>();

    // 캐릭터 이름에 따라 스탯 초기화, 애니메이션과 리지드바디 초기화
    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        rig = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();

        if (pData != null)
        {
            // pData의 새로운 인스턴스를 생성해 pDataInstance에 저장
            pDataInstance = ScriptableObject.CreateInstance<PlayerData>();

            // pData의 데이터를 pDataInstance로 복사
            pDataInstance.pName = pData.pName;
            pDataInstance.level = pData.level;
            pDataInstance.atk = pData.atk;
            pDataInstance.hpMax = pData.hpMax;
            pDataInstance.hp = pData.hp;
            pDataInstance.def = pData.def;
            pDataInstance.spd = pData.spd;
            pDataInstance.atkSpd = pData.atkSpd;

            // 이름에 따라 유닛 초기화
            switch (pData.pName)
            {
                case "전사":
                    pDataInstance.atk = 10 + pData.level;
                    pDataInstance.hpMax = 200 + pData.level * 15;
                    pDataInstance.hp = pData.hpMax;
                    pDataInstance.def = 4 + pData.level;
                    pDataInstance.spd = 2f;
                    pDataInstance.atkSpd = 1.5f;
                    break;

                case "가드":
                    //pDataInstance.atk = 5 + pData.level;
                    pDataInstance.atk = 0;
                    pDataInstance.hpMax = 400 + pData.level * 50;
                    pDataInstance.hp = pData.hpMax;
                    pDataInstance.def = 7 + pData.level;
                    pDataInstance.spd = 1;
                    pDataInstance.atkSpd = 4f;
                    break;

                case "궁수":
                    pDataInstance.atk = 15 + pData.level;
                    pDataInstance.hpMax = 70 + pData.level * 10;
                    pDataInstance.hp = pData.hpMax;
                    pDataInstance.def = 2 + pData.level;
                    pDataInstance.spd = 1.5f;
                    pDataInstance.atkSpd = 1.5f;
                    break;

                case "마법사":
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
        // 생성되면 우선은 바로 이동
        ChangeState(PlayerState.Move);
    }

    // 열거형 이름에 따라 현재 상태를 바꾸고 해당 상태의 코루틴을 실행하는 함수
    public void ChangeState(PlayerState newState)
    {
        // 플레이어의 직전 상태가 현재 상태가 아닐때 실행 ( 즉, 상태가 변하면 실행 )
        if (playerState != newState)
        {
            // 이전 상태 코루틴 종료
            StopCoroutine(playerState.ToString());  
            //열거형 뒤에 ToString()을 사용하면 0,1,2가 아닌 해당 이름으로 반환됨

            // 새로운 상태로 변경
            playerState = newState;

            // 현재 상태의 코루틴 실행
            StartCoroutine(playerState.ToString());
        }
    }

    private void Update()
    {
        // 현재 상태를 실시간으로 판단하기 위해 Update에서 사용
        if (pDataInstance.hp <= 0)
        {
            // 현재 hp가 0이하일때 Die로 상태 변환
            ChangeState(PlayerState.Die);
        }
        else if ((isAttacking || isBaseAttacking))
        {
            // 현재 공격중일때 Attack로 상태 변환
            ChangeState(PlayerState.Attack);
        }
        else
        {
            // 그 외에는 이동
            ChangeState(PlayerState.Move);
        }
    }
    
    // 적 기지를 향해 이동하는 코루틴
    IEnumerator Move()
    {
        // 해당 유닛의 속도로 오른쪽으로 이동
        rig.velocity = Vector2.right * pDataInstance.spd;
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

            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f 
            && !anim.IsInTransition(0));
            switch (pData.pName)
            {
                case "전사":
                    SingleDamage();
                    break;
                case "가드":
                    SingleDamage();
                    break;
                case "궁수":
                    // 투사체 생성, 투사체가 적에게 닿았을 경우 투사체를 없애고 데미지
                    ArrowAtk();
                    break;
                case "마법사":
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
            Transform enemyTransform = enemys[0].transform; // enemys 배열은 적 캐릭터의 리스트라고 가정
            while (arrow != null && enemyTransform != null)
            {
                // 현재 화살과 적 캐릭터 간의 거리 계산
                float distance = Vector2.Distance(arrow.transform.position, enemyTransform.position);
                // 일정 거리 이하로 이동했을 때 화살 파괴
                if (distance <= 0.6f)
                {
                    SingleDamage();
                    Destroy(arrow);
                    yield break; // 코루틴 종료
                }
                // 화살을 적 캐릭터 방향으로 일정 속도로 이동시키기
                Vector2 movArrow = Vector2.MoveTowards(arrow.transform.position, 
                            enemyTransform.position + new Vector3(1f, 0.5f), 0.05f);
                arrow.transform.position = movArrow;
                yield return null; // 다음 프레임까지 기다림
            }
        }
        if (isBaseAttacking)
        {
            Base ebaseC = GameManager.gm.ebase.GetComponent<Base>();
            while (arrow != null && ebaseC != null)
            {
                // 현재 화살과 적 캐릭터 간의 거리 계산
                float distance = Vector2.Distance(arrow.transform.position, 
                                    ebaseC.gameObject.transform.position);
                // 일정 거리 이하로 이동했을 때 화살 파괴
                if (distance <= 0.6f)
                {
                    SingleDamage();
                    Destroy(arrow);
                    yield break; // 코루틴 종료
                }
                // 화살을 적 캐릭터 방향으로 일정 속도로 이동시키기
                Vector2 movArrow = Vector2.MoveTowards(arrow.transform.position, 
                                    ebaseC.gameObject.transform.position, 0.05f);
                arrow.transform.position = movArrow;
                yield return null; // 다음 프레임까지 기다림
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
        // bool 변수로 통제해주려 했을 때는 해당 코루틴은 정상적으로 작동되나 이후에 공격을 안하고 그냥 서로 넘어가버림
        Debug.Log("맞았음");
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        // 안되는 원인은 이 코루틴이 진행하는 동안 다른 코루틴이 실행되어 이 코루틴이 중단됨.
        //yield return new WaitForEndOfFrame(); 이거로 하면 변경은 되나 너무 빨라서 유저가 인지를 못함
        sr.color = Color.red;
        Debug.Log("원래 색상으로 변경");
        isHitting = false;
    }
    */

    IEnumerator Die()
    {
        rig.velocity = Vector2.zero;
        anim.SetTrigger("doDie");
        yield return new WaitForSeconds(0.2f);  // 지연시간을 주지 않으면 모션을 기다리지 않고 바로 죽게됨
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
