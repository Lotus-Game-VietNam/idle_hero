using Lotus.CoreFramework;
using System;
using UnityEngine;

public class BezierProjectile : MonoBehaviour
{
    public ParticleSystem modelFx;
    public ParticleSystem hitFx;
    public float duration = 1f;


    public Action OnArried = null;

    private float time = 0f;


    private Vector3 p0, p1, p2, p3;



    public void Initialized(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        this.p0 = p0;
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;

        time = 0;
        transform.localPosition = transform.localEulerAngles = Vector3.zero;
        modelFx.PlayVfx();
    }


    private void Update()
    {
        if (time >= 1)
            return;

        time += Time.deltaTime / duration;
        Vector3 direction = Extensions.GetQuadBezierPoint(p0, p1, p2, p3, time);
        transform.position = direction;
        transform.rotation = Quaternion.LookRotation(Extensions.GetBezierFirstDerivative(p0, p1, p2, p3, time));

        if (time >= 1)
        {
            modelFx.StopVfx();
            hitFx.PlayVfx();
            OnArried?.Invoke();
        }
    }
}
