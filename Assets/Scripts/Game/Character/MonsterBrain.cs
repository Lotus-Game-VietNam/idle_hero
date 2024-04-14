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

    protected override void Shot(AttackType type)
    {
        if (targetAttack != null && !targetAttack.characterStats.Alive)
        {
            animatorState.ChangeState(AnimationStates.Idle);
            return;
        }

        base.Shot(type);
    }

    protected override void FollowTarget()
    {
        if (TargetIsNull()) return;

        if (!targetAttack.characterStats.Alive || !characterStats.Alive) return;

        onFollowTarget = !characterAttack.OnAttackRange(targetAttack.center);

        if (!onFollowTarget || animatorState.currentState.IsAttack())
        {
            SetBlendSpeed(0);
            return;
        } 

        //animatorState.ChangeState(AnimationStates.Run);
        characterMovement.MoveToTarget(targetAttack.center, characterAttack.attackRange);

        SetBlendSpeed(1);
    }

    protected override void OnFarm()
    {
        FollowTarget();
        Shot(AttackType.NormalAttack);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (animatorState.currentState != AnimationStates.Run)
            blendSpeed = 0f;
    }
}
