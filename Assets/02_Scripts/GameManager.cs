using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public float cost = 5;
    public int mold;
    public bool isPlaying = false;
    public bool isFirstOn = true;
    public GameObject ebase, pbase;

    public bool isFirstInfo = true;

    void Awake()
    {
        //싱글톤 GameManager 초기화
        if (gm == null)
        {
            gm = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        mold = 1000;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1;
        cost = 5;
        ebase = GameObject.FindGameObjectWithTag("EnemyBase");
        pbase = GameObject.FindGameObjectWithTag("PlayerBase");
    }

        private void Update()
    {
        // 플레이중일 때만 코스트 증가
        if (isPlaying && cost<=10)
        {
            cost += Time.deltaTime;
        }
    }
}