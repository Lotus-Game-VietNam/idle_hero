using Lotus.CoreFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3ProjectileVfx : ProjectileVfx
{
    public ParticleSystem aoeFx = null;
    public List<BezierProjectile> projectiles = new List<BezierProjectile>();



    protected override void OnShow()
    {
        Firing();
    }

    protected override void Firing()
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

        StartCoroutine(IEFire());
    }

    private IEnumerator IEFire()
    {
        projectiles[0].OnArried = () => 
        {
            aoeFx.transform.position = data.target.center + (Vector3.up * 0.1f);
            aoeFx.PlayVfx();
        };

        foreach (BezierProjectile projectile in projectiles)
        {
            float distanceTotarget = Vector3.Distance(transform.position, targetAttack);
            float offset = Random.Range(0, 10) % 2 == 0 ? 1: -1;
            Vector3 p0 = transform.position;
            Vector3 p1 = Vector3.Lerp(p0, targetAttack, Random.Range(0.2f, 0.3f)) + (Vector3.up * (distanceTotarget / 2.5f)) + (transform.right * Random.Range(0, distanceTotarget / 5) * offset);
            Vector3 p2 = Vector3.Lerp(p0, targetAttack, Random.Range(0.3f, 0.4f)) + (Vector3.up * (distanceTotarget / 3.5f)) + (transform.right * Random.Range(0, distanceTotarget / 6) * offset);
            Vector3 p3 = data.target.center + (transform.right * (Random.Range(0, 7)) * offset) + (transform.forward * (Random.Range(-7, 7)));
            projectile.Initialized(p0, p1, p2, p3);
            yield return null;
        }
    }

    protected override void Explosition()
    {

    }

    protected override void OnTriggerEnter(Collider other)
    {

    }
}
