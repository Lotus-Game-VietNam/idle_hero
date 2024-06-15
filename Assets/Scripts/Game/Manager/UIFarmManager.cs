using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;


public class UIFarmManager : MonoUI
{
    [Title("Text Mesh Pro")]
    [SerializeField] private TMP_Text costBuyItemText = null;
    [SerializeField] private TMP_Text costUpgradeIncomeText = null;
    [SerializeField] private TMP_Text sellValueText = null;
    [SerializeField] private TMP_Text stageText = null;
    [SerializeField] private TMP_Text landText = null;

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
        InitText();

        this.AddListener(EventName.BuyItem, BuyItem);
        this.AddListener<bool>(EventName.ShowShellValue, ShowSellValue);
    }

    private void OnDisable()
    {
        this.RemoveListener(EventName.BuyItem);
        this.RemoveListener(EventName.ShowShellValue);
    }


    private void InitText()
    {
        costBuyItemText.text = GetCostBuyItemTextValue();
        costUpgradeIncomeText.text = GetCostUpgradeIncomeTextValue();
        stageText.text = $"STAGE {DataManager.WorldData.currentLevel}:";
        landText.text = GameConstants.landName[((DataManager.WorldData.currentLevel - 1) / 3)].ToUpper();
    }


    private string GetCostBuyItemTextValue() => $"<sprite name=\"gem_blue\">{DataManager.InventoryData.GetCostToBuyItem().Convert()}";

    private string GetCostUpgradeIncomeTextValue() => $"<sprite name=\"gem_blue\">{DataManager.HeroData.GetCostUpgradeIncome().Convert()}";

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
        this.SendMessage(EventName.AutoTap, "FarmManager", true);
        autoTapCountingDown.CountingDown();
        autoTapCountingDown.OnCountingDownCussess = () =>
        {
            this.SendMessage(EventName.AutoTap, "FarmManager", false);
        };
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
        if (ResourceManager.Gem < DataManager.HeroData.GetCostUpgradeIncome())
            return;

        ResourceManager.Gem -= DataManager.HeroData.GetCostUpgradeIncome();
        DataManager.HeroData.SetUpgradeIncomeSuccess().Save();
        costUpgradeIncomeText.text = GetCostUpgradeIncomeTextValue();

        LogTool.LogEditorOnly($"Level: {DataManager.HeroData.inComeLevel} --- {DataManager.HeroData.GetMinIncome().Convert()}");
    }

    private void ShowSellValue(bool show)
    {
        sellValueObj.SetActive(show);
        sellTextObj.SetActive(!show);
        sellValueText.text = GetSellItemTextValue();
    }

    public void ShowBossPopup()
    {
        PopupManager.Instance.Dequeue("BossPopup").Show();
    }
}
