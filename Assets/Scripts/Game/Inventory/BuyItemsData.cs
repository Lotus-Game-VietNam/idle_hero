using Lotus.CoreFramework;
using System.Collections.Generic;
using UnityEngine;

public class BuyItemsData
{
    public readonly int costBuyItemOnTutorial = 50;
    public readonly int costBuyItemFirstTime = 100;

    public int lastCostBuyItem;


    /// <summary>
    /// User đã mua vật phẩm lần thứ bao nhiêu
    /// </summary>
    public int buyItemsCount;


    /// <summary>
    /// Đếm số lượng vật phẩm đã mua trong bể items, khi đã mua đủ số lượng của bể thì reset về 0 để đếm lại số lượng cho bể level tiếp theo
    /// </summary>
    public int buyItemsCountOnPool;


    /// <summary>
    /// Level của bể item, level này quyết định tỷ lệ mua ra vật phẩm level mấy
    /// </summary>
    public int currentLevelPool;


    /// <summary>
    /// Những item mà user đã mua, chính là các vật phẩm đang nằm trên hộp
    /// Key - Vị trí của vật phẩm trên hộp
    /// Value - Data của vật phẩm
    /// </summary>
    public Dictionary<CellPosition, ItemData> items = new Dictionary<CellPosition, ItemData>();



    public BuyItemsData()
    {
        buyItemsCount = 0;
        buyItemsCountOnPool = 0;
        currentLevelPool = 1;
    }

    public int GetCostToBuyItem()
    {
        if (buyItemsCount <= 1)
            return buyItemsCount == 0 ? costBuyItemOnTutorial : costBuyItemFirstTime;
        return lastCostBuyItem + (buyItemsCount - 1) + 10;
    }

    public BuyItemsData SetBuyItemSuccess()
    {
        lastCostBuyItem = GetCostToBuyItem();

        buyItemsCount++;

        buyItemsCountOnPool++;

        if (buyItemsCountOnPool == 12 + (3 * currentLevelPool))
        {
            buyItemsCountOnPool = 0;
            currentLevelPool = Mathf.Clamp(currentLevelPool + 1, 1, ConfigManager.GameConfig.BuyItemsRatio.Count);
        }

        return this;
    }

    public BuyItemsData SaveItem(CellPosition cellPosition, ItemData itemData, int maxItemsCount = 12)
    {
        if (!items.ContainsKey(cellPosition))
            items.Add(cellPosition, null);

        items[cellPosition] = itemData;

        return this;
    }
}
