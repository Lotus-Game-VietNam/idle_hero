using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UIFarmManager : MonoUI
{
    [Title("Text Mesh Pro")]
    [SerializeField] private TMP_Text costBuyItemText = null;


    private void Awake()
    {
        ComponentReference.MainRect = () => rect;
    }

    private void Start()
    {
        InitText();
    }

    private void InitText()
    {
        costBuyItemText.text = GetCostBuyItemTextValue();
    }


    private string GetCostBuyItemTextValue() => $"<sprite name=\"gem_blue\">{DataManager.BuyItemsData.GetCostToBuyItem()}";

    public void OnBuyItemClicked()
    {
        float costToBuy = DataManager.BuyItemsData.GetCostToBuyItem();

        if (ResourceManager.Gem < costToBuy)
            return;

        ResourceManager.Gem -= costToBuy;
        DataManager.BuyItemsData.SetBuyItemSuccess().Save();

        costBuyItemText.text = GetCostBuyItemTextValue();

        this.SendMessage(EventName.BuyItem);
    }
}
