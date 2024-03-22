using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBrain : CharacterBrain
{
    public override CharacterType CharacterType => CharacterType.Hero;

    protected override string GetProjectileName(AttackType type)
    {
        return "Hero_Projectile_1_1";
    }

    

    protected override void OnUpdate()
    {
        base.OnUpdate();
        Shot(AttackType.NormalAttack);
    }
}
