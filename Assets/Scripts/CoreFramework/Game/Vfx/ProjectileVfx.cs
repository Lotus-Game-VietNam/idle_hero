using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class ProjectileVfx : IPool<ProjectileData>
{
    [DetailedInfoBox("Deactive prefab này khi set up...", "Object này đang sử dụng object pooling, khi setup xong prefab này hãy Deactive game object đi để Pooling xử lý đúng logic")]

    [Title("Object Reference")]
    public ParticleSystem modelFx = null;
    public ParticleSystem muzzleFx = null;
    public ParticleSystem hitFx = null;

    [Title("Projectile Setting")]
    public float hideAffterTime = 4f;
    public float moveSpeed = 1000f;


    private Rigidbody _body = null;
    public Rigidbody body => this.TryGetComponent(ref _body);

    private SphereCollider _sphereCollider = null;
    public SphereCollider sphereCollider => this.TryGetComponent(ref _sphereCollider);

    public ProjectileData data { get; private set; }

    private Coroutine hideCrt = null;


    protected override void Initialized(ProjectileData data)
    {
        this.data = data;
    }

    protected override void OnHide()
    {
        
    }

    protected override void OnShow()
    {
        body.velocity = Vector3.zero;
        modelFx.StopVfx();
        muzzleFx.StopVfx();
        hitFx.StopVfx();
        modelFx.gameObject.SetActive(true);
        muzzleFx.transform.localPosition = Vector3.zero;
        Firing();
        WaitToHide();
    }

    protected virtual void Firing()
    {
        if (data == null)
        {
            LogTool.LogErrorEditorOnly("Chưa truyền project data!");
            return;
        }

        transform.LookAt(data.target.body.position + (Vector3.up * (data.target.characterAttack.height / 2)));
        muzzleFx.transform.SetParent(null);
        muzzleFx.PlayVfx();
        modelFx.PlayVfx();
        body.AddForce(transform.forward * moveSpeed);
    }

    private void WaitToHide()
    {
        if (hideCrt != null)
            StopCoroutine(hideCrt);
        hideCrt = this.DelayCall(hideAffterTime, () =>
        {
            muzzleFx.transform.SetParent(transform);
            this.PushProjectileVfx(this);
        });
    }

    private void Explosition()
    {
        body.velocity = Vector3.zero;
        modelFx.gameObject.SetActive(false);
        hitFx.PlayVfx();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Explosition();

        if (data == null)
            LogTool.LogErrorEditorOnly("Chưa truyền project data!");
        else if (data != null && other == data.target.characterAttack.collider)
            data.target.TakedDamage(data.damage, data.sender);
    }
}
