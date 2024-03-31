using DG.Tweening;
using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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


    public Dictionary<ItemType, Dictionary<int, List<InventoryItem>>> items { get; private set; }

    public Dictionary<CellPosition, EffectBase> canMergeVfxs { get; private set; }

    public CellData[,] cells { get; private set; }

    public float cellLenght { get; private set; }

    public float cellOffset { get; private set; }

    public Vector3 pivotLeftBottomGrid { get; private set; }




    private void Awake()
    {
        InitEvents();
        GenerateGrid();
        GenerateItems();
        GenerateCanMergeVfs();
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
        items = new Dictionary<ItemType, Dictionary<int, List<InventoryItem>>>();
        canMergeVfxs = new Dictionary<CellPosition, EffectBase>();

        foreach (var item in DataManager.InventoryData.items)
        {
            if (item.Value == null) continue;
            SpawnItem(item.Value, cells[item.Key.posX, item.Key.posY], false, false);
        }
    }

    private void InitEvents()
    {
        this.AddListener(EventName.BuyItem, BuyItem);
    }

    #endregion



    #region Buy Items

    private void SpawnItem(ItemData itemData, CellData cell, bool saveData = true, bool updateCanMergeVfx = true, Action<InventoryItem> OnShowComplete = null)
    {
        InventoryItem item = this.DequeueItem($"{itemData.itemType}_{itemData.itemLevel}", itemsParant);

        item.SetPosition(cell.worldPosition + (Vector3.up * cellOffset * 2)).SetRotation(transform.rotation).SetLocalScale(Vector3.one).Initial(itemData).Show();

        OnShowComplete?.Invoke(item);

        cell.MatchingItem(item);

        ItemType itemType = itemData.ItemType;

        if (!items.ContainsKey(itemType))
            items.Add(itemType, new Dictionary<int, List<InventoryItem>>());

        if (!items[itemType].ContainsKey(itemData.itemLevel))
            items[itemType].Add(itemData.itemLevel, new List<InventoryItem>());

        items[itemType][itemData.itemLevel].Add(item);

        item.dragAndDrop.OnTouchEvent = OnTouchItem;
        item.dragAndDrop.OnDoubleTouchEvent = OnDoubleTouchItem;
        item.dragAndDrop.OnDragEvent = OnItemDrag;
        item.dragAndDrop.OnDropEvent = OnItemDrop;

        if (saveData)
            DataManager.InventoryData.SaveItem(cell.cellPosition, itemData).Save();

        if (updateCanMergeVfx)
            UpdateCanMergeVfxsOnSpawnNewItem(item, cell);
    }

    private void DoPunchScaleItem(InventoryItem item, float punchValue = 0.2f, float punchDuration = 0.5f)
    {
        item.transform.DOPunchScale(Vector3.one * punchValue, punchDuration).SetEase(Ease.InOutElastic);
    }

    private void BuyItem()
    {
        if (TotalItems() >= maxItemCount)
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
            SpawnItem(itemData, cell, true, true, (_item) => { DoPunchScaleItem(_item, 0.2f, 0.25f); });
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

    private int TotalItems()
    {
        int count = 0;
        foreach (var itemByType in items)
        {
            foreach (var itemByLevel in itemByType.Value)
            {
                count += itemByLevel.Value.Count;
            }
        }
        return count;
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

        ItemType itemType = selectedItem.data.ItemType;
        items[itemType][selectedItem.data.itemLevel].Remove(selectedItem);
        items[itemType][dropedItem.data.itemLevel].Remove(dropedItem);

        selectedCell.MatchingItem(null);
        DataManager.InventoryData.SaveItem(selectedCell.cellPosition, null).Save();

        UpdateCanMergeVfxsOnMergeItems(selectedItem, dropedCell);
        SpawnItem(ConfigManager.GetItem(selectedItem.data.ItemType, selectedItem.data.itemLevel + 1), dropedCell, true, true, (_item) => { DoPunchScaleItem(_item, 0.5f); });
    }

    private void ChangeItemPosition(InventoryItem selectedItem, CellData dropedCell)
    {
        selectedItem.dragAndDrop.MoveToPos(dropedCell.worldPosition + (Vector3.up * cellOffset * 2));

        dropedCell.MatchingItem(selectedItem);
        selectedCell.MatchingItem(null);

        UpdateCanMergeVfxOnChangeItemPosition(selectedCell, dropedCell);

        DataManager.InventoryData.SaveItem(selectedCell.cellPosition, null).SaveItem(dropedCell.cellPosition, selectedItem.data).Save();
    }

    private void ExchangeItemsPosition(InventoryItem selectedItem, InventoryItem dropedItem, CellData dropedCell)
    {
        selectedItem.dragAndDrop.MoveToPos(dropedCell.worldPosition + (Vector3.up * cellOffset * 2));
        dropedItem.dragAndDrop.MoveToPos(selectedCell.worldPosition + (Vector3.up * cellOffset * 2));

        dropedCell.MatchingItem(selectedItem);
        selectedCell.MatchingItem(dropedItem);

        UpdateCanMergeVfxsOnExchangeItemsPosition(dropedCell);

        DataManager.InventoryData.SaveItem(selectedCell.cellPosition, dropedItem.data).SaveItem(dropedCell.cellPosition, selectedItem.data).Save();
    }

    private void SellItem(InventoryItem selectedItem, RectTransform sellButton)
    {
        selectedItem.HideAct.Invoke();

        ItemType itemType = selectedItem.data.ItemType;
        items[itemType][selectedItem.data.itemLevel].Remove(selectedItem);

        selectedCell.MatchingItem(null);
        DataManager.InventoryData.SaveItem(selectedCell.cellPosition, null).Save();

        UpdateCanMergeVfxsOnSellItem(selectedItem);

        CollectionIcons.Instance.Show(10, sellButton.transform.position);
        float gemsToAdd = DataManager.InventoryData.GetValueToSellItem();

        this.DelayCall(1, () => { ResourceManager.Gem += gemsToAdd; });

        DataManager.InventoryData.SetSellItemSuccess().Save();
    }

    private RectTransform GetSellButton()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (var result in results)
        {
            GameObject hitObject = result.gameObject;
            if (!hitObject.CompareTag("SellButton")) continue;
            return hitObject.GetComponent<RectTransform>();
        }

        return null;
    }

    private void RevertToPrevPos(IDragAndDrop<InventoryItem> item)
    {
        item.RevertToPrevPos();

        if (canMergeVfxs.ContainsKey(selectedCell.cellPosition))
            canMergeVfxs[selectedCell.cellPosition].gameObject.SetActive(true);
    }

    private void OnItemDrop(IDragAndDrop<InventoryItem> item)
    {
        CellData dropedCell = GetCell(item.worldPosition);

        InventoryItem selectedItem = item.data;
        InventoryItem dropedItem = dropedCell?.itemOnCell;
        RectTransform sellButton = GetSellButton();

        if (sellButton != null)
            SellItem(selectedItem, sellButton);
        else if (dropedCell == null || selectedCell == dropedCell)
            RevertToPrevPos(item);
        else if (dropedItem == null)
            ChangeItemPosition(selectedItem, dropedCell);
        else if (selectedItem.data.itemType.Equals(dropedItem.data.itemType) && selectedItem.data.itemLevel == dropedItem.data.itemLevel)
            OnMergeItems(selectedItem, dropedItem, dropedCell);
        else
            ExchangeItemsPosition(selectedItem, dropedItem, dropedCell);

        this.SendMessage(EventName.ShowShellValue, false);

        selectedCell = null;
    }

    private void OnItemDrag(IDragAndDrop<InventoryItem> item)
    {

    }

    private void OnTouchItem(IDragAndDrop<InventoryItem> item)
    {
        selectedCell = GetCell(item.worldPosition);

        if (canMergeVfxs.ContainsKey(selectedCell.cellPosition))
            canMergeVfxs[selectedCell.cellPosition].gameObject.SetActive(false);

        this.SendMessage(EventName.ShowShellValue, true);
    }

    private void OnDoubleTouchItem(IDragAndDrop<InventoryItem> item)
    {

    }

    #endregion



    #region VFX

    private void UpdateCanMergeVfxsOnSpawnNewItem(InventoryItem item, CellData cell)
    {
        ItemType itemType = item.data.ItemType;

        if (items[itemType][item.data.itemLevel].Count < 2)
            return;

        if (items[itemType][item.data.itemLevel].Count == 2)
            SpawnCanMergeVfx(items[itemType][item.data.itemLevel][0], GetCell(items[itemType][item.data.itemLevel][0].transform.position));

        SpawnCanMergeVfx(item, cell);
    }

    private void UpdateCanMergeVfxOnChangeItemPosition(CellData selectedCell, CellData dropedCell)
    {
        if (canMergeVfxs.ContainsKey(selectedCell.cellPosition))
        {
            EffectBase canMergeVfx = canMergeVfxs[selectedCell.cellPosition];
            canMergeVfx.SetPosition(dropedCell.worldPosition + (Vector3.up * cellOffset * 2));
            canMergeVfxs.Remove(selectedCell.cellPosition);
            canMergeVfxs.Add(dropedCell.cellPosition, canMergeVfx);
            canMergeVfx.gameObject.SetActive(true);
        }
    }

    private void UpdateCanMergeVfxsOnExchangeItemsPosition(CellData dropedCell)
    {
        if (!canMergeVfxs.ContainsKey(selectedCell.cellPosition) && !canMergeVfxs.ContainsKey(dropedCell.cellPosition))
            return;

        if (canMergeVfxs.ContainsKey(selectedCell.cellPosition) && canMergeVfxs.ContainsKey(dropedCell.cellPosition))
        {
            canMergeVfxs[selectedCell.cellPosition].gameObject.SetActive(true);
            return;
        }

        if (canMergeVfxs.ContainsKey(selectedCell.cellPosition))
            UpdateCanMergeVfxOnChangeItemPosition(selectedCell, dropedCell);
        else
            UpdateCanMergeVfxOnChangeItemPosition(dropedCell, selectedCell);
    }

    private void UpdateCanMergeVfxsOnMergeItems(InventoryItem selectedItem, CellData dropedCell)
    {
        if (canMergeVfxs.ContainsKey(selectedCell.cellPosition))
        {
            this.PushEffect(canMergeVfxs[selectedCell.cellPosition]);
            canMergeVfxs.Remove(selectedCell.cellPosition);
        }

        if (canMergeVfxs.ContainsKey(dropedCell.cellPosition))
        {
            this.PushEffect(canMergeVfxs[dropedCell.cellPosition]);
            canMergeVfxs.Remove(dropedCell.cellPosition);
        }

        if (items[selectedItem.data.ItemType][selectedItem.data.itemLevel].Count == 1)
        {
            CellData cellData = GetCell(items[selectedItem.data.ItemType][selectedItem.data.itemLevel][0].transform.position);
            EffectBase vfx = canMergeVfxs[cellData.cellPosition];
            canMergeVfxs.Remove(cellData.cellPosition);
            this.PushEffect(vfx);
        }
    }

    private void UpdateCanMergeVfxsOnSellItem(InventoryItem selectedItem)
    {
        if (canMergeVfxs.ContainsKey(selectedCell.cellPosition))
        {
            this.PushEffect(canMergeVfxs[selectedCell.cellPosition]);
            canMergeVfxs.Remove(selectedCell.cellPosition);
        }

        if (items[selectedItem.data.ItemType][selectedItem.data.itemLevel].Count == 1)
        {
            CellData cellData = GetCell(items[selectedItem.data.ItemType][selectedItem.data.itemLevel][0].transform.position);
            EffectBase vfx = canMergeVfxs[cellData.cellPosition];
            canMergeVfxs.Remove(cellData.cellPosition);
            this.PushEffect(vfx);
        }
    }

    private void GenerateCanMergeVfs()
    {
        foreach (var itemByType in items)
        {
            foreach (var itemByLevel in itemByType.Value)
            {
                if (itemByLevel.Value.Count < 2)
                    break;
                foreach (var item in itemByLevel.Value)
                {
                    CellData cellData = GetCell(item.transform.position);
                    SpawnCanMergeVfx(item, cellData);
                }
            }
        }
    }

    private void SpawnCanMergeVfx(InventoryItem item, CellData cellData)
    {
        EffectBase vfx = this.DequeueEffect("CanMerge", transform);
        try
        {
            vfx.SetPosition(item.transform.position).SetRotation(transform.rotation).Show();
            canMergeVfxs.Add(cellData.cellPosition, vfx);
        }
        catch (Exception ex)
        {

        }
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
