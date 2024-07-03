using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject warriorPrefab, guardPrefab, archerPrefab, wizardPrefab;
    public GameObject giz;
    public List<GameObject> playerCharacter;
    public List<Button> playingBtns;
    public List<Button> standbyBtns;
    public Button infoBtn;

    public PlayerData warriorData, guardData, archerData, wizardData, pBase;
    public EnemyData eBase;
    public TextMeshProUGUI txtWeapon, txtWarrior, txtGuard, txtArcher, txtWizaed, txtCost, txtCostShadow, txtResult, txtWave, txtEHP, txtPHP, txtMold;
    public GameObject endWindow, startWindow;
    public Slider eHpSlider, pHpSlider;

    //public Button btnWeapon, btnWarrior, btnGuard, btnArcher, btnWizard;

    private void Awake()
    {
        if (!GameManager.gm.isFirstOn)
        {
            startWindow.SetActive(false);
        }
        else
        {
            startWindow.SetActive(true);
            GameManager.gm.isFirstOn = false;
        }
    }

    private void Update()
    {
        // ���� / �������� ���� ��ư on off
        BtnOnOffToPlaying();

        Text_Display(txtWeapon, pBase);
        Text_Display(txtWarrior, warriorData);
        Text_Display(txtGuard, guardData);
        Text_Display(txtArcher, archerData);
        Text_Display(txtWizaed, wizardData);
        Text_Display(txtCost, GameManager.gm.cost);
        Text_Display(txtCostShadow, GameManager.gm.cost);
        Text_Display(txtWave, eBase);
        Text_Display(txtMold, GameManager.gm.mold);

        HpBarDisplay(eHpSlider, txtEHP, eBase, pBase);
        HpBarDisplay(pHpSlider, txtPHP, eBase, pBase);

        TimeScaleESC();

        if (!GameManager.gm.isFirstInfo)
        {
            infoBtn.gameObject.SetActive(false);
        }
    }

    public void Btn_InfoSetting()
    {
        GameManager.gm.isFirstInfo = false;
    }

    public void Btn_GoMain()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Btn_Quit()
    {
        // Ŭ�� �� ���ø����̼� ����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif
    }

    // Pause�� Play ��ư�� ���� �Լ�
    public void Btn_TimeScale()
    {
        if (Time.timeScale > 0)
        {
            // �÷��� ���¶�� �Ͻ�����
            Time.timeScale = 0;
        }
        else
        {
            // �Ͻ����� ���¶�� �÷���
            Time.timeScale = 1;
        }
    }

    void TimeScaleESC()
    {
        if (Time.timeScale > 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
        }
        else if (Time.timeScale == 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
        }
    }


    // ���� ȭ�鿡 �ִ� ��ư Ŭ������ ĳ���͸� �����ϰ� �ش��ϴ� �ڽ�Ʈ�� ����
    public void SpawnToClick(Button btn)
    {
        Vector2 gizPos = giz.transform.position;
        GameObject characterPrefab = null;
        
        if (btn.name == "Warrior" && GameManager.gm.cost >= 2f)
        {
            characterPrefab = warriorPrefab;
            GameManager.gm.cost -= 2;
        }
        else if (btn.name == "Guard" && GameManager.gm.cost >= 3f)
        {
            characterPrefab = guardPrefab;
            GameManager.gm.cost -= 3;
        }
        else if (btn.name == "Archer" && GameManager.gm.cost >= 4f)
        {
            characterPrefab = archerPrefab;
            GameManager.gm.cost -= 4;
        }
        else if (btn.name == "Wizard" && GameManager.gm.cost >= 5f)
        {
            characterPrefab = wizardPrefab;
            GameManager.gm.cost -= 5;
        }

        if (characterPrefab != null)
        {
            GameObject go = Instantiate(characterPrefab, gizPos, Quaternion.identity);
            playerCharacter.Add(go);
        }
    }

    public void Text_Display(TextMeshProUGUI textGo, PlayerData player)
    {
        switch (textGo.name)
        {
            case "TextWeapon":
                textGo.text = "Lv " + player.level;
                break;

            case "TextWarrior":
                textGo.text = "Lv " + player.level;
                break;

            case "TextGuard":
                textGo.text = "Lv " + player.level;
                break;

            case "TextArcher":
                textGo.text = "Lv " + player.level;
                break;

            case "TextWizard":
                textGo.text = "Lv " + player.level;
                break;

            default:
                Debug.Log("������ ���̽��� �ƴ�");
                break;
        }
    }

    public void Text_Display(TextMeshProUGUI textGo, EnemyData eBase)
    {
        switch (textGo.name)
        {
            case "TextWave":
                textGo.text = "Wave " + eBase.level;
                break;
        }
    }

    public void Text_Display(TextMeshProUGUI textGo, float val)
    {
        int costs = Mathf.FloorToInt(val);
        switch (textGo.name)
        {
            case "TextCost":
                textGo.text = costs.ToString();
                break;
            case "TextCostShadow":
                textGo.text = costs.ToString();
                break;
            case "TextMold":
                textGo.text = val.ToString("n0");
                break;
        }
    }

    //���� HP�� ������� �����ִ� �ռ�
    public void HpBarDisplay(Slider hpBar, TextMeshProUGUI textHP, EnemyData eData, PlayerData pData)
    {
        //�̸��� ���� hpbar�� ǥ�õǴ� �ؽ�Ʈ�� �޶���
        if (hpBar.name == "EnemyHP")
        {
            hpBar.value = (float)(eData.hp / eData.hpMax);
            textHP.text = (hpBar.value * 100).ToString("F2") + "%";
        }

        else if (hpBar.name == "PlayerHP")
        {
            hpBar.value = (float)(pData.hp / pData.hpMax);
            textHP.text = (hpBar.value * 100).ToString("F2") + "%";
        }
    }

    //��ư�� ������ ������ �÷��ִ� �Լ�
    public void Btn_LevelUp(Button btnGo)
    {
        //sp�� 0���� Ŭ ���� �۵�
        if (GameManager.gm.mold > 0)
        {
            switch (btnGo.name)
            {
                case "Weapon":
                    //���� ���� ����
                    pBase.level++;
                    GameManager.gm.mold -= 500;
                    break;

                case "Warrior":
                    //Warrior ���� ����
                    warriorData.level++;
                    GameManager.gm.mold -= 200;
                    break;

                case "Guard":
                    //Guard ���� ����
                    guardData.level++;
                    GameManager.gm.mold -= 100;
                    break;

                case "Archer":
                    //Archer ���� ����
                    archerData.level++;
                    GameManager.gm.mold -= 400;
                    break;

                case "Wizard":
                    //Wizard ���� ����
                    wizardData.level++;
                    GameManager.gm.mold -= 400;
                    break;

                default:
                    Debug.Log("������ ���̽��� �ƴ�");
                    break;
            }
        }
    }

    private void BtnOnOffToPlaying()
    {
        // ������ �ð��� ��
        if (!GameManager.gm.isPlaying)
        {
            foreach (Button playingBtn in playingBtns)
            {
                playingBtn.enabled = false;
            }
            foreach (Button standbyBtn in standbyBtns)
            {
                standbyBtn.enabled = true;
                standbyBtn.gameObject.SetActive(true);
                infoBtn.enabled = true;
            }
        }
        // �������϶�
        else if (GameManager.gm.isPlaying)
        {
            foreach (Button playingBtn in playingBtns)
            {
                playingBtn.enabled = true;
            }
            foreach (Button standbyBtn in standbyBtns)
            {
                standbyBtn.enabled = false;
                standbyBtn.gameObject.SetActive(false);
                infoBtn.enabled = false;
            }
        }
    }

    public void End_UI_Window(GameObject window, TextMeshProUGUI endTxt, string result)
    {
        Time.timeScale = 0;
        window.SetActive(true);

        endTxt.text = result;
    }
}
