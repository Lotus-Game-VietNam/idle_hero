using DG.Tweening;
using Lotus.CoreFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBrain : CharacterBrain
{
    public override CharacterType CharacterType => CharacterType.Hero;


    private HeroCostumes _heroCostumes = null;
    public HeroCostumes heroCostumes => this.TryGetComponentInChildren(ref _heroCostumes);



    protected override void Initialized(CharacterConfig data)
    {
        base.Initialized(data);
        heroCostumes.Initialized();
    }

    protected override void InitEvents()
    {
        base.InitEvents();
        this.AddListener<ItemType, int>(EventName.ChangeCostume, ChangeCostume);
    }


    protected override string GetProjectileName(AttackType type)
    {
        return "Hero_Projectile_1_1";
    }

    private void ChangeCostume(ItemType itemType, int itemLevel)
    {
        transform.DOPunchScale(Vector3.one * 0.1f, 0.5f).SetEase(Ease.InOutElastic);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        Shot(AttackType.NormalAttack);
    }
}
