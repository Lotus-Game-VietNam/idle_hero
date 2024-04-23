using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class RenderModel : IPool<MonoBehaviour>
{
    [Title("Configuration")]
    public Vector3 localPosition;
    public Vector3 localEulerAngle;
    public Vector3 localScale;

    public override bool autoHide => false;

    public override Action HideAct => this.PushRenderModel;

    public void Initialized()
    {
        
    }

    protected override void Initialized(MonoBehaviour data)
    {
        transform.localPosition = localPosition;
        transform.localEulerAngles = localEulerAngle;
        transform.localScale = localScale;
    }

    protected override void OnHide()
    {
        
    }

    protected override void OnShow()
    {
        
    }
}
