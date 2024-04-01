using Lotus.CoreFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroData : CharacterConfig
{
    public Dictionary<ItemType, ItemData> items = null;
    public int inComeLevel;


    public readonly int minIncomeOnTutorial = 5;
    public readonly int minIncomeFirstLevel = 10;

    public readonly int costUpgradeIncomeOnTutorial = 200;


    public float lastMinIncome;
    public float lastCostUpgradeIncome;


    public HeroData() : base()
    {
        items = new Dictionary<ItemType, ItemData>();
        int count = Enum.GetValues(typeof(ItemType)).Length;
        for (int i = 0; i < count; i++)
            items.Add((ItemType)i, ConfigManager.GetItem((ItemType)i, 0));
        inComeLevel = 0;
    }

    public override Dictionary<CharacterAttributes, float> GetAttributes()
    {
        if (items == null)
            return null;

        Dictionary<CharacterAttributes, float> result = new Dictionary<CharacterAttributes, float>();
        foreach (var item in items)
            result.Add(item.Value.AttributeAdded, item.Value.valueAdded);
        return result;
    }

    public float GetMinIncome()
    {
        if (inComeLevel <= 1)
            return inComeLevel == 0 ? minIncomeOnTutorial : minIncomeFirstLevel;
        return Mathf.RoundToInt(lastMinIncome * 1.4f);
    }

    public float GetMaxIncome() => GetMinIncome() * 2;

    public float GetCostUpgradeIncome() => inComeLevel == 0 ? costUpgradeIncomeOnTutorial : (inComeLevel * 100) + lastCostUpgradeIncome + 200;

    public HeroData SetUpgradeIncomeSuccess()
    {
        lastMinIncome = GetMinIncome();
        lastCostUpgradeIncome = GetCostUpgradeIncome();
        inComeLevel++;
        return this;
    }
}
