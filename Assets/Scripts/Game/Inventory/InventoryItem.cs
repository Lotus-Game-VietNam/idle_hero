using Lotus.CoreFramework;
using System;
using TMPro;

public class InventoryItem : IPool<ItemData>
{
    public override bool autoHide => false;

    public override Action HideAct => this.PushItem;


    private TMP_Text _levelText = null;
    public TMP_Text levelText => this.TryGetComponentInChildren(ref _levelText);



    private ItemDragAndDrop _dragAndDrop = null;
    public ItemDragAndDrop dragAndDrop => this.TryGetComponent(ref _dragAndDrop);



    protected override void Initialized(ItemData data)
    {
        dragAndDrop.Initialized(this);
        levelText.text = (data.itemLevel + 1).ToString();
    }

    protected override void OnHide()
    {
        
    }

    protected override void OnShow()
    {
        
    }
}
