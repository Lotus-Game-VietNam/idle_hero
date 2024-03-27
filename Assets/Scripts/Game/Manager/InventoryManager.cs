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
            SpawnItem(item.Value, cells[item.Key.posX, item.Key.posY], false);
    }

    private void InitEvents()
    {
        this.AddListener(EventName.BuyItem, BuyItem);
    }

    private void SpawnItem(ItemData itemData, CellData cell, bool saveData = true)
    {
        InventoryItem item = this.DequeueItem($"{itemData.itemType}_1", itemsParant);

        item.SetPosition(cell.worldPosition + (Vector3.up * cellOffset * 2)).SetRotation(transform.rotation).Initial(itemData).Show();

        cell.MatchingItem(item);
        items.Add(item);

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
