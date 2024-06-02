using Lotus.CoreFramework;
using UnityEngine;

public class BossPopup : PopupBase
{


    private Transform _renderParent = null;
    public Transform renderParent 
    { 
        get
        {
            if (_renderParent == null)
                _renderParent = ComponentReference.BossRenderParent.Invoke();
            return _renderParent;
        }
    }


    public RenderModel currentModelRender { get; private set; }



    protected override void UpdateContent()
    {
        if (currentModelRender == null)
            currentModelRender = this.DequeueRenderModel($"Boss_{((DataManager.WorldData.currentLevel - 1) / 3) + 1}", renderParent);

        currentModelRender.Initial(null).Show();
    }


    public void Fighting()
    {
        this.LoadSceneAsync(Utilities.GetCurrentBattleScene(), 0.5f);
    }
}
