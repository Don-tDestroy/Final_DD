using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance; // 싱글톤 객체

    private int isTutorial = 0; // 게임 첫 시작 여부 (첫 시작이면 True)
    private int totalPartCnt = 0; // 총 주운 부품 개수

    private int ewhaPower = 0;
    private int curStageNumber = 0;
    private bool isEnding = false;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (!PlayerPrefs.HasKey("IsTutorial"))
        {
            isTutorial = 1;
            PlayerPrefs.SetInt("IsTutorial", 1);
        }
        isTutorial = PlayerPrefs.GetInt("IsTutorial");
    }

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }
            return _instance;
        }
    }

    public int GetIsTutorial()
    {
        return isTutorial;
    }

    public void SetIsTutorial(int isTutorial)
    {
        PlayerPrefs.SetInt("IsTutorial", isTutorial);
    }

    public int GetTotalPartCnt()
    {
        return totalPartCnt;
    }

    public void AddTotalPartCnt(int cnt)
    {
        totalPartCnt += cnt;
    }

    public int GetStageNumber()
    {
        return curStageNumber;
    }

    public void SetStageNumber(int n)
    {
        curStageNumber = n;
    }

    public int GetEwhaPower()
    {
        return ewhaPower;
    }

    public void AddEwhaPower(int epower)
    {
        ewhaPower += epower;
    }

    public bool GetIsEnding()
    {
        return isEnding;
    }

    public void SetIsEnding(bool ending)
    {
        isEnding = ending;
    }
}
