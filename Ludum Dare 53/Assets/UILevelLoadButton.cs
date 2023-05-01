using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UILevelLoadButton : MonoBehaviour
{
    public TextMeshProUGUI lockedInfoText, LevelInfoText;
    public GameObject LockedObject;
    public Button LoadButton;

    public Color LockedColour;
    public Color unlockedColour;

    public void Refresh(string LevelInfo, System.Action OnClicked, bool locked = false, string lockedInfo = "")
    {
        LoadButton.onClick.RemoveAllListeners();
        if(locked == false)
        {
            LoadButton.interactable = true;
            LoadButton.onClick.AddListener(()=>OnClicked?.Invoke());
            LoadButton.image.color = unlockedColour;

        }
        else
        {
            LoadButton.interactable = false;
            LoadButton.image.color = LockedColour;
        }
        lockedInfoText.text = lockedInfo;
        LevelInfoText.text = LevelInfo;
        LevelInfoText.gameObject.SetActive(!locked);
        LockedObject.SetActive(locked);
    }

}
