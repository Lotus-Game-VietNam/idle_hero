using AmazingAssets.AdvancedDissolve;
using DG.Tweening;
using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoUI
{
    [Title("Object Reference")]
    public WorldSpaceCanvas uiCanvas = null;
    public Slider HPBar = null;
    public Slider HPBGBar = null;
    public ParticleSystem healingFx = null;

    [Title("Dissolve Setting")]
    public SkinnedMeshRenderer skinnedMeshRenderer = null;
    public Material defaultMaterial = null;
    public Material dissolveMaterial = null;
    public float timeDissolve = 1f;


    private Tween hpTween = null;
    private Tween hpBGTween = null;


    public Dictionary<CharacterAttributes, float> attributes { get; private set; }

    public float MaxHP => attributes[CharacterAttributes.HP];

    public float ATK => attributes[CharacterAttributes.ATK];

    public float SPD => attributes.ContainsKey(CharacterAttributes.SPD) ? attributes[CharacterAttributes.SPD] : 1;

    public bool Alive => currentHP > 0;

    public float currentHP { get; private set; }


    public Action OnDead = null;




    public void Initialized(Dictionary<CharacterAttributes, float> attributes)
    {
        this.attributes = attributes;
        ActiveUIStats(true);

        currentHP = MaxHP;
        HPBar.value = HPBGBar.value = 1f;

        if (skinnedMeshRenderer != null)
            skinnedMeshRenderer.material = defaultMaterial;

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

    public void Dissolve(Action Callback = null)
    {
        if (skinnedMeshRenderer == null) return;

        skinnedMeshRenderer.material = dissolveMaterial;

        AdvancedDissolveKeywords.SetKeyword(dissolveMaterial, AdvancedDissolveKeywords.State.Enabled, true);
        AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(dissolveMaterial, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, 0);

        StartCoroutine(IEDissolve(Callback));
    }

    protected IEnumerator IEDissolve(Action Callback = null)
    {
        float clipValue = 0;

        while (clipValue < 1)
        {
            AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(dissolveMaterial, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, clipValue);
            clipValue += Time.deltaTime / 1.5f;
            yield return null;
        }

        Callback?.Invoke();
    }

    public void Revive()
    {
        currentHP = MaxHP;
        OnHealthChanged(MaxHP);
        ActiveUIStats(true);
        healingFx.PlayVfx();
    }

    public void ActiveUIStats(bool value) => uiCanvas.gameObject.SetActive(value);
}
