

public enum SceneName
{
    Begin,
    Loading,
    Farm_1,
    Boss_1
}


public enum CharacterType
{
    Hero,
    Monster,
    Bot
}



public enum ItemType
{
    Cloth,
    Bow,
    Wing
}


public enum CharacterAttributes
{
    HP,
    ATK,
    SPD
}



public enum ResourceType
{
    Gem
}


public enum AttackType
{
    NormalAttack,
    SkillOne,
    SkillTrue,
    SkillThird,
}


public enum AnimationStates
{
    None,
    Idle,
    Run,
    NormalAttack,
    SkillOne,
    SkillTrue,
    SkillThird,
    TakeDamage,
    Die,
    Cheer
}


public enum EventName
{
    OnCharacterDead,
    BuyItem,
    X2Income,
    AutoTap,
    ShowShellValue,
    ChangeCostume,
    RefreshMonsterTarget,
    OnTriggerSkill,
    OnWin
}