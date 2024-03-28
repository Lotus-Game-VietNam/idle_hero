using Lotus.CoreFramework;
using System;
using TMPro;

public class InventoryItem : IPool<ItemData>
{
    public override bool autoHide => false;

    public override Action HideAct => this.PushItem;


    public CellPosition cellPosition { get; private set; }


    private TMP_Text _levelText = null;
    public TMP_Text levelText => this.TryGetComponentInChildren(ref _levelText);



    private ItemDragAndDrop _dragAndDrop = null;
    public ItemDragAndDrop dragAndDrop => this.TryGetComponent(ref _dragAndDrop);


    private void Awake()
    {
        dragAndDrop.Initialized(this);
    }

    protected override void Initialized(ItemData data)
    {
        levelText.text = (data.itemLevel + 1).ToString();
    }

    protected override void OnHide()
    {
        
    }

    protected override void OnShow()
    {
        
    }

    public void SetCellPosition(CellPosition cellPosition) => this.cellPosition = cellPosition;
}
