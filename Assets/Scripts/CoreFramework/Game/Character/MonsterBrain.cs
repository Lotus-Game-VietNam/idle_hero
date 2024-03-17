using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBrain : CharacterBrain
{
    public override CharacterType CharacterType => CharacterType.Monster;

    protected override string GetProjectileName(AttackType type)
    {
        return "Monster_Projectile_1_1";
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        FollowTarget();
        Shot(AttackType.NormalAttack);
    }
}
