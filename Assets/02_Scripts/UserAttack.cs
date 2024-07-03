using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UserAttack : MonoBehaviour
{
    public GameObject arrowImage, magicImage, arrowPrefab, magicPrefab;
    public TextMeshProUGUI needCost;

    bool magicAttackMode = false;

    void Update()
    {
        AttackModeChange();

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() 
            && GameManager.gm.isPlaying)
        {
            // ȭ�� ����� ��
            if (!magicAttackMode && GameManager.gm.cost >= 1)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 createPos = mousePos + new Vector2(-2, 2);
                GameObject arrow = Instantiate(arrowPrefab, createPos, arrowPrefab.transform.rotation);
                StartCoroutine(ArrowATK(arrow));
                GameManager.gm.cost--;
            }
            // ���� ����� ��
            else if (magicAttackMode && GameManager.gm.cost >= 2)
            {
                //Ŭ���� ��ġ�� ����
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameObject magic = Instantiate(magicPrefab, mousePos, Quaternion.identity);
                StartCoroutine(MagicATK(magic));
                GameManager.gm.cost -= 2;
            }
        }
    }

    IEnumerator ArrowATK(GameObject arrow)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        while (arrow != null)
        {
            // ȭ���� ���콺 Ŭ���� �������� ���� �ӵ��� �̵���Ű��
            Vector2 movArrow = Vector2.MoveTowards(arrow.transform.position, mousePos, 0.05f);
            arrow.transform.position = movArrow;

            yield return null; // ���� �����ӱ��� ��ٸ�
        }
    }

    IEnumerator MagicATK(GameObject paticleOb)
    {
        paticleOb.SetActive(true);
        ParticleSystem particleSystem = paticleOb.GetComponent<ParticleSystem>();

        yield return new WaitUntil(() => !particleSystem.isPlaying);
        Destroy(paticleOb);
    }

    void AttackModeChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            magicAttackMode = false;
            arrowImage.SetActive(true);
            magicImage.SetActive(false);
            needCost.text = "1";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            magicAttackMode = true;
            arrowImage.SetActive(false);
            magicImage.SetActive(true);
            needCost.text = "2";
        }
    }

     
}
