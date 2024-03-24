using UnityEditor;

public static class NNUIMenuItem
{
    [MenuItem("GameObject/LotusFramework/PopupManager")]
    public static void AddUIContext(MenuCommand menuCommand)
    {
        CreateUIContext.CreatePrefab("CoreFramework/Prefabs/PopupManager");
    }
}