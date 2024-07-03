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
            // 화살 모드일 때
            if (!magicAttackMode && GameManager.gm.cost >= 1)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 createPos = mousePos + new Vector2(-2, 2);
                GameObject arrow = Instantiate(arrowPrefab, createPos, arrowPrefab.transform.rotation);
                StartCoroutine(ArrowATK(arrow));
                GameManager.gm.cost--;
            }
            // 마법 모드일 때
            else if (magicAttackMode && GameManager.gm.cost >= 2)
            {
                //클릭한 위치에 생성
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
            // 화살을 마우스 클릭한 방향으로 일정 속도로 이동시키기
            Vector2 movArrow = Vector2.MoveTowards(arrow.transform.position, mousePos, 0.05f);
            arrow.transform.position = movArrow;

            yield return null; // 다음 프레임까지 기다림
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
