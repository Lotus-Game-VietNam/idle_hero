using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class CharacterBrain : IPool<CharacterConfig>
{
    [DetailedInfoBox("Deactive prefab này khi set up", "Object này đang sử dụng object pooling, khi setup xong prefab này hãy Deactive game object đi để Pooling xử lý đúng logic")]

    public CharacterBrain target = null;


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
        characterStats.Initialized(data.GetAttributes());
        characterMovement.Initialized(characterStats.SPD);
        animatorState.Initialized(AnimationStates.Idle);
        InitEvents();
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
    #endregion



    #region Attack

    public virtual void TakedDamage(float damage, CharacterBrain sender)
    {
        characterStats.OnHealthChanged(-damage);

    }

    protected virtual void OnNormalShot()
    {
        characterAttack.OnShot(AttackType.NormalAttack, characterStats.ATK, this, target);
    }

    [Button("Normal Shot")]
    public void Shot()
    {
        animatorState.ChangeState(AnimationStates.NormalAttack);
    }
    #endregion
}
