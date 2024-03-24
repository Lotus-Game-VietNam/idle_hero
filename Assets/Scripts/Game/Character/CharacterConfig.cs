using System.Collections.Generic;

public class CharacterConfig
{
    public Dictionary<CharacterAttributes, float> attributes = null;


    public CharacterConfig()
    {
        
    }

    public virtual Dictionary<CharacterAttributes, float> GetAttributes() => attributes;
}
