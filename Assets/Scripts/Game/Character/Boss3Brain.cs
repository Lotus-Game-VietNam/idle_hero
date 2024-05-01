public class Boss3Brain : Boss1Brain
{

    protected override float skillDamage => characterStats.ATK;

    protected override int totalCountProjectileOnSkill => 5;

    protected override float skillFireRate => 0.5f;



    protected override string GetProjectileName(AttackType type)
    {
        return type == AttackType.SkillOne ? "Boss_2_Projectile_2" : "Boss_2_Projectile_1";
    }

    protected override void SetAttackRange()
    {
        characterAttack.SetAttackRange(!onSkillCD ? 25 : -1);
    }
}
