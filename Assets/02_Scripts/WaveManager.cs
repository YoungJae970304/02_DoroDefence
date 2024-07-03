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

    // ��ư �̺�Ʈ�� �߰��� ���̺� ���� �Լ�
    public void StartWave()
    {
        // ���̺� ���°� ����Ǿ� ���� ���� ����
        if (!GameManager.gm.isPlaying)
        {
            // ���͸� ���� �������� �����ϴ� �ڷ�ƾ�� ����
            StartCoroutine(SpawnMonster()); 
        }
    }

    // ���� ���� �ڷ�ƾ
    IEnumerator SpawnMonster()
    {
        // ���̺� ���¸� ���������� ����
        GameManager.gm.isPlaying = true;

        // �����Ǵ� ������ ���� ( ���̺� ���� * 5 )
        num = enemyBase.eData.level * 5;    

        // ���� �����̸��� ���͸� ����
        for (int i = 0; i < num; i++)
        {
            SpawnRandMob();
            float randomDelay = Random.Range(0.5f, 4f);
            yield return new WaitForSeconds(randomDelay);
        }
    }

    void SpawnRandMob()
    {
        // ���� �� ����
        int randValue = Random.Range(0, 1001);  

        // �� ������ ���̺� ������ �����Ǵ� Ȯ��
        int LaProbability = enemyBase.eData.level * 1;
        int MaProbability = enemyBase.eData.level * 2;
        int drProbability = enemyBase.eData.level * 4;
        
        // �� ���� Ȯ���� ���ص� ���� Ȯ������ �Ѿ�� �ʵ��� ����
        LaProbability = Mathf.Clamp(LaProbability, 0, 350-10);  
        MaProbability = Mathf.Clamp(MaProbability, 0, 650-60);
        drProbability = Mathf.Clamp(drProbability, 0, 850-160);

        //���ö� : �ʱ� Ȯ�� 1%, ���� Ȯ�� 35%
        if (randValue <= 10 + LaProbability)
        {
            Instantiate(prefabsMob[3], spawnPos.position, Quaternion.identity);
        }

        //�ƽ��� : �ʱ� Ȯ�� 5%, ���� Ȯ�� 30%
        else if (randValue <= 60 + MaProbability)
        {
            Instantiate(prefabsMob[2], spawnPos.position, Quaternion.identity);
        }

        //�巹��ũ : �ʱ� Ȯ�� 10%, ���� Ȯ�� 20%
        else if (randValue <= 160 + drProbability)
        {
            Instantiate(prefabsMob[1], spawnPos.position, Quaternion.identity);
        }

        //���η� : �ʱ� Ȯ�� 84%, ���� Ȯ�� 15%
        else
        {
            Instantiate(prefabsMob[0], spawnPos.position, Quaternion.identity);
        }
    }
}
