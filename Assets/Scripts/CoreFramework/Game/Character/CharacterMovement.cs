using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Title("Configuration")]
    [SerializeField] private float angularSpeed = 60f;


    public Transform body { get; private set; }

    private Coroutine rotateCrt = null;



    public void Initialized(Transform body)
    {
        this.body = body;
    }

    public void RotateToTargetCoroutine(Vector3 target, float rotateSpeed, Action Callback = null)
    {
        if (rotateCrt != null)
            StopCoroutine(rotateCrt);
        rotateCrt = StartCoroutine(IERotateToTarget(target, rotateSpeed, Callback));
    }

    protected IEnumerator IERotateToTarget(Vector3 target, float duration, Action Callback)
    {
        Vector3 direction = target - body.position;
        direction.y = body.forward.y;
        Quaternion startQuaternion = body.rotation;
        Quaternion targetQuaternion = Quaternion.LookRotation(direction);
        float count = 0;
        float lerpValue = 0;
        while (lerpValue < 1)
        {
            count += Time.deltaTime;
            lerpValue = count / duration;
            body.rotation = Quaternion.Slerp(startQuaternion, targetQuaternion, lerpValue);
            yield return null;
        }

        Callback?.Invoke();
    }

    public void StopRotateCrt()
    {
        if (rotateCrt != null)
        {
            StopCoroutine(rotateCrt);
            rotateCrt = null;
        }
    }
}
