using System.Collections;
using UnityEngine;

public class Boss3Brain : Boss1Brain
{

    protected override float skillDamage => characterStats.ATK;

    protected override int totalCountProjectileOnSkill => 5;

    protected override float skillFireRate => 0.5f;



    private bool onSkill = false;



    protected override void SetStarterValues()
    {
        base.SetStarterValues();
        onSkill = false;
    }


    protected override string GetProjectileName(AttackType type)
    {
        return type == AttackType.SkillOne ? "Boss_3_Projectile_2" : "Boss_3_Projectile_1";
    }

    protected override void SetAttackRange()
    {
        characterAttack.SetAttackRange(!onSkillCD ? 25 : -1);
    }

    protected override void OnShotFinish()
    {
        base.OnShotFinish();
        onSkill = false;
    }

    protected override IEnumerator IEFireSkill()
    {
        if (animatorState.currentState == AnimationStates.NormalAttack)
            yield break;

        onSkillCD = true;
        onSkill = true;

        for (int i = 0; i < totalCountProjectileOnSkill; i++)
        {
            int type = (int)AttackType.SkillOne;
            characterAttack.Shot((AttackType)type, new ProjectileData(GetProjectileName((AttackType)type), GetFinalDamage(type), this, targetAttack));
            yield return new WaitForSeconds(skillFireRate);
        }
    }

    public override void TakedDamage(float damage, CharacterBrain sender)
    {
        if (onSkill)
            return;

        base.TakedDamage(damage, sender);
    }
}
