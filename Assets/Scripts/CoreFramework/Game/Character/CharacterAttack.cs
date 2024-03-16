using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAttack : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        public AttackType attackType = AttackType.NormalAttack;
        public string vfxName;
        public Transform spawnVfxPoint;
    }


    [Title("Object Reference")]
    [SerializeField] protected Collider collider = null;

    [Title("Configuration")]
    [SerializeField] protected Data[] _attackData = null;


    public Transform body => Collider.transform;

    public Collider Collider => collider;

    public Dictionary<AttackType, Data> attackData { get; private set; } = new Dictionary<AttackType, Data>();


    private void Awake()
    {
        foreach (var data in _attackData)
            attackData.Add(data.attackType, data);
    }

    public virtual void OnShot(AttackType type, float damage, CharacterBrain sender, CharacterBrain target)
    {
        this.DequeueProjectileVfx(attackData[type].vfxName).Initial(new ProjectileData(damage, attackData[type].spawnVfxPoint.position, sender, target)).Show();

    }
}
