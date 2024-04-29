using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody))]
public class ProjectileVfx : IPool<ProjectileData>
{
    [Title("Object Reference")]
    public ParticleSystem modelFx = null;
    public ParticleSystem muzzleFx = null;
    public ParticleSystem hitFx = null;

    [Title("Projectile Setting")]
    public float hideAffterTime = 4f;
    public float moveSpeed = 300f;



    public override bool autoHide => true;
    public override float timeToHide => hideAffterTime;
    public override Action HideAct => this.PushProjectileVfx;


    private Rigidbody _body = null;
    public Rigidbody body => this.TryGetComponent(ref _body);

    private SphereCollider _sphereCollider = null;
    public SphereCollider sphereCollider => this.TryGetComponent(ref _sphereCollider);


    public Vector3 targetAttack { get; private set; }



    protected virtual void Awake()
    {
        body.useGravity = false;
        moveSpeed *= ((SceneName)SceneManager.GetActiveScene().buildIndex).IsOnFarm() ? 1 : 2;
    }

    protected override void Initialized(ProjectileData data)
    {
        targetAttack = data.target.center + (Vector3.up * (data.target.characterAttack.height / 2));
    }

    public IPool<ProjectileData> SetTargetAttackPosition(Vector3 target)
    {
        targetAttack = target;
        return this;
    }

    protected override void OnHide()
    {
        if (muzzleFx != null)
            muzzleFx.transform.SetParent(transform);
    }

    protected override void OnShow()
    {
        body.velocity = Vector3.zero;

        modelFx.StopVfx();
        muzzleFx.StopVfx();
        hitFx.StopVfx();

        modelFx.gameObject.SetActive(true);

        if (muzzleFx != null)
            muzzleFx.transform.localPosition = Vector3.zero;

        Firing();
    }

    protected virtual void Firing()
    {
        if (data == null)
        {
            LogTool.LogErrorEditorOnly("Chưa truyền project data!");
            return;
        }

        transform.LookAt(targetAttack);

        if (muzzleFx != null)
        {
            muzzleFx.transform.SetParent(null);
            muzzleFx.PlayVfx();
        }
        
        modelFx.PlayVfx();
        body.AddForce(transform.forward * moveSpeed);
    }


    protected virtual void Explosition()
    {
        body.velocity = Vector3.zero;
        modelFx.gameObject.SetActive(false);
        hitFx.PlayVfx();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other == data.sender.characterAttack.capsuleCollider || other.CompareTag("Projectile"))
            return;

        Explosition();

        if (data == null)
            LogTool.LogErrorEditorOnly("Chưa truyền project data!");
        else if (data != null && other == data.target.characterAttack.capsuleCollider)
            data.target.TakedDamage(data.damage, data.sender);
    }
}
