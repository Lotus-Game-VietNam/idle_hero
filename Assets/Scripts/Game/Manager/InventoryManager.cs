using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Title("Grid Setting")]
    [SerializeField] private LayerMask surfaceMask;
    [SerializeField] private int gridSizeX = 4;
    [SerializeField] private int gridSizeY = 3;


    private Transform _itemsParent = null;
    public Transform itemsParant 
    {
        get
        {
            if (_itemsParent == null)
                _itemsParent = transform.Find("Items");
            return _itemsParent;
        }
    }


    public int maxItemCount => gridSizeX * gridSizeY;


    public List<InventoryItem> items { get; private set; }

    public CellData[,] cells { get; private set; }

    public float cellLenght { get; private set; }

    public float cellOffset { get; private set; }

    public Vector3 pivotLeftBottomGrid { get; private set; }




    private void Awake()
    {
        InitEvents();
        GenerateGrid();
        GenerateItems();
    }


    #region Constructor

    private void GenerateGrid()
    {
        Vector3 pointToRaycast = transform.position + transform.up * 10;
        if (!Physics.Raycast(pointToRaycast, transform.up * -1, out RaycastHit hit, 20, surfaceMask)) return;

        Vector3 centerOnSurface = hit.point;
        Transform inventoryMesh = transform.GetChild(0);

        cellOffset = inventoryMesh.localScale.x / 30f;
        cellLenght = inventoryMesh.localScale.x / gridSizeX;
        pivotLeftBottomGrid = centerOnSurface - (transform.forward * ((gridSizeY / 2f) - 0.5f) * cellLenght) - (transform.right * ((gridSizeX / 2f) - 0.5f) * cellLenght);

        cells = new CellData[gridSizeX, gridSizeY];
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                CellData cell = new CellData(new CellPosition(x, y), pivotLeftBottomGrid + (transform.right * x * cellLenght) + transform.forward * y * cellLenght);
                cells[x, y] = cell;
            }
        }
    }

    private void GenerateItems()
    {
        items = new List<InventoryItem>();

        foreach (var item in DataManager.InventoryData.items)
        {
            if (item.Value == null) continue;
            SpawnItem(item.Value, cells[item.Key.posX, item.Key.posY], false);
        }
    }

    private void InitEvents()
    {
        this.AddListener(EventName.BuyItem, BuyItem);
    }

    #endregion



    #region Buy Items

    private void SpawnItem(ItemData itemData, CellData cell, bool saveData = true)
    {
        InventoryItem item = this.DequeueItem($"{itemData.itemType}_{itemData.itemLevel}", itemsParant);

        item.SetPosition(cell.worldPosition + (Vector3.up * cellOffset * 2)).SetRotation(transform.rotation).Initial(itemData).Show();
        item.SetCellPosition(cell.cellPosition);

        cell.MatchingItem(item);
        items.Add(item);

        item.dragAndDrop.OnTouchEvent = OnTouchItem;
        item.dragAndDrop.OnDoubleTouchEvent = OnDoubleTouchItem;
        item.dragAndDrop.OnDragEvent = OnItemDrag;
        item.dragAndDrop.OnDropEvent = OnItemDrop;

        if (saveData)
            DataManager.InventoryData.SaveItem(cell.cellPosition, itemData).Save();
    }

    private void BuyItem()
    {
        if (items.Count >= maxItemCount)
            return;

        float costToBuy = DataManager.InventoryData.GetCostToBuyItem();

        if (ResourceManager.Gem < costToBuy)
            return;

        ResourceManager.Gem -= costToBuy;
        DataManager.InventoryData.SetBuyItemSuccess().Save();

        foreach (var cell in cells)
        {
            if (cell.itemOnCell != null)
                continue;

            ItemData itemData = GetRandomItemData();
            SpawnItem(itemData, cell);
            //LogTool.LogErrorEditorOnly($"Level Pool: {DataManager.BuyItemsData.currentLevelPool} --- Item Type: {itemData.itemType} --- Item Level: {itemData.itemLevel}");
            break;
        }

        this.SendMessage(EventName.BuyItem, "UIFarmManager");
    }

    private ItemData GetRandomItemData()
    {
        ItemType type = GetRandomItemType();
        int level = GetRandomItemLevel();
        return ConfigManager.GetItem(type, level);
    }

    private int GetRandomItemLevel()
    {
        int randomValue = Random.Range(0, 100);
        Ratio[] ratios = ConfigManager.GetCurrentBuyItemRatio();

        for (int i = 0; i < ratios.Length; i++)
        {
            if (randomValue >= ratios[i].min && randomValue < ratios[i].max)
                return i;
        }

        return 0;
    }

    private ItemType GetRandomItemType() => (ItemType)Random.Range(0, 3);

    #endregion



    #region Drag And Merge Items

    private CellData GetCell(int posX, int posY) => posX >= gridSizeX || posY >= gridSizeY ? null : cells[posX, posY];

    private CellData GetCell(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(Vector3.Project(worldPosition - pivotLeftBottomGrid, transform.right).magnitude / cellLenght);
        int y = Mathf.RoundToInt(Vector3.Project(worldPosition - pivotLeftBottomGrid, transform.forward).magnitude / cellLenght);

        if (x < 0 || y < 0)
            return null;

        return cells[x, y];
    }

    private void OnMergeItems(InventoryItem selectedItem, InventoryItem dropedItem, CellData dropedCell)
    {
        selectedItem.HideAct.Invoke();
        dropedItem.HideAct.Invoke();
        DataManager.InventoryData.SaveItem(selectedItem.cellPosition, null).Save();
        SpawnItem(ConfigManager.GetItem(selectedItem.data.ItemType, selectedItem.data.itemLevel + 1), dropedCell);
    }

    private void ChangedItemPosition(InventoryItem selectedItem, CellData dropedCell)
    {
        selectedItem.dragAndDrop.MoveToPos(dropedCell.worldPosition + (Vector3.up * cellOffset * 2));
        DataManager.InventoryData.SaveItem(selectedItem.cellPosition, null).SaveItem(dropedCell.cellPosition, selectedItem.data).Save();
        selectedItem.SetCellPosition(dropedCell.cellPosition);
    }

    private void OnItemDrop(IDragAndDrop<InventoryItem> item)
    {
        CellData dropedCell = GetCell(item.worldPosition);

        InventoryItem selectedItem = item.data;
        InventoryItem dropedItem = dropedCell.itemOnCell;

        if (dropedItem == null)
            ChangedItemPosition(selectedItem, dropedCell);
        else if (selectedItem.data.itemType.Equals(dropedItem.data.itemType) && selectedItem.data.itemLevel == dropedItem.data.itemLevel)
            OnMergeItems(selectedItem, dropedItem, dropedCell);
        else
            item.RevertToPrevPos();

    }

    private void OnItemDrag(IDragAndDrop<InventoryItem> item)
    {
        //CellData cell = GetCell(item.worldPosition);
        //if (cell == null) return;
        //LogTool.LogErrorEditorOnly($"PosX: {cell.cellPosition.posX} --- PosY: {cell.cellPosition.posY}");
    }

    private void OnTouchItem(IDragAndDrop<InventoryItem> item)
    {

    }

    private void OnDoubleTouchItem(IDragAndDrop<InventoryItem> item)
    {

    }

    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(pivotLeftBottomGrid, 0.05f);

        if (cells == null) return;

        foreach (CellData cell in cells)
        {
            Gizmos.DrawSphere(cell.worldPosition, 0.05f);
        }
    }
}
