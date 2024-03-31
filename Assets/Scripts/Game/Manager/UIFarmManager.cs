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

    [Title("Component")]
    [SerializeField] private UI_CountingDown autoTapCountingDown = null;
    [SerializeField] private UI_CountingDown x2IncomeCountingDown = null;


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


    private string GetCostBuyItemTextValue() => $"<sprite name=\"gem_blue\">{DataManager.InventoryData.GetCostToBuyItem().Convert()}";

    public void OnBuyItemClicked()
    {
        this.SendMessage(EventName.BuyItem, "InventoryManager");
    }

    private void BuyItem()
    {
        costBuyItemText.text = GetCostBuyItemTextValue();
    }


    public void OnAutoTapClicked()
    {
        autoTapCountingDown.CountingDown();
    }

    public void OnX2IncomeClicked()
    {
        this.SendMessage(EventName.X2Income, "FarmManager", true);
        x2IncomeCountingDown.CountingDown();
        x2IncomeCountingDown.OnCountingDownCussess = () => 
        {
            this.SendMessage(EventName.X2Income, "FarmManager", false);
        };
    }

    public void UpgradeIncomeClicked()
    {
        DataManager.HeroData.SetUpgradeIncomeSuccess().Save();
        LogTool.LogEditorOnly($"Level: {DataManager.HeroData.inComeLevel} --- {DataManager.HeroData.GetMinIncome().Convert()}");
    }
}
