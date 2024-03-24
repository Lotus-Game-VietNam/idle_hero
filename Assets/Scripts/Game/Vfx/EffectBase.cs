using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class EffectBase : IPool<EffectData>
{
    [Title("Configuration")]
    public float hideAffterTime = 2f;



    public override bool autoHide => true;
    public override float timeToHide => hideAffterTime;
    public override Action HideAct => this.PushEffect;


    protected override void Initialized(EffectData data)
    {

    }

    protected override void OnHide()
    {
        
    }

    protected override void OnShow()
    {

    }
}
