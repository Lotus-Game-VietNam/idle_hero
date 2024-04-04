using Lotus.CoreFramework;
using UnityEngine;

public class MonsterBrain : CharacterBrain
{
    public override CharacterType CharacterType => CharacterType.Monster;

    protected override string GetProjectileName(AttackType type)
    {
        return "Monster_Projectile_1_1";
    }

    protected override void OnDead()
    {
        this.DelayCall(1f, () =>
        {
            characterStats.Dissolve(() => { HideAct?.Invoke(); });
        });

        //this.DequeueEffect("MonsterDie").SetPosition(center + (Vector3.up * (characterAttack.height / 2))).Show();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        FollowTarget();
        Shot(AttackType.NormalAttack);
    }
}
