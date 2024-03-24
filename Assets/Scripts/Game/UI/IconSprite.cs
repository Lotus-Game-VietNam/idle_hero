using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class IconSprite : IPool<IconData>
{
    [Title("Configuration")]
    public float hideAffterTime = 3f;


    public override bool autoHide => true;

    public override float timeToHide => hideAffterTime;

    public override Action HideAct => this.PushIcon;



    protected override void Initialized(IconData data)
    {

    }

    protected override void OnHide()
    {
        
    }

    protected override void OnShow()
    {
        
    }
}
