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
        // 전투 / 비전투에 따라 버튼 on off
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
        // 클릭 시 어플리케이션 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif
    }

    // Pause와 Play 버튼에 들어가는 함수
    public void Btn_TimeScale()
    {
        if (Time.timeScale > 0)
        {
            // 플레이 상태라면 일시정지
            Time.timeScale = 0;
        }
        else
        {
            // 일시정지 상태라면 플레이
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


    // 메인 화면에 있는 버튼 클릭으로 캐릭터를 생성하고 해당하는 코스트를 차감
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
                Debug.Log("지정된 케이스가 아님");
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

    //현재 HP를 백분율로 보여주는 합수
    public void HpBarDisplay(Slider hpBar, TextMeshProUGUI textHP, EnemyData eData, PlayerData pData)
    {
        //이름에 따라 hpbar에 표시되는 텍스트가 달라짐
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

    //버튼을 누르면 레벨을 올려주는 함수
    public void Btn_LevelUp(Button btnGo)
    {
        //sp가 0보다 클 때만 작동
        if (GameManager.gm.mold > 0)
        {
            switch (btnGo.name)
            {
                case "Weapon":
                    //무기 레벨 증가
                    pBase.level++;
                    GameManager.gm.mold -= 500;
                    break;

                case "Warrior":
                    //Warrior 레벨 증가
                    warriorData.level++;
                    GameManager.gm.mold -= 200;
                    break;

                case "Guard":
                    //Guard 레벨 증가
                    guardData.level++;
                    GameManager.gm.mold -= 100;
                    break;

                case "Archer":
                    //Archer 레벨 증가
                    archerData.level++;
                    GameManager.gm.mold -= 400;
                    break;

                case "Wizard":
                    //Wizard 레벨 증가
                    wizardData.level++;
                    GameManager.gm.mold -= 400;
                    break;

                default:
                    Debug.Log("지정된 케이스가 아님");
                    break;
            }
        }
    }

    private void BtnOnOffToPlaying()
    {
        // 비전투 시간일 때
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
        // 전투중일때
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
