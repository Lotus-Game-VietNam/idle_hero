using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;


public class UIFarmManager : MonoUI
{
    [Title("Text Mesh Pro")]
    [SerializeField] private TMP_Text costBuyItemText = null;
    [SerializeField] private TMP_Text sellValueText = null;

    [Title("Game Object")]
    [SerializeField] private GameObject sellTextObj = null;
    [SerializeField] private GameObject sellValueObj = null;

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
        this.AddListener<bool>(EventName.ShowShellValue, ShowSellValue);
    }

    private void InitText()
    {
        costBuyItemText.text = GetCostBuyItemTextValue();
    }


    private string GetCostBuyItemTextValue() => $"<sprite name=\"gem_blue\">{DataManager.InventoryData.GetCostToBuyItem().Convert()}";

    private string GetSellItemTextValue() => $"<sprite name=\"gem_blue\">{DataManager.InventoryData.GetValueToSellItem().Convert()}";

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

    private void ShowSellValue(bool show)
    {
        sellValueObj.SetActive(show);
        sellTextObj.SetActive(!show);
        sellValueText.text = GetSellItemTextValue();
    }
}
