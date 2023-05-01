using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiGameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public GameObject timeChangeUIPrefab;

    public GameObject endlessTime;
    public TextMeshProUGUI endlessTimeText;
    public TextMeshProUGUI pauseText;

    private void Awake()
    {
        LevelManager.onGameTimerChanged += Refresh;
        GameManager.onReset += OnReset;
        CustomerManager.onCompletedFoodOrder += ShowTimeChangeUI;
        pauseText.text = Application.platform == RuntimePlatform.WebGLPlayer ? "Press P to pause" : "Press ESC to pause";
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
        endlessTime.gameObject.SetActive(GameManager.gameMode == GameManager.GameMode.ENDLESS);
        endlessTimeText.text = "Current Time: " + LevelManager.currentTimeLastedString;
    }
}
