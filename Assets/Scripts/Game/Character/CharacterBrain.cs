using Lotus.CoreFramework;
using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class CharacterBrain : IPool<CharacterConfig>
{
    #region Component

    private CharacterStats _characterStats = null;
    public CharacterStats characterStats => this.TryGetComponentInChildren(ref _characterStats);

    private CharacterMovement _characterMovement = null;
    public CharacterMovement characterMovement => this.TryGetComponentInChildren(ref _characterMovement);

    private AnimatorStates _animatorState = null;
    public AnimatorStates animatorState => this.TryGetComponentInChildren(ref _animatorState);

    private CharacterAttack _characterAttack = null;
    public CharacterAttack characterAttack => this.TryGetComponentInChildren(ref _characterAttack);


    #endregion



    #region Data

    public abstract CharacterType CharacterType { get; }

    #endregion



    #region Constructor
    private bool hasInitialized = false;

    public override bool autoHide => false;

    public override Action HideAct => this.PushCharacter;


    protected virtual void Awake()
    {
        animatorState.events.OnShotFinishEvent = OnShotFinish;
        animatorState.events.OnShotEvent = OnShot;
        characterStats.OnDead = Dead;
    }


    protected override void Initialized(CharacterConfig data)
    {
        InitEvents();
        characterMovement.Initialized(GetComponent<NavMeshAgent>());
        characterStats.Initialized(data.GetAttributes());
        animatorState.Initialized();
        characterAttack.Initialized();
        hasInitialized = true;
    }

    protected override void OnHide()
    {
        hasInitialized = false;
    }

    protected override void OnShow()
    {

    }

    protected virtual void InitEvents()
    {
        
    }

    public IPool<CharacterConfig> SetTargetAttack(CharacterBrain target)
    {
        this.targetAttack = target;
        return this;
    }
    #endregion



    #region Attack
    public Vector3 center => transform.position;

    public bool onFollowTarget { get; private set; }

    public virtual CharacterBrain targetAttack { get; private set; }


    public virtual void TakedDamage(float damage, CharacterBrain sender)
    {
        characterStats.OnHealthChanged(-damage);

    }

    protected virtual void OnShot(int type)
    {
        characterAttack.Shot((AttackType)type, new ProjectileData(GetProjectileName((AttackType)type), characterStats.ATK, this, targetAttack));
    }

    protected virtual void OnShotFinish()
    {
        
    }

    protected virtual void Shot(AttackType type)
    {
        if (TargetIsNull()) return;

         if (!targetAttack.characterStats.Alive || !characterStats.Alive) return;

        if (onFollowTarget || animatorState.currentState == type.Convert() || characterMovement.crtRotating) return;

        characterMovement.RotateToCrt(targetAttack.center, 0.2f, null, () => 
        {
            animatorState.ChangeState(type.Convert());
        });
    }

    protected virtual void FollowTarget()
    {
        if (TargetIsNull()) return;

        if (!targetAttack.characterStats.Alive || !characterStats.Alive) return;

        onFollowTarget = !characterAttack.OnAttackRange(targetAttack.center);

        if (!onFollowTarget) return;

        animatorState.ChangeState(AnimationStates.Run);
        characterMovement.MoveTo(targetAttack.center, characterAttack.attackRange);
    }

    private void Dead()
    {
        characterAttack.ActiveCollider(false);
        characterMovement.ActiveAgent(false);

        animatorState.ChangeState(AnimationStates.Die);

        this.SendMessage(EventName.OnCharacterDead, this);

        OnDead();
    }

    protected virtual void OnDead()
    {

    }


    protected virtual void OnUpdate()
    {

    }

    protected void Update()
    {
        if (!hasInitialized || !characterStats.Alive)
        {
            animatorState.ChangeState(AnimationStates.Idle);
            return;
        } 

        OnUpdate();
    }
    #endregion


    #region Support

    protected bool TargetIsNull()
    {
        if (targetAttack == null)
        {
            LogTool.LogErrorEditorOnly("TargetAttack is Null");
            return true;
        }
        return false;
    }

    protected abstract string GetProjectileName(AttackType type);
    #endregion
}
