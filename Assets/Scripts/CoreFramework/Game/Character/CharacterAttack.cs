using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [Title("Object Reference")]
    [SerializeField] protected Transform[] spawnProjectilePoint = null;
    [SerializeField] protected CapsuleCollider _collider = null;

    [Title("Configuration")]
    [SerializeField] protected float attackRange = 3f;


    public CapsuleCollider collider => _collider;

    public Transform body => collider.transform;

    public float height => collider.height * collider.transform.localScale.y;



    public virtual void OnShot(AttackType type, ProjectileData projectileData)
    {
        this.DequeueProjectileVfx(projectileData.projectileName).Initial(projectileData).SetPosition(GetProjectilePoint(type)).Show();

    }

    private Vector3 GetProjectilePoint(AttackType type) => spawnProjectilePoint[(int)type].position;

    public bool OnAttackRange(Vector3 target) => Vector3.Distance(body.position, target) <= attackRange; 
}
