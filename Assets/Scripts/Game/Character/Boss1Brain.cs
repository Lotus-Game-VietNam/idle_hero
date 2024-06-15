using Lotus.CoreFramework;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss1Brain : MonsterBrain
{
    protected readonly float normalAttackDelay = 4f;

    protected virtual float skillFireRate => 0.15f;

    protected virtual int totalCountProjectileOnSkill => 3;

    protected virtual float skillDamage => characterStats.ATK * 2;

    protected bool onSkillCD_1 = false;
    protected bool onSkillCD_2 = false;

    protected bool isOnSkill = false;

    protected readonly float timesCD_1 = 10f;
    protected readonly float timesCD_2 = 13f;

    protected float countNormalAttack = 0f;
    protected float countTimeSkill_1 = 0f;
    protected float countTimeSkill_2 = 0f;



    protected override void Awake()
    {
        base.Awake();
        animatorState.events.Event1 = OnStartSkill2;
        animatorState.events.Event2 = OnSkill2;
    }

    protected override void Initialized(CharacterConfig data)
    {
        InitEvents();
        characterMovement.Initialized(GetComponent<NavMeshAgent>());
        characterStats.Initialized(new System.Collections.Generic.Dictionary<CharacterAttributes, float>
        {
            { CharacterAttributes.HP, DataManager.WorldData.currentLevel * 200 },
            { CharacterAttributes.ATK, DataManager.WorldData.currentLevel * 5 },
        });
        animatorState.Initialized();
        characterAttack.Initialized();
        SetStarterValues();
    }

    protected override void SetStarterValues()
    {
        base.SetStarterValues();
        //star = 3;
        isOnSkill = false;
        onSkillCD_1 = onSkillCD_2 = false;
        countTimeSkill_1 = countTimeSkill_2 = countNormalAttack = 0;
    }


    protected override string GetProjectileName(AttackType type)
    {
        return "Boss_1_Projectile_1";
    }

    protected override float GetFinalDamage(int attackType)
    {
        return attackType == 0 ? base.GetFinalDamage(attackType) : skillDamage;
    }

    protected AttackType GetAttackType()
    {
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
            StartCoroutine(IEFireSkill());
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
        onSkillCD_1 = true;
        for (int i = 0; i < totalCountProjectileOnSkill; i++)
        {
            base.OnShot((int)AttackType.SkillOne);
            yield return new WaitForSeconds(skillFireRate);
        }
        isOnSkill = false;
    }

    private void OnStartSkill2()
    {
        if (animatorState.currentState == AnimationStates.NormalAttack)
            return;

        isOnSkill = true;
        onSkillCD_2 = true;
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

    protected virtual IEnumerator IESkill2()
    {
        float countTime = 0;
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


    protected override void OnTargetDead()
    {
        animatorState.ChangeState(AnimationStates.Idle);
    }

    protected virtual void SetAttackRange()
    {
        characterAttack.SetAttackRange(!onSkillCD_1 && !onSkillCD_2 ? 20 : -1);
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

        if (!isOnSkill)
            Shot(GetAttackType());

        Skill1CountingDown();
        Skill2CountingDown();

        SetAttackRange();
    }
}
