using UnityEngine;

public class Boss2SkillVfx : EffectBase
{



    public CharacterBrain sender { get; private set; }
    public CharacterBrain player { get; private set; }



    protected override void Initialized(EffectData data)
    {
        base.Initialized(data);
        sender = data.objectData[0] as CharacterBrain;
        player = data.objectData[1] as CharacterBrain;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other != player.characterAttack.capsuleCollider)
            return;

        player.TakedDamage(data.floatData[0], sender);
        player.Stun(data.floatData[1]);
    }
}
