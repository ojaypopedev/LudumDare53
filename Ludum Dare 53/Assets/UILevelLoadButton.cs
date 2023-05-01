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

    public void Refresh(string LevelInfo, System.Action OnClicked, bool locked = false, string lockedInfo = "")
    {
        LoadButton.onClick.RemoveAllListeners();
        if(locked == false)
        {
            LoadButton.onClick.AddListener(()=>OnClicked?.Invoke());

        }
        else
        {
            LoadButton.interactable = false;
            LoadButton.image.color = LockedColour;
        }
        lockedInfoText.text = lockedInfo;
        LevelInfoText.text = LevelInfo;
        LockedObject.SetActive(locked);
    }

}
