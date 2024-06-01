using System;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public Action<int> OnShotEvent = null;

    public Action OnShotFinishEvent = null;


    public Action Event1 = null;
    public Action Event2 = null;
    public Action Event3 = null;


    public void OnShot(int type)
    {
        OnShotEvent?.Invoke(type);
    }

    public void OnShotFinish()
    {
        OnShotFinishEvent?.Invoke();
    }


    public void Event1Func()
    {
        Event1?.Invoke();
    }

    public void Event2Func()
    {
        Event2?.Invoke();
    }

    public void Event3Func()
    {
        Event3?.Invoke();
    }
}
