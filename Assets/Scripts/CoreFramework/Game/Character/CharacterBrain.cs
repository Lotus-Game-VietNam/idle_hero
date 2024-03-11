using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class CharacterBrain : IPool<CharacterConfig>
{
    [DetailedInfoBox("Deactive prefab này khi set up", "Object này đang sử dụng object pooling, khi setup xong prefab này hãy Deactive game object đi để Pooling xử lý đúng logic")]

    [Title("Component")]
    [SerializeField] protected CharacterStats characterStats = null;
    [SerializeField] protected CharacterMovement characterMovement = null;
    [SerializeField] protected AnimatorStates animatorState = null;


    public abstract CharacterType CharacterType { get; }


    protected override void Initialized(CharacterConfig data)
    {
        characterStats.Initialized(data.GetAttributes());
        characterMovement.Initialized(characterStats.SPD);
        animatorState.Initialized(AnimationStates.Idle);
    }

    protected override void OnHide()
    {
        
    }

    protected override void OnShow()
    {
        
    }
}
