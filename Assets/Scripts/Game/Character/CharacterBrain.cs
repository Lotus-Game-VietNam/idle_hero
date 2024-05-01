using Lotus.CoreFramework;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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

    public SceneName currentScene { get; private set; }

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

        currentScene = (SceneName)SceneManager.GetActiveScene().buildIndex;
    }


    protected override void Initialized(CharacterConfig data)
    {
        InitEvents();
        characterMovement.Initialized(GetComponent<NavMeshAgent>());
        characterStats.Initialized(data.GetAttributes());
        animatorState.Initialized();
        characterAttack.Initialized();
        SetStarterValues();
        
    }

    protected virtual void SetStarterValues()
    {
        blendSpeed = 0f;
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

    public virtual void SetJoystick(Joystick joystick) { }

    public virtual void SetSkillButtons(UI_SkillButton[] skillButtons) { }
    #endregion



    #region Attack
    public Vector3 center => transform.position;

    public bool onFollowTarget { get; protected set; }

    public virtual CharacterBrain targetAttack { get; private set; }

    protected float blendSpeed = 0f;

    private Coroutine stunCrt = null;

    public bool onStun { get; protected set; }



    protected virtual float GetFinalDamage(int attackType) => characterStats.ATK;


    public virtual void TakedDamage(float damage, CharacterBrain sender)
    {
        LogTool.LogEditorOnly($"Taked {damage} damage");
        characterStats.OnHealthChanged(-damage);

    }

    protected virtual void OnShot(int type)
    {
        characterAttack.Shot((AttackType)type, new ProjectileData(GetProjectileName((AttackType)type), GetFinalDamage(type), this, targetAttack));
    }

    protected virtual void OnShotFinish()
    {
        animatorState.ChangeState(AnimationStates.Idle);
    }

    protected virtual void Shot(AttackType type)
    {
        if (TargetIsNull()) return;

        if (!characterStats.Alive) return;

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

        if (!onFollowTarget || animatorState.currentState.IsAttack()) return;

        animatorState.ChangeState(AnimationStates.Run);
        characterMovement.MoveToTarget(targetAttack.center, characterAttack.attackRange);
    }

    public virtual void Stun(float time)
    {
        if (stunCrt != null)
            StopCoroutine(stunCrt);
        onStun = true;
        animatorState.ChangeState(AnimationStates.Stun);
        stunCrt = this.DelayCall(time, () => 
        { 
            onStun = false;
            animatorState.Ator.Play("Movement");
            animatorState.ChangeState(AnimationStates.Idle);
        });
        LogTool.LogEditorOnly($"Stun: {time}s");
    }

    private void Dead()
    {
        characterAttack.ActiveCollider(false);
        characterMovement.ActiveAgent(false);

        animatorState.ChangeState(AnimationStates.Die);

        this.SendMessage(EventName.OnCharacterDead, this);

        OnDead();
    }

    protected abstract void OnFarm();

    private void Farm()
    {
        if (!currentScene.IsOnFarm())
            return;

        OnFarm();
    }

    protected abstract void OnDead();

    protected abstract void OnTargetDead();

    protected virtual void OnUpdate()
    {
        Farm();
    }

    protected void Update()
    {
        if (!hasInitialized)
        {
            animatorState.ChangeState(AnimationStates.Idle);
            return;
        }

        if (!characterStats.Alive)
            return;

        if (animatorState.currentState == AnimationStates.TakeDamage)
            return;

        if (targetAttack != null && !targetAttack.characterStats.Alive)
        {
            OnTargetDead();
            return;
        }

        if (onStun)
            return;

        if (animatorState.currentState.IsAttack() && !characterMovement.crtRotating)
            characterMovement.RotateToTarget(targetAttack.center);

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

    protected virtual void SetBlendSpeed(float speed)
    {
        blendSpeed = Mathf.Lerp(blendSpeed, speed, 5f * Time.deltaTime);
        animatorState.Ator.SetFloat("Speed", blendSpeed);
        characterMovement.SetMoveSpeed(blendSpeed);
    }

    protected abstract string GetProjectileName(AttackType type);
    #endregion
}
