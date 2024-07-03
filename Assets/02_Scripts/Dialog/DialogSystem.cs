using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    // ��ȭ�� �����ϴ� ĳ���͵��� UI �迭
    private Speaker[] speakers;

    [SerializeField]
    //���� �б��� ��� ��� �迭
    private DialogData[] dialogs;

    [SerializeField]
    // �ڵ� ���� ����
    private bool isAutoStart = true;
    // ���� 1ȸ�� ȣ���ϱ� ���� ����
    private bool isFirst = true;
    // ���� ��� ����
    private int currentDialogIndex = -1;
    // ���� ���� �ϴ� ȭ��(Speaker)�� speakers �迭 ����
    private int currentSpeakerIndex = 0;
    // �ؽ�Ʈ ��Ƽ�� ȿ���� ��� �ӵ�
    private float typingSpeed = 0.1f;
    // �ؽ�Ʈ Ÿ���� ȿ���� ���������
    private bool isTypingEffect = false;

    private void Awake()
    {
        //04
        Setup();
    }

    //03
    private void Setup()
    {
        // ��� ��ȭ ���� ���ӿ�����Ʈ ��Ȱ��ȭ
        for (int i = 0; i < speakers.Length; ++i)  //��ġ�� ��ġ�� ����? �����ʳ�?
        {
            SetActiveObjects(speakers[i], false);
            //ĳ���� �̹����� ���̵��� ����
            speakers[i].spriteRenderer.gameObject.SetActive(true);
        }
    }

    //05
    public bool UpdateDialog()
    {
        // ��� �бⰡ ���۵� �� 1ȸ�� ȣ��
        if (isFirst == true)
        {
            //�ʱ�ȭ. ĳ���� �̹����� Ȱ��ȭ�ϰ�,
            // ��� ���� UI�� ��� ��Ȱ��ȭ
            Setup();

            // �ڵ� ���(isAutoStart = true)�� �����Ǿ� ������
            // ù ��° ��� ���
            if (isAutoStart)
            {
                SetNextDialog();
            }

            isFirst = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // �ؽ�Ʈ Ÿ���� ȿ���� ������϶�
            // ���콺 ���� Ŭ���ϸ� Ÿ���� ȿ�� ����
            if (isTypingEffect == true)
            {
                isTypingEffect = false;

                // Ÿ���� ȿ���� �����ϰ�, ���� ��� ��ü�� ���
                StopCoroutine("OnTypingText");
                speakers[currentSpeakerIndex].textDialogue.text =
                dialogs[currentDialogIndex].dialogue;
                // ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
                speakers[currentSpeakerIndex].objectArrow.SetActive(true);

                return false;
            }

            // ��簡 �������� ��� ���� ��� ����
            if (dialogs.Length > currentDialogIndex + 1)
            {
                SetNextDialog();
            }
            // ��簡 �� �̻� ���� ���
            // ��� ������Ʈ�� ��Ȱ��ȭ�� true ��ȯ
            else
            {
                // ���� ��ȭ�� �����ߴ� ��� ĳ����,
                // ��ȭ ���� UI�� ������ �ʰ� ��Ȱ��ȭ
                for (int i = 0; i < speakers.Length; ++i)
                {
                    SetActiveObjects(speakers[i], false);
                    //SetActiveObjects()�� ĳ���� �̹����� ������ �ʰ� �ϴ� �κ��� ���� ������ ������ ȣ��
                    speakers[i].spriteRenderer.gameObject.SetActive(false);
                }

                return true;
            }
        }

        return false;
    }

    private void SetNextDialog()
    {
        // ���� ȭ���� ��ȭ ���� ������Ʈ ��Ȱ��ȭ
        SetActiveObjects(speakers[currentSpeakerIndex], false);

        // ���� ��縦 �����ϵ���
        currentDialogIndex++;

        //���� ȭ�� ���� ����
        currentSpeakerIndex =
        dialogs[currentDialogIndex].speakerIndex;

        // ���� ȭ���� ��ȭ ���� ������Ʈ Ȱ��ȭ
        SetActiveObjects(speakers[currentSpeakerIndex], true);
        // ���� ȭ�� �̸� �ؽ�Ʈ ����
        speakers[currentSpeakerIndex].textName.text =
        dialogs[currentDialogIndex].name;
        // ���� ȭ���� ��� �ؽ�Ʈ ����
        // �ڷ�ƾ �ؽ�Ʈ ȿ���� ��ó
        /*
        speakers[currentSpeakerIndex].textDialogue.text =
        dialogs[currentDialogIndex].dialogue;
        */
        StartCoroutine("OnTypingText");
    }

    private IEnumerator OnTypingText()
    {
        int index = 0;

        isTypingEffect = true;

        // �ؽ�Ʈ�� �� ���ھ� Ÿ���� ġ�� ���
        while (index <= dialogs[currentDialogIndex].dialogue.Length)
        {
            speakers[currentSpeakerIndex].textDialogue.text =
            dialogs[currentDialogIndex].dialogue.Substring(0, index);   //0(ù����)���� index���� ǥ��
            //C# ���ڿ� ���� ��� ����
            index++;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTypingEffect = false;

        // ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
        speakers[currentSpeakerIndex].objectArrow.SetActive(true);
    }

    private void SetActiveObjects(Speaker speaker, bool visible)
    {
        speaker.imageDialog.gameObject.SetActive(visible);
        speaker.textName.gameObject.SetActive(visible);
        speaker.textDialogue.gameObject.SetActive(visible);

        // ȭ��ǥ�� ��簡 ����Ǿ��� ���� Ȱ��ȭ�ϱ� ������ �׻� false
        speaker.objectArrow.SetActive(false);

        // ĳ���� ���� �� ����
        Color color = speaker.spriteRenderer.color;
        color.a = visible == true ? 1 : 0.2f; //visible ? 1 : 0.2f;
        speaker.spriteRenderer.color = color;
    }
}

[System.Serializable]
public struct Speaker
{
    // ĳ���� �̹��� (û�� / ȭ�� ���İ� ����)
    public SpriteRenderer spriteRenderer;
    // ��ȭâ �̹��� UI
    public Image imageDialog;
    // ���� ������� ĳ���� �̸� ��� Text UI
    public TextMeshProUGUI textName;
    // ���� ��� ��� �ؽ�Ʈ UI
    public TextMeshProUGUI textDialogue;
    // ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� ������Ʈ
    public GameObject objectArrow;
}

[System.Serializable]
public struct DialogData
{
    // �̸��� ��縦 ����� ���� DialogSystem�� spakers �迭 ����
    public int speakerIndex;
    // ĳ���� �̸�
    public string name;
    [TextArea(3, 5)]
    // ���
    public string dialogue;
}
