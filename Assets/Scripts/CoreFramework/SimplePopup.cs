using Lotus.CoreFramework;
using TMPro;

public class SimplePopup : PopupBase
{
    public TMP_Text title = null;
    public TMP_Text message = null;


    protected override void UpdateContent()
    {
        title.text = "Warning";
        message.text = "No message";
    }
}
