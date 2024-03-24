using Lotus.CoreFramework;
using System;

public class InventoryItem : IPool<ItemData>
{
    public override bool autoHide => false;

    public override Action HideAct => this.PushItem;

    protected override void Initialized(ItemData data)
    {
        
    }

    protected override void OnHide()
    {
        
    }

    protected override void OnShow()
    {
        
    }
}
