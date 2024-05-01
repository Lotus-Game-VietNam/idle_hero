using Lotus.CoreFramework;
using UnityEngine;

public class Boss2Brain : MonsterBrain
{
    private readonly float timesCD = 10f;
    private readonly float normalAttackDelay = 4f;
    private readonly float skillStunTime = 1f;

    private float skillDamage => characterStats.ATK * 0.8f;

    private bool onSkillCD = false;
    private bool onSkill = false;

    private float countNormalAttack = 0f;
    private float countTimeSkill = 0f;



    protected override void SetStarterValues()
    {
        base.SetStarterValues();
        onSkill = false;
    }


    protected override string GetProjectileName(AttackType type)
    {
        return "Boss_1_Projectile_1";
    }

    protected override float GetFinalDamage(int attackType)
    {
        return attackType == 0 ? base.GetFinalDamage(attackType) : skillDamage;
    }

    protected override void OnShot(int type)
    {
        AttackType attackType = (AttackType)type;

        if (attackType == AttackType.NormalAttack)
            base.OnShot(type);
        else if (attackType == AttackType.SkillOne)
            OnSkill();
    }

    protected override void Shot(AttackType type)
    {
        if (type == AttackType.NormalAttack && !NormalAttack())
            return;

        base.Shot(type);
    }

    protected override void OnShotFinish()
    {
        base.OnShotFinish();
        onSkill = false;
    }

    private void OnSkill()
    {
        onSkillCD = true;
        onSkill = true;
        this.DequeueEffect("Boss2_Skill", null).SetPosition(center).
            Initial(new EffectData(new float[] { skillDamage, skillStunTime }, null, new CharacterBrain[] { this, targetAttack })).Show();
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
        Shot(!onSkillCD ? AttackType.SkillOne : AttackType.NormalAttack);
        SkillCountingDown();
        SetAttackRange();
    }
}
