using System;

public class ItemData
{
    public string itemType;

    public int itemLevel;

    public float valueAdded;


    public CharacterAttributes AttributeAdded => (CharacterAttributes)(int)ItemType;

    public ItemType ItemType => (ItemType)Enum.Parse(typeof(ItemType), itemType);


    public ItemData()
    {
        
    }
}
