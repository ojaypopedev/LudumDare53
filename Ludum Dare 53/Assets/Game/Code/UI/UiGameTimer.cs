using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiGameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public GameObject timeChangeUIPrefab;

    private void Awake()
    {
        LevelManager.onGameTimerChanged += Refresh;
        GameManager.onReset += OnReset;
        CustomerManager.onCompletedFoodOrder += ShowTimeChangeUI;
    }

    public void ShowTimeChangeUI(bool sucess, FoodOrder _)
    {

        if(GameManager.gameMode == GameManager.GameMode.ENDLESS)
        {
            GameObject timeChangeInstance = Instantiate(timeChangeUIPrefab, timeChangeUIPrefab.transform.parent);
            timeChangeInstance.SetActive(true);

            if (sucess)
            {
                timeChangeInstance.GetComponentInChildren<TextMeshProUGUI>().text = "+" + LevelManager.instance.successRewardTime;
                timeChangeInstance.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
            }
            else
            {
                timeChangeInstance.GetComponentInChildren<TextMeshProUGUI>().text = "-" + LevelManager.instance.failurePunishTime;
                timeChangeInstance.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
            }

            Destroy(timeChangeInstance, 2);
        }

    }

    void OnReset()
    {
        timerText.text = LevelManager.getTimeLeftString;
    }

    void Refresh()
    {
        timerText.text = LevelManager.getTimeLeftString;
    }
}
