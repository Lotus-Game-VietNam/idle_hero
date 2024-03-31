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

    [Title("Object Reference")]
    public Image dimed = null;


    private Button _button = null;
    public Button button => this.TryGetComponent(ref _button);


    public Action OnCountingDownCussess = null;



    public void CountingDown()
    {
        if (currentState == State.CountingDown)
            return;

        dimed.rectTransform.localScale = Vector3.one;
        dimed.enabled = true;

        button.interactable = false;

        currentState = State.CountingDown;

        dimed.rectTransform.DOScaleY(0.1f, duration).OnComplete(() =>
        {
            dimed.enabled = false;
            button.interactable = true;
            currentState = State.None;
            OnCountingDownCussess?.Invoke();
        });
    }
}
