using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class DialogTest : MonoBehaviour
{
    public GameObject[] box;

    public GameObject shopPanel, infoDialog;

    [SerializeField]
    private DialogSystem[] dialogSystem;

    private IEnumerator Start()
    {
        /*
        // ī�޶� �� �������� �̵�
        Vector2 camPos = Camera.main.transform.position;
        camPos.x = 12;
        Camera.main.transform.position = camPos;
        */
        box[6].SetActive(true);
        // ù ��° ��� �б� ����
        yield return new WaitUntil(() => dialogSystem[0].UpdateDialog());

        // ù��° ��� ������ ����
        box[0].SetActive(true);
        yield return new WaitUntil(() => dialogSystem[1].UpdateDialog());

        // �� ��° ��� ������ ����
        shopPanel.SetActive(true);
        box[0].SetActive(false);
        yield return new WaitUntil(() => dialogSystem[2].UpdateDialog());

        // 2-1���� ����
        shopPanel.SetActive(false);
        box[1].SetActive(true);
        yield return new WaitUntil(() => dialogSystem[3].UpdateDialog());

        // 3���� ����
        box[1].SetActive(false);
        box[2].SetActive(true);
        yield return new WaitUntil(() => dialogSystem[4].UpdateDialog());

        // 4���� ����
        box[2].SetActive(false);
        box[3].SetActive(true);
        yield return new WaitUntil(() => dialogSystem[5].UpdateDialog());

        // 4_1 �� �� ����
        box[3].SetActive(false);
        box[4].SetActive(true);
        yield return new WaitUntil(() => dialogSystem[6].UpdateDialog());

        // ������ ���
        box[4].SetActive(false);
        box[5].SetActive(true);
        yield return new WaitUntil(() => dialogSystem[7].UpdateDialog());
        box[5].SetActive(false);
        box[6].SetActive(false);
        yield return new WaitUntil(() => dialogSystem[8].UpdateDialog());


        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
        infoDialog.SetActive(false);

        //�÷��� ��� ����
        //UnityEditor.EditorApplication.ExitPlaymode();
    }
}
