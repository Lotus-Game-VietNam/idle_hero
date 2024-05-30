using Lotus.CoreFramework;
using System.Collections;
using UnityEngine;

public class Boss2Brain : MonsterBrain
{
    private readonly float normalAttackDelay = 4f;
    private readonly float skillStunTime = 1f;

    private float skillDamage => characterStats.ATK * 0.8f;
    
    private bool onSkillCD = false;
    private bool onSkill = false;

    private float timesCD = 10f;
    private float countNormalAttack = 0f;
    private float countTimeSkill = 0f;



    protected override void SetStarterValues()
    {
        base.SetStarterValues();
        star = 3;
        timesCD = star >= 2 ? 11 : 10;
        onSkill = false;
    }

    protected override string GetProjectileName(AttackType type)
    {
        return type == AttackType.SkillTrue ? "Boss_2_Projectile_2" : "Boss_1_Projectile_1";
    }

    protected override float GetFinalDamage(int attackType)
    {
        return attackType == 0 ? base.GetFinalDamage(attackType) : skillDamage;
    }


    protected AttackType GetAttackType()
    {
        if (onSkillCD)
            return AttackType.NormalAttack;

        if (star == 1)
            return AttackType.SkillOne;
        else if (star == 2)
            return AttackType.SkillTrue;
        else if (star == 3)
        {
            float random = Random.Range(0, 100f);
            if (random < 65)
                return AttackType.SkillTrue;
            else
                return AttackType.SkillOne;
        }

        return AttackType.NormalAttack;
    }


    protected override void OnShot(int type)
    {
        AttackType attackType = (AttackType)type;

        if (attackType == AttackType.NormalAttack)
            base.OnShot(type);
        else if (attackType == AttackType.SkillOne)
            StartCoroutine(IESkill1());
        else if (attackType == AttackType.SkillTrue)
            StartCoroutine(IESkill2());
    }

    protected override void Shot(AttackType type)
    {
        if (type == AttackType.NormalAttack && !NormalAttack())
            return;

        if (onSkill)
            return;

        base.Shot(type);
    }


    private IEnumerator IESkill2()
    {
        if (animatorState.currentState == AnimationStates.NormalAttack)
            yield break;

        onSkill = true;
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

        onSkill = false;
    }


    private IEnumerator IESkill1()
    {
        onSkillCD = true;
        onSkill = true;
        for (int i = 0;i < 3; i++)
        {
            this.DequeueEffect("Boss2_Skill", null).SetPosition(center).
            Initial(new EffectData(new float[] { skillDamage, skillStunTime }, null, new CharacterBrain[] { this, targetAttack })).Show();
            yield return new WaitForSeconds(0.5f);
        }
        onSkill = false;
    }

    public override void TakedDamage(float damage, CharacterBrain sender)
    {
        if (onSkill)
            return;
        base.TakedDamage(damage, sender);
    }

    private bool NormalAttack()
    {
        countNormalAttack += Time.deltaTime;
        if (countNormalAttack < normalAttackDelay)
            return false;
        countNormalAttack = 0;
        return true;
    }

    private void SkillCountingDown()
    {
        if (!onSkillCD)
            return;

        countTimeSkill += Time.deltaTime;
        if (countTimeSkill >= timesCD)
        {
            countTimeSkill = 0f;
            onSkillCD = false;
        }
    }

    private void SetAttackRange()
    {
        characterAttack.SetAttackRange(!onSkillCD ? 15 : -1);
    }

    protected override void OnTargetDead()
    {
        animatorState.ChangeState(AnimationStates.Idle);
    }


    protected override void OnUpdate()
    {
        base.OnUpdate();
        FollowTarget();
        Shot(GetAttackType());
        SkillCountingDown();
        SetAttackRange();
    }
}
