using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOnboardingMessage : BaseScreen
{

    public List<string> messageBacklog = new List<string>();

    public void ShowMessage(string message)
    {
        StartCoroutine(TypeText(message));
    }

    IEnumerator TypeText(string message)
    {
        yield return null;
    }

    public void OnClickContinue()
    {
        Hide();
    }
    
}
