using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public Base enemyBase;
    public Transform spawnPos;

    public List<GameObject> prefabsMob;
    int num;

    // 버튼 이벤트에 추가할 웨이브 시작 함수
    public void StartWave()
    {
        // 웨이브 상태가 종료되어 있을 때만 실행
        if (!GameManager.gm.isPlaying)
        {
            // 몬스터를 랜덤 간격으로 생성하는 코루틴을 시작
            StartCoroutine(SpawnMonster()); 
        }
    }

    // 몬스터 스폰 코루틴
    IEnumerator SpawnMonster()
    {
        // 웨이브 상태를 진행중으로 변경
        GameManager.gm.isPlaying = true;

        // 생성되는 몬스터의 숫자 ( 웨이브 레벨 * 5 )
        num = enemyBase.eData.level * 5;    

        // 일정 딜레이마다 몬스터를 생성
        for (int i = 0; i < num; i++)
        {
            SpawnRandMob();
            float randomDelay = Random.Range(0.5f, 4f);
            yield return new WaitForSeconds(randomDelay);
        }
    }

    void SpawnRandMob()
    {
        // 랜덤 값 생성
        int randValue = Random.Range(0, 1001);  

        // 각 몬스터의 웨이브 레벨당 변동되는 확률
        int LaProbability = enemyBase.eData.level * 1;
        int MaProbability = enemyBase.eData.level * 2;
        int drProbability = enemyBase.eData.level * 4;
        
        // 각 변동 확률이 정해둔 최종 확률보다 넘어가지 않도록 조정
        LaProbability = Mathf.Clamp(LaProbability, 0, 350-10);  
        MaProbability = Mathf.Clamp(MaProbability, 0, 650-60);
        drProbability = Mathf.Clamp(drProbability, 0, 850-160);

        //라플라스 : 초기 확률 1%, 최종 확률 35%
        if (randValue <= 10 + LaProbability)
        {
            Instantiate(prefabsMob[3], spawnPos.position, Quaternion.identity);
        }

        //맥스웰 : 초기 확률 5%, 최종 확률 30%
        else if (randValue <= 60 + MaProbability)
        {
            Instantiate(prefabsMob[2], spawnPos.position, Quaternion.identity);
        }

        //드레이크 : 초기 확률 10%, 최종 확률 20%
        else if (randValue <= 160 + drProbability)
        {
            Instantiate(prefabsMob[1], spawnPos.position, Quaternion.identity);
        }

        //도로롱 : 초기 확률 84%, 최종 확률 15%
        else
        {
            Instantiate(prefabsMob[0], spawnPos.position, Quaternion.identity);
        }
    }
}
