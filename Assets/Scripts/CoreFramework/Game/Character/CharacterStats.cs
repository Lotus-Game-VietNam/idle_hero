using DG.Tweening;
using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [Title("Object Reference")]
    public Canvas canvas = null;
    public Slider HPBar = null;
    public Slider HPBGBar = null;


    private Tween hpTween = null;
    private Tween hpBGTween = null;


    public Dictionary<CharacterAttributes, float> attributes { get; private set; }

    public float MaxHP => attributes[CharacterAttributes.HP];

    public float ATK => attributes[CharacterAttributes.ATK];

    public float SPD => attributes.ContainsKey(CharacterAttributes.SPD) ? attributes[CharacterAttributes.SPD] : 1;

    public bool Alive => currentHP > 0;

    public float currentHP { get; private set; }


    public Action OnDead = null;


    private Transform _mainCamera = null;
    public Transform mainCamera 
    {
        get
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main.transform;
            return _mainCamera;
        }
    }



    private void Awake()
    {
        canvas.worldCamera = Camera.main;
    }

    public void Initialized(Dictionary<CharacterAttributes, float> attributes)
    {
        this.attributes = attributes;
        ActiveUIStats(true);
        currentHP = MaxHP;
        HPBar.value = HPBGBar.value = 1f;
        transform.localPosition = transform.localEulerAngles = Vector3.zero;
    }

    public void OnHealthChanged(float changeValue)
    {
        if (!Alive)
            return;

        currentHP = Mathf.Clamp(currentHP + changeValue, 0, MaxHP);

        hpTween.Stop();
        hpBGTween.Stop();

        hpTween = HPBar.DOValue(Mathf.Clamp(currentHP / MaxHP, 0, MaxHP), 0.3f).SetEase(Ease.OutCirc);
        hpBGTween = HPBGBar.DOValue(Mathf.Clamp(currentHP / MaxHP, 0, MaxHP), 0.5f).SetDelay(0.3f).SetEase(Ease.OutCirc);

        if (!Alive)
        {
            ActiveUIStats(false);
            OnDead?.Invoke();
        }
    }

    public void ActiveUIStats(bool value) => canvas.gameObject.SetActive(value);

    private void Update()
    {
        canvas.transform.forward = -mainCamera.forward;
    }
}
