using Lotus.CoreFramework;
using Sirenix.OdinInspector;
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


    public CellData[,] cells { get; private set; }

    public float cellLenght { get; private set; }

    public float cellOffset { get; private set; }

    public Vector3 pivotLeftBottomGrid { get; private set; }




    private void Awake()
    {
        Vector3 pointToRaycast = transform.position + transform.up * 10;
        if (!Physics.Raycast(pointToRaycast, transform.up * -1, out RaycastHit hit, 20, surfaceMask)) return;
        Vector3 centerOnSurface = hit.point;
        cellOffset = transform.GetChild(0).localScale.x / 30f;
        cellLenght = transform.GetChild(0).localScale.x / gridSizeX;
        pivotLeftBottomGrid = centerOnSurface - (transform.forward * ((gridSizeY / 2f) - 0.5f) * cellLenght) - (transform.right * ((gridSizeX / 2f) - 0.5f) * cellLenght);

        cells = new CellData[gridSizeX, gridSizeY];
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                CellData cell = new CellData(pivotLeftBottomGrid + (transform.right * x * cellLenght) + transform.forward * y * cellLenght);
                cells[x, y] = cell;
                this.DequeueItem($"{(ItemType)Random.Range(0, 3)}_1", itemsParant).SetPosition(cell.worldPosition + (Vector3.up * cellOffset * 2)).SetRotation(transform.rotation).Show();
            }
        }
    }


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
