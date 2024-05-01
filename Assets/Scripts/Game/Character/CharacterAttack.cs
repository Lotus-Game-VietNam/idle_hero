using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [Title("Object Reference")]
    [SerializeField] protected Transform[] spawnProjectilePoint = null;
    [SerializeField] protected CapsuleCollider _collider = null;

    [Title("Configuration")]
    [SerializeField] protected float _attackRange = 3f;


    public float attackRange => _attackRange;

    public CapsuleCollider capsuleCollider => _collider;

    public float height => capsuleCollider.height * capsuleCollider.transform.localScale.y * transform.parent.localScale.y;


    public void Initialized()
    {
        ActiveCollider(true);
    }


    public virtual void Shot(AttackType type, ProjectileData projectileData)
    {
        this.DequeueProjectileVfx(projectileData.projectileName).Initial(projectileData).SetPosition(GetProjectilePoint(type)).Show();
    }

    private Vector3 GetProjectilePoint(AttackType type) => spawnProjectilePoint[(int)type].position;

    public bool OnAttackRange(Vector3 target) => Vector3.Distance(transform.position, target) <= attackRange; 

    public void SetAttackRange(float range) => _attackRange = range;

    public void ActiveCollider(bool value)
    {
        if (value)
            capsuleCollider.enabled = true;
        else
            capsuleCollider.enabled = false;
    }
}
