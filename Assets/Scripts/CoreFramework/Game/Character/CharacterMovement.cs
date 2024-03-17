using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    [Title("Configuration")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float angularSpeed = 60f;


    public NavMeshAgent agent { get; private set; }

    public Transform body => agent.transform;

    public bool crtRotating { get; private set; }

    public Action Arrived = null;

    private Coroutine rotateCrt = null;
    private Vector3 direction = Vector3.zero;
    private Vector3 navmeshPathPostion = Vector3.zero;
    private Quaternion startQuaternion = Quaternion.identity;
    private Quaternion targetQuaternion = Quaternion.identity;
    private NavMeshPath navmeshPath = null;
    private NavMeshHit hit;
    private int walkableArea;
    


    public void Initialized(NavMeshAgent agent)
    {
        this.agent = agent;
        hit = new NavMeshHit();
        navmeshPath = new NavMeshPath();
        walkableArea = NavMesh.GetAreaFromName("Walkable");
    }

    public void MoveTo(Vector3 target) => MoveTo(target, agent.radius * 2);

    public void MoveTo(Vector3 target, float stoppingDistance)
    {
        if (!agent.CalculatePath(target, navmeshPath))
        {
            LogTool.LogErrorEditorOnly(GetComponentInParent<HeroBrain>().gameObject.name + " calculatePath fail");
            return;
        }

        navmeshPathPostion = agent.SamplePathPosition(walkableArea, 1, out hit) ? hit.position : navmeshPath.corners[1];

        if (!agent.isOnNavMesh)
        {
            LogTool.LogErrorEditorOnly(GetComponentInParent<HeroBrain>().gameObject.name + " not is on Navmesh");
            return;
        }

        RotateTo(navmeshPathPostion);
        agent.Move(transform.forward * moveSpeed * Time.deltaTime);

        if (DistanceToTarget(target) <= stoppingDistance)
            Arrived?.Invoke();
    }

    /// <summary>
    /// Xoay vật thể hướng vào target. Hoạt động trong Update
    /// </summary>
    /// <param name="target"></param>
    /// <param name="rotateSpeed"></param>
    /// <param name="Callback"></param>
    public void RotateTo(Vector3 target)
    {
        direction = target - transform.position;
        direction.y = transform.forward.y;
        targetQuaternion = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, angularSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Xoay vật thể hướng vào target. Không hoạt động trong Update
    /// </summary>
    /// <param name="target"></param>
    /// <param name="rotateSpeed"></param>
    /// <param name="Callback"></param>
    public void RotateToCrt(Vector3 target, float rotateSpeed, Action Callback = null)
    {
        if (rotateCrt != null)
            StopCoroutine(rotateCrt);
        rotateCrt = StartCoroutine(IERotateTo(target, rotateSpeed, Callback));
    }

    protected IEnumerator IERotateTo(Vector3 target, float duration, Action Callback)
    {
        crtRotating = true;
        direction = target - body.position;
        direction.y = body.forward.y;
        startQuaternion = body.rotation;
        targetQuaternion = Quaternion.LookRotation(direction);
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
        crtRotating = false;
    }

    public void StopRotateCrt()
    {
        if (rotateCrt != null)
        {
            StopCoroutine(rotateCrt);
            rotateCrt = null;
        }
    }

    public float DistanceToTarget(Vector3 target) => Vector3.Distance(transform.position, new Vector3(target.x, transform.position.y, target.z));
}
