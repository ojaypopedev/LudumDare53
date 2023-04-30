using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEndScreen : BaseScreen
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI ordersCompleted;
    public TextMeshProUGUI ordersFailed;
    public TextMeshProUGUI waitTime;
    public TextMeshProUGUI hotDogsFired;
    public TextMeshProUGUI accuracy;
    public Button continueButton;

    public override void Awake()
    {
        GameManager.onGameFinished += Show;
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnClickContinue);
        base.Awake();
    }

    void Show(GameManager.CompleteState completeState)
    {
        title.text = completeState == GameManager.CompleteState.WIN ? "LEVEL WON!" : "LEVEL FAILED!";
        Show();
    }

    public override void Show()
    {
        ordersCompleted.text = StatTracker.completedOrders.ToString();
        ordersFailed.text = StatTracker.failedOrders.ToString();
        waitTime.text = StatTracker.averageTime.ToString() + "s";
        hotDogsFired.text = StatTracker.hotDogsFired.ToString();
        accuracy.text = StatTracker.accuracy.ToString() + "%";

        base.Show();
    }

    public void OnClickContinue()
    {
        GameManager.Reset();
        Hide();
    }

}
