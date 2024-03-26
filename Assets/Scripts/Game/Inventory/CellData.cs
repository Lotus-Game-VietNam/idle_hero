using UnityEngine;

public class CellData
{
    public Vector3 worldPosition;

    public InventoryItem itemOnCell { get; private set; }


    public CellData(Vector3 worldPosition)
    {
        this.worldPosition = worldPosition;
    }

    public void MatchingItem(InventoryItem item) => itemOnCell = item;
}
