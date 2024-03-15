using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{


    public Dictionary<CharacterAttributes, float> attributes { get; private set; }

    public float HP => attributes[CharacterAttributes.HP];

    public float ATK => attributes[CharacterAttributes.ATK];

    public float SPD => attributes.ContainsKey(CharacterAttributes.SPD) ? attributes[CharacterAttributes.SPD] : 1;


    public void Initialized(Dictionary<CharacterAttributes, float> attributes)
    {
        this.attributes = attributes;
    }
}
