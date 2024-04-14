using System;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public Action<int> OnShotEvent = null;

    public Action OnShotFinishEvent = null;


    public void OnShot(int type)
    {
        OnShotEvent?.Invoke(type);
    }

    public void OnShotFinish()
    {
        OnShotFinishEvent?.Invoke();
    }
}
