using Lotus.CoreFramework;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBrain : CharacterBrain
{
    public override CharacterType CharacterType => CharacterType.Monster;


    public int star { get; protected set; }



    protected override void Awake()
    {
        base.Awake();
        animatorState.events.OnAttackEvent = OnAttack;
    }

    protected override void Initialized(CharacterConfig data)
    {
        InitEvents();
        characterMovement.Initialized(GetComponent<NavMeshAgent>());
        characterStats.Initialized(new System.Collections.Generic.Dictionary<CharacterAttributes, float> 
        {
            { CharacterAttributes.HP, DataManager.WorldData.currentLevel * 10 },
            { CharacterAttributes.ATK, DataManager.WorldData.currentLevel * 2 },
        });
        animatorState.Initialized();
        characterAttack.Initialized();
        SetStarterValues();
    }


    protected override void SetStarterValues()
    {
        base.SetStarterValues();
        star = (DataManager.WorldData.currentLevel + 3) % 3 == 0 ? 3 : (DataManager.WorldData.currentLevel + 3) % 3;
    }


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

    protected virtual void OnAttack()
    {
        this.DequeueEffect("MonsterHitFx").SetPosition(targetAttack.center + (Vector3.up * (targetAttack.characterAttack.height / 2))).Show();
        targetAttack.TakedDamage(characterStats.ATK, this);
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

    protected override void OnTargetDead()
    {
        
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (animatorState.currentState != AnimationStates.Run)
            blendSpeed = 0f;
    }
}
