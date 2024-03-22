using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using UnityEngine;

public class EffectBase : IPool<EffectData>
{
    [Title("Configuration")]
    public float hideAffterTime = 2f;



    private Coroutine hideCrt = null;


    public EffectData data { get; private set; }




    protected override void Initialized(EffectData data)
    {
        this.data = data;
        transform.position = data.showPoint;
    }

    protected override void OnHide()
    {
        
    }

    protected override void OnShow()
    {
        WaitToHide();
    }

    private void WaitToHide()
    {
        if (hideCrt != null)
            StopCoroutine(hideCrt);
        hideCrt = this.DelayCall(hideAffterTime, () =>
        {
            this.PushEffect();
        });
    }
}
