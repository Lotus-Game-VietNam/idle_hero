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
        InitEvents();
        InitText();
    }

    private void InitEvents()
    {
        this.AddListener(EventName.BuyItem, BuyItem);
    }

    private void InitText()
    {
        costBuyItemText.text = GetCostBuyItemTextValue();
    }


    private string GetCostBuyItemTextValue() => $"<sprite name=\"gem_blue\">{DataManager.BuyItemsData.GetCostToBuyItem()}";

    public void OnBuyItemClicked()
    {
        this.SendMessage(EventName.BuyItem, "InventoryManager");
    }

    private void BuyItem()
    {
        costBuyItemText.text = GetCostBuyItemTextValue();
    }
}
