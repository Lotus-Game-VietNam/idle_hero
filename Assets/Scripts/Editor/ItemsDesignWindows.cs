using Lotus.CoreFramework;
using UnityEditor;
using UnityEngine;

public class ItemsDesignWindows : EditorWindow
{
    public ItemType ItemType;
    public int itemLevel;


    private InventoryManager inventoryManager = null;


    [MenuItem("Tools/Items Design")]
    public static void OpenWindow()
    {
        ItemsDesignWindows window = (ItemsDesignWindows)GetWindow(typeof(ItemsDesignWindows));
        window.minSize = new UnityEngine.Vector2(600, 1000);
        window.Show();
    }


    private void OnGUI()
    {
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Chọn loại item: ");
        ItemType = (ItemType)EditorGUILayout.EnumPopup(ItemType);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Nhập level item: ");
        itemLevel = EditorGUILayout.IntField(itemLevel);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("SPAWN ITEM"))
        {
            if (inventoryManager == null)
            {
                var manager = GameObject.FindGameObjectWithTag("InventoryManager");
                inventoryManager = manager.GetComponent<InventoryManager>();
            }

            inventoryManager.SpawnItemEditor(ConfigManager.GetItem(ItemType, itemLevel));
        }


    }
}
