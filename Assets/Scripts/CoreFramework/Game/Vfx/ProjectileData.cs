using UnityEngine;


public class ProjectileData
{
    public float damage;
    public Vector3 startPoint;
    public CharacterBrain sender;
    public CharacterBrain target;


    public ProjectileData()
    {
        
    }

    public ProjectileData(float damage, Vector3 startPoint, CharacterBrain sender, CharacterBrain target)
    {
        this.startPoint = startPoint;
        this.damage = damage;
        this.sender = sender;
        this.target = target;
    }
}
