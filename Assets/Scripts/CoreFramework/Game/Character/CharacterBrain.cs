using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class CharacterBrain : IPool<CharacterConfig>
{
    [DetailedInfoBox("Deactive prefab này khi set up...", "Object này đang sử dụng object pooling, khi setup xong prefab này hãy Deactive game object đi để Pooling xử lý đúng logic")]


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
    protected override void Initialized(CharacterConfig data)
    {
        InitEvents();
        characterMovement.Initialized(body);
        characterStats.Initialized(data.GetAttributes());
        animatorState.Initialized(AnimationStates.Idle);
    }

    protected override void OnHide()
    {
        
    }

    protected override void OnShow()
    {
        
    }

    protected virtual void InitEvents()
    {
        animatorState.events.OnNormalShotEvent = OnNormalShot;
    }

    public IPool<CharacterConfig> SetTargetAttack(CharacterBrain target)
    {
        this.targetAttack = target;
        return this;
    }
    #endregion



    #region Attack
    public CharacterBrain targetAttack { get; private set; }

    public Transform body => animatorState.transform;


    public virtual void TakedDamage(float damage, CharacterBrain sender)
    {
        characterStats.OnHealthChanged(-damage);

    }

    protected virtual void OnNormalShot()
    {
        characterAttack.OnShot(AttackType.NormalAttack, new ProjectileData("Projectile_1_1", characterStats.ATK, this, targetAttack));
    }

    protected void Shot()
    {
        characterMovement.RotateToTargetCoroutine(targetAttack.body.position, 0.1f, () => 
        {
            animatorState.ChangeState(AnimationStates.NormalAttack);
        });
    }

    protected virtual void Update()
    {

    }
    #endregion
}
