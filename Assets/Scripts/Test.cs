using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{


    private void Awake()
    {

    }

    [Button("GetPopup")]
    public void GetDemoPopup()
    {
        PopupManager.Instance.Dequeue("DemoPopup").Show();
    }

    [Button("HidePopup")]
    public void HideDemoPopup()
    {
        PopupManager.Instance.GetPopupVisible("DemoPopup").Hide();
    }
}
