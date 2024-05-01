using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System;

public class EffectBase : IPool<EffectData>
{
    [Title("Configuration")]
    public bool _autoHide = false;
    public float hideAffterTime = 2f;



    public override bool autoHide => _autoHide;
    public override float timeToHide => hideAffterTime;
    public override Action HideAct => this.PushEffect;


    protected override void Initialized(EffectData data)
    {

    }

    protected override void OnHide()
    {
        StopAllCoroutines();
    }

    protected override void OnShow()
    {

    }


}
