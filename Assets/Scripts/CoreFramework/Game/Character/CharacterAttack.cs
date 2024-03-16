using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [Title("Object Reference")]
    [SerializeField] protected Transform[] spawnProjectilePoint = null;
    [SerializeField] protected Collider _collider = null;


    public Transform body => Collider.transform;

    public Collider Collider => _collider;




    public virtual void OnShot(AttackType type, ProjectileData projectileData)
    {
        this.DequeueProjectileVfx(projectileData.projectileName).Initial(projectileData).SetPosition(GetProjectilePoint(type)).Show();

    }

    private Vector3 GetProjectilePoint(AttackType type) => spawnProjectilePoint[(int)type].position;
}
