public class BuyItemsData
{
    public readonly int costBuyItemOnTutorial = 50;
    public readonly int costBuyItemFirstTime = 100;

    public int lastCostBuyItem;


    /// <summary>
    /// User đã mua vật phẩm lần thứ bao nhiêu
    /// </summary>
    public int buyItemsCount;



    public BuyItemsData()
    {
        buyItemsCount = 0;
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

        return this;
    }
}
