using DG.Tweening;
using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System;
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

    private Transform _mainCamera = null;
    public Transform mainCamera 
    {
        get
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main.transform;
            return _mainCamera;
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

    private void SpawnItem(ItemData itemData, CellData cell, bool saveData = true, Action<InventoryItem> OnShowComplete = null)
    {
        InventoryItem item = this.DequeueItem($"{itemData.itemType}_{itemData.itemLevel}", itemsParant);

        item.SetPosition(cell.worldPosition + (Vector3.up * cellOffset * 2)).SetRotation(transform.rotation).Initial(itemData).Show();
        OnShowComplete?.Invoke(item);

        cell.MatchingItem(item);
        items.Add(item);

        item.dragAndDrop.OnTouchEvent = OnTouchItem;
        item.dragAndDrop.OnDoubleTouchEvent = OnDoubleTouchItem;
        item.dragAndDrop.OnDragEvent = OnItemDrag;
        item.dragAndDrop.OnDropEvent = OnItemDrop;

        if (saveData)
            DataManager.InventoryData.SaveItem(cell.cellPosition, itemData).Save();
    }

    private void DoPunchScaleItem(InventoryItem item, float punchValue = 0.2f, float punchDuration = 0.5f)
    {
        item.transform.DOPunchScale(Vector3.one * punchValue, punchDuration).SetEase(Ease.InOutElastic);
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
            SpawnItem(itemData, cell, true, (_item) => { DoPunchScaleItem(_item, 0.2f, 0.25f); });
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
        int randomValue = UnityEngine.Random.Range(0, 100);
        Ratio[] ratios = ConfigManager.GetCurrentBuyItemRatio();

        for (int i = 0; i < ratios.Length; i++)
        {
            if (randomValue >= ratios[i].min && randomValue < ratios[i].max)
                return i;
        }

        return 0;
    }

    private ItemType GetRandomItemType() => (ItemType)UnityEngine.Random.Range(0, 3);

    #endregion



    #region Drag And Merge Items

    public CellData selectedCell { get; private set; }


    private CellData GetCell(int posX, int posY) => posX >= gridSizeX || posY >= gridSizeY ? null : cells[posX, posY];

    private CellData GetCell(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(Vector3.Project(worldPosition - pivotLeftBottomGrid, transform.right).magnitude / cellLenght);
        int y = Mathf.RoundToInt(Vector3.Project(worldPosition - pivotLeftBottomGrid, transform.forward).magnitude / cellLenght);

        if (x < 0 || y < 0 || x >= gridSizeX || y >= gridSizeY)
            return null;

        return cells[x, y];
    }

    private void OnMergeItems(InventoryItem selectedItem, InventoryItem dropedItem, CellData dropedCell)
    {
        if (selectedItem.data.itemLevel >= ConfigManager.GameConfig.ItemsConfig[selectedItem.data.ItemType].Length ||
            dropedItem.data.itemLevel >= ConfigManager.GameConfig.ItemsConfig[dropedItem.data.ItemType].Length)
        {
            selectedItem.dragAndDrop.RevertToPrevPos();
            return;
        }

        this.DequeueEffect("MergeSuccess", transform).SetPosition(dropedCell.worldPosition + ((mainCamera.position - dropedCell.worldPosition).normalized * 1.5f)).SetRotation(transform.rotation).Show();

        selectedItem.HideAct.Invoke();
        dropedItem.HideAct.Invoke();

        items.Remove(selectedItem);
        items.Remove(dropedItem);

        selectedCell.MatchingItem(null);
        DataManager.InventoryData.SaveItem(selectedCell.cellPosition, null).Save();
        SpawnItem(ConfigManager.GetItem(selectedItem.data.ItemType, selectedItem.data.itemLevel + 1), dropedCell, true, (_item) => { DoPunchScaleItem(_item, 0.5f); });
    }

    private void ChangeItemPosition(InventoryItem selectedItem, CellData dropedCell)
    {
        selectedItem.dragAndDrop.MoveToPos(dropedCell.worldPosition + (Vector3.up * cellOffset * 2));

        dropedCell.MatchingItem(selectedItem);
        selectedCell.MatchingItem(null);

        DataManager.InventoryData.SaveItem(selectedCell.cellPosition, null).SaveItem(dropedCell.cellPosition, selectedItem.data).Save();
    }

    private void ExchangeItemsPosition(InventoryItem selectedItem, InventoryItem dropedItem, CellData dropedCell)
    {
        selectedItem.dragAndDrop.MoveToPos(dropedCell.worldPosition + (Vector3.up * cellOffset * 2));
        dropedItem.dragAndDrop.MoveToPos(selectedCell.worldPosition + (Vector3.up * cellOffset * 2));

        dropedCell.MatchingItem(selectedItem);
        selectedCell.MatchingItem(dropedItem);

        DataManager.InventoryData.SaveItem(selectedCell.cellPosition, dropedItem.data).SaveItem(dropedCell.cellPosition, selectedItem.data).Save();
    }

    private void OnItemDrop(IDragAndDrop<InventoryItem> item)
    {
        CellData dropedCell = GetCell(item.worldPosition);

        InventoryItem selectedItem = item.data;
        InventoryItem dropedItem = dropedCell?.itemOnCell;

        if (dropedCell == null || selectedCell == dropedCell)
            item.RevertToPrevPos();
        else if (dropedItem == null)
            ChangeItemPosition(selectedItem, dropedCell);
        else if (selectedItem.data.itemType.Equals(dropedItem.data.itemType) && selectedItem.data.itemLevel == dropedItem.data.itemLevel)
            OnMergeItems(selectedItem, dropedItem, dropedCell);
        else
            ExchangeItemsPosition(selectedItem, dropedItem, dropedCell);

        selectedCell = null;
    }

    private void OnItemDrag(IDragAndDrop<InventoryItem> item)
    {
        //CellData cell = GetCell(item.worldPosition);
        //if (cell == null) return;
        //LogTool.LogErrorEditorOnly($"PosX: {cell.cellPosition.posX} --- PosY: {cell.cellPosition.posY}");
    }

    private void OnTouchItem(IDragAndDrop<InventoryItem> item)
    {
        selectedCell = GetCell(item.worldPosition);
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
