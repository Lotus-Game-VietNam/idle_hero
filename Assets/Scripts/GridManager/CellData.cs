using UnityEngine;

namespace Lotus.CoreFramework
{
    public class CellData
    {
        public CellPosition cellPosition;

        public Vector3 worldPosition;

        public InventoryItem itemOnCell { get; private set; }


        public CellData(CellPosition cellPosition, Vector3 worldPosition)
        {
            this.cellPosition = cellPosition;
            this.worldPosition = worldPosition;
        }

        public void MatchingItem(InventoryItem item) => itemOnCell = item;
    }
}

