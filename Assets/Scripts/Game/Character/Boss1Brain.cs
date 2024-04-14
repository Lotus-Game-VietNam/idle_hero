using System.Collections;
using UnityEngine;

public class Boss1Brain : MonsterBrain
{

    private readonly float timesCD = 10f;
    private readonly int totalCountProjectileOnSkill = 3;
    private readonly float skillFireRate = 0.15f;
    private readonly float normalAttackDelay = 3f;

    private float skillDamage => characterStats.ATK * 2;

    private bool onSkillCD = false;

    private float countNormalAttack = 0f;
    private float countTimeSkill = 0f;
    


    protected override void SetStarterValues()
    {
        base.SetStarterValues();
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
            StartCoroutine(IEFireSkill());
    }

    protected override void Shot(AttackType type)
    {
        if (type == AttackType.NormalAttack && !NormalAttack())
            return;

        base.Shot(type);
    }

    private IEnumerator IEFireSkill()
    {
        if (animatorState.currentState == AnimationStates.NormalAttack)
            yield break;

        onSkillCD = true;
        for (int i = 0; i < totalCountProjectileOnSkill; i++)
        {
            base.OnShot((int)AttackType.SkillOne);
            yield return new WaitForSeconds(skillFireRate);
        }
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


    protected override void OnUpdate()
    {
        base.OnUpdate();
        FollowTarget();
        Shot(!onSkillCD ? AttackType.SkillOne : AttackType.NormalAttack);
        SkillCountingDown();
    }
}
