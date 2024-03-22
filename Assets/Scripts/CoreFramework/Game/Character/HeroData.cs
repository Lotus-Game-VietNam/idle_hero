using Lotus.CoreFramework;
using System;
using System.Collections.Generic;

public class HeroData : CharacterConfig
{
    public Dictionary<ItemType, ItemData> items = null;
    public int inComeLevel;


    public HeroData() : base()
    {
        items = new Dictionary<ItemType, ItemData>();
        int count = Enum.GetValues(typeof(ItemType)).Length;
        for (int i = 0; i < count; i++)
            items.Add((ItemType)i, ConfigManager.GetItem((ItemType)i, 0));
        inComeLevel = 1;
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
}
