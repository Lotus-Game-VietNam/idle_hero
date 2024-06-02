using Lotus.CoreFramework;
using System.Collections;
using UnityEngine;

public class Boss2Brain : MonsterBrain
{
    private readonly float normalAttackDelay = 3f;
    private readonly float skillStunTime = 1f;

    private float skillDamage => characterStats.ATK * 0.8f;
    
    private bool onSkillCD_1 = false;
    private bool onSkillCD_2 = false;

    private bool onSkill = false;

    private readonly float timesCD_1 = 10f;
    private readonly float timesCD_2 = 15f;

    private float countNormalAttack = 0f;
    private float countTimeSkill_1 = 0f;
    private float countTimeSkill_2 = 0f;



    protected override void Awake()
    {
        base.Awake();
        animatorState.events.Event1 = OnStartSkill2;
        animatorState.events.Event2 = OnSkill2;
    }

    protected override void SetStarterValues()
    {
        base.SetStarterValues();
        //star = 3;
        onSkill = false;
        onSkillCD_1 = onSkillCD_2 = false;
        countTimeSkill_1 = countTimeSkill_2 = countNormalAttack = 0;
    }

    protected override string GetProjectileName(AttackType type)
    {
        return type == AttackType.SkillTrue ? "Boss_2_Projectile_2" : "Boss_2_Projectile_1";
    }

    protected override float GetFinalDamage(int attackType)
    {
        return attackType == 0 ? base.GetFinalDamage(attackType) : skillDamage;
    }


    protected AttackType GetAttackType()
    {
        if (animatorState.currentState.IsAttack())
            return animatorState.currentState.Convert();

        if (onSkillCD_1 && onSkillCD_2)
            return AttackType.NormalAttack;

        if (star == 1 && !onSkillCD_1)
            return AttackType.SkillOne;
        else if (star == 2 && !onSkillCD_2)
            return AttackType.SkillTrue;
        else if (star == 3)
        {
            if (!onSkillCD_1 && !onSkillCD_2)
            {
                float random = Random.Range(0, 100f);
                if (random < 65)
                    return AttackType.SkillTrue;
                else
                    return AttackType.SkillOne;
            }
            else
            {
                if (!onSkillCD_1)
                    return AttackType.SkillOne;
                else
                    return AttackType.SkillTrue;
            }
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
    }

    protected override void Shot(AttackType type)
    {
        if (type == AttackType.NormalAttack && !NormalAttack())
            return;

        if (onSkill)
            return;

        base.Shot(type);
    }

    private void OnStartSkill2()
    {
        if (animatorState.currentState == AnimationStates.NormalAttack)
            return;

        onSkill = true;
        onSkillCD_2 = true;
        this.DequeueEffect("Boss_2_Aura").SetPosition(transform.position).Show();
        StartCoroutine(IEStartSkill2());
    }

    private IEnumerator IEStartSkill2()
    {
        float countTime = 0f;
        while (countTime < 2)
        {
            characterMovement.RotateToTarget(targetAttack.center);
            countTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnSkill2()
    {
        StartCoroutine(IESkill2());
    }

    private IEnumerator IESkill2()
    {
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
        onSkillCD_1 = true;
        onSkill = true;
        this.DequeueEffect("Boss2_Skill", null).SetPosition(center).
            Initial(new EffectData(new float[] { skillDamage, skillStunTime }, null, new CharacterBrain[] { this, targetAttack })).Show();
        //for (int i = 0;i < 3; i++)
        //{
        //    this.DequeueEffect("Boss2_Skill", null).SetPosition(center).
        //    Initial(new EffectData(new float[] { skillDamage, skillStunTime }, null, new CharacterBrain[] { this, targetAttack })).Show();
        //    yield return new WaitForSeconds(0.5f);
        //}
        yield return null;
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

    private void Skill1CountingDown()
    {
        if (!onSkillCD_1)
            return;

        countTimeSkill_1 += Time.deltaTime;
        if (countTimeSkill_1 >= timesCD_1)
        {
            countTimeSkill_1 = 0f;
            onSkillCD_1 = false;
        }
    }

    private void Skill2CountingDown()
    {
        if (!onSkillCD_2)
            return;

        countTimeSkill_2 += Time.deltaTime;
        if (countTimeSkill_2 >= timesCD_2)
        {
            countTimeSkill_2 = 0f;
            onSkillCD_2 = false;
        }
    }

    private void SetAttackRange()
    {
        characterAttack.SetAttackRange(!onSkillCD_1 ? 15 : -1);
    }

    protected override void OnTargetDead()
    {
        animatorState.ChangeState(AnimationStates.Idle);
    }


    protected override void OnUpdate()
    {
        base.OnUpdate();
        FollowTarget();

        if (!onSkill)
            Shot(GetAttackType());

        Skill1CountingDown();
        Skill2CountingDown();
        SetAttackRange();
    }
}
