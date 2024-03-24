using UnityEngine;


public class ProjectileData
{
    public string projectileName;
    public float damage;
    public CharacterBrain sender;
    public CharacterBrain target;


    public ProjectileData()
    {
        
    }

    public ProjectileData(string projectileName, float damage, CharacterBrain sender, CharacterBrain target)
    {
        this.projectileName = projectileName;
        this.damage = damage;
        this.sender = sender;
        this.target = target;
    }
}
