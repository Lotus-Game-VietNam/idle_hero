using System.Collections;
using UnityEngine;

public class Boss1Brain : MonsterBrain
{
    private readonly float normalAttackDelay = 4f;

    protected virtual float skillFireRate => 0.15f;

    protected virtual int totalCountProjectileOnSkill => 3;

    protected virtual float skillDamage => characterStats.ATK * 2;

    protected bool onSkillCD = false;
    protected bool isOnSkill = false;

    private float timesCD = 10f;
    protected float countNormalAttack = 0f;
    protected float countTimeSkill = 0f;
    


    protected override void SetStarterValues()
    {
        base.SetStarterValues();
        //star = 3;
        timesCD = star >= 2 ? 13 : 10;
    }


    protected override string GetProjectileName(AttackType type)
    {
        return "Boss_1_Projectile_1";
    }

    protected override float GetFinalDamage(int attackType)
    {
        return attackType == 0 ? base.GetFinalDamage(attackType) : skillDamage;
    }

    private AttackType GetAttackType()
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
            StartCoroutine(IEFireSkill());
        else if (attackType == AttackType.SkillTrue)
            StartCoroutine(IESkill2());
    }

    protected override void Shot(AttackType type)
    {
        if (type == AttackType.NormalAttack && !NormalAttack())
            return;

        if (isOnSkill)
            return;

        base.Shot(type);
    }

    protected virtual IEnumerator IEFireSkill()
    {
        if (animatorState.currentState == AnimationStates.NormalAttack)
            yield break;

        isOnSkill = true;
        onSkillCD = true;
        for (int i = 0; i < totalCountProjectileOnSkill; i++)
        {
            base.OnShot((int)AttackType.SkillOne);
            yield return new WaitForSeconds(skillFireRate);
        }
        isOnSkill = false;
    }

    private IEnumerator IESkill2()
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

        countTime = 0;
        bool isTrigger = false;
        while (countTime < 0.5f)
        {
            characterMovement.agent.Move(transform.forward * 60f * Time.deltaTime);
            countTime += Time.deltaTime;
            if (!isTrigger && characterMovement.DistanceToTarget(targetAttack.center) <= characterMovement.agent.radius * 3f)
            {
                isTrigger = true;
                targetAttack.animatorState.ChangeState(AnimationStates.TakeDamage);
                targetAttack.TakedDamage(characterStats.ATK * 2.5f, this);
            }
            yield return null;
        }

        isOnSkill = false;
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

    protected override void OnTargetDead()
    {
        animatorState.ChangeState(AnimationStates.Idle);
    }

    protected virtual void SetAttackRange()
    {
        characterAttack.SetAttackRange(!onSkillCD ? 20 : -1);
    }

    protected override void FollowTarget()
    {
        if (isOnSkill)
            return;

        base.FollowTarget();
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
