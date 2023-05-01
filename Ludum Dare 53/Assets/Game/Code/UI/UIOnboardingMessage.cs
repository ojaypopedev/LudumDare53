using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIOnboardingMessage : BaseScreen
{
    public static UIOnboardingMessage instance;
    public TextMeshProUGUI messageText;
    public List<string> messageBacklog = new List<string>();

    bool isShown;

    public override void Awake()
    {
        instance = this;
        base.Awake();
    }

    public void ShowMessage(string message)
    {
        Show();
        if (!isShown)
        {
            StartCoroutine(TypeText(message));
            isShown = true;
        }
        else
        {
            messageBacklog.Add(message);
        }
    }

    IEnumerator TypeText(string message)
    {
        messageText.text = "";
        int currentIndex = 0;

        while (currentIndex < message.Length - 1)
        {
            messageText.text += message[currentIndex];
            yield return new WaitForSecondsRealtime(Random.Range(0.01f, 0.1f));
            currentIndex++;
        }
    }

    public void OnClickContinue()
    {
        isShown = false;
        StopAllCoroutines();

        if (messageBacklog.Count > 0)
        {
            ShowMessage(messageBacklog.First());
            messageBacklog.RemoveAt(0);
        }
        Hide();
    }
    
}
