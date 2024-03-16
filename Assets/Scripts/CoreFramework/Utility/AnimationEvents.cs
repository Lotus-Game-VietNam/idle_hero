using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public Action OnNormalShotEvent = null;


    public void OnNormalShot()
    {
        OnNormalShotEvent?.Invoke();
    }
}
