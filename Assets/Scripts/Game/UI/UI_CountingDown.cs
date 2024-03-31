using DG.Tweening;
using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_CountingDown : MonoUI
{
    public enum State { None, CountingDown }

    [Title("Configuration")]
    public State currentState = State.None;
    public float duration = 60f;


    private Image _dimed = null;
    public Image dimed => this.TryGetComponent(ref _dimed);


    public Action OnCountingDownCussess = null;



    public void CountingDown()
    {
        rect.localScale = Vector3.one;
        dimed.enabled = true;

        currentState = State.CountingDown;

        rect.DOScaleY(0.1f, duration).OnComplete(() =>
        {
            dimed.enabled = false;
            currentState = State.None;
            OnCountingDownCussess?.Invoke();
        });
    }
}
