using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodOrderUI : MonoBehaviour
{
    public static FoodOrderUI Prefab => Resources.Load<FoodOrderUI>("UI/FoodOrderUI");

    private FoodOrder _order;
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] Image fillImage;
    [SerializeField] Gradient fillGradient;
    public static FoodOrderUI Create(FoodOrder foodOrder)
    {
        FoodOrderUI uiInstance = Instantiate(Prefab);
        uiInstance.Refresh(foodOrder);
        return uiInstance;
    }

    private void Refresh(FoodOrder foodOrder)
    {
        _order = foodOrder;
        textMesh.text = _order.recipie.recipeName;
    }

    private void Update()
    {
        if (_order == null) return;

        fillImage.fillAmount = _order.TimeLeftPercentage;
        fillImage.color = fillGradient.Evaluate(_order.TimeLeftPercentage);

    }

    public  void Hide()
    {
        Destroy(gameObject);
    }
}
