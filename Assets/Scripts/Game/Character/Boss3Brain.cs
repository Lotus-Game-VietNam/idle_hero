using Lotus.CoreFramework;
using System.Collections;
using UnityEngine;

public class Boss3Brain : Boss1Brain
{

    protected override float skillDamage => characterStats.ATK;

    protected override int totalCountProjectileOnSkill => 5;

    protected override float skillFireRate => 0.5f;




    protected override void SetStarterValues()
    {
        base.SetStarterValues();
        star = 2;
    }


    protected override string GetProjectileName(AttackType type)
    {
        return "Boss_3_Projectile_1";
    }

    protected override void SetAttackRange()
    {
        characterAttack.SetAttackRange(!onSkillCD ? 25 : -1);
    }


    protected override IEnumerator IEFireSkill()
    {
        if (animatorState.currentState == AnimationStates.NormalAttack)
            yield break;

        onSkillCD = true;
        isOnSkill = true;

        for (int i = 0; i < totalCountProjectileOnSkill; i++)
        {
            int type = (int)AttackType.SkillOne;
            characterAttack.Shot((AttackType)type, new ProjectileData("Boss_3_Projectile_2", GetFinalDamage(type), this, targetAttack));
            yield return new WaitForSeconds(skillFireRate);
        }

        isOnSkill = false;
    }

    protected override IEnumerator IESkill2()
    {
        if (animatorState.currentState == AnimationStates.NormalAttack)
            yield break;

        isOnSkill = true;
        onSkillCD = true;
        float countTime = 0f;
        while (countTime < 2)
        {
            characterMovement.RotateToTarget(targetAttack.center);
            countTime += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < 5; i++)
        {
            ProjectileData projectileData = new ProjectileData("Boss_2_Projectile_2", characterStats.ATK * 1.5f, this, targetAttack);
            Vector3 startPos = targetAttack.center + Vector3.up * 50 - Vector3.right * 20;
            this.DequeueProjectileVfx(projectileData.projectileName).Initial(projectileData).SetPosition(startPos).Show();
            yield return new WaitForSeconds(1);
        }

        isOnSkill = false;
    }

    public override void TakedDamage(float damage, CharacterBrain sender)
    {
        if (isOnSkill)
            return;

        base.TakedDamage(damage, sender);
    }
}
