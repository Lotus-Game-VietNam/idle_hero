using Lotus.CoreFramework;
using System;

public class CostumeModel : IPool<ItemData>
{
    public override bool autoHide => false;

    public override Action HideAct => this.PushCostumeModel;



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
