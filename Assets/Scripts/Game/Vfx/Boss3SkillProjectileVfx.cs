using Lotus.CoreFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3SkillProjectileVfx : ProjectileVfx
{


    public override bool autoHide => false;


    private Vector3 targetFollow => data.target.center + (Vector3.up * (data.target.characterAttack.height / 2));

    private Coroutine fireCrt = null;

    private bool hasEplosition = false;


    protected override void Initialized(ProjectileData data)
    {
        hasEplosition = false;
        base.Initialized(data);
    }

    protected override void OnShow()
    {
        base.OnShow();
        StartCoroutine(WaitToExplosition());
    }

    private IEnumerator WaitToExplosition()
    {
        yield return new WaitForSecondsRealtime(timeToHide);
        if (!hasEplosition)
            Explosition();
        this.DelayCall(2.5f, HideAct);
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

        modelFx.PlayVfx();

        if (fireCrt != null)
            StopCoroutine(fireCrt);
        fireCrt = StartCoroutine(IEFiring());
    }

    private IEnumerator IEFiring()
    {
        while (this != null)
        {
            Vector3 direction = data.target.characterStats.Alive ? targetFollow - transform.position : transform.forward;
            Vector3 targetMove = body.position + (direction.normalized * moveSpeed * Time.fixedDeltaTime);
            transform.LookAt(targetMove);
            body.MovePosition(targetMove);
            yield return null;
        }
    }

    protected override void Explosition()
    {
        hasEplosition = true;
        StopCoroutine(fireCrt);
        body.velocity = Vector3.zero;
        modelFx.gameObject.SetActive(false);
        hitFx.PlayVfx();
    }
}
