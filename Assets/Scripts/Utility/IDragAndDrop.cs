using DG.Tweening;
using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System;
using UnityEngine;


/// <summary>
/// Đây là plugin để thao tác kéo thả vật thể 3D, không sửa code trong script này, cần thêm logic gì thì sửa ở class kế thừa nó
/// </summary>
/// <typeparam name="T"></typeparam>


[RequireComponent(typeof(Collider))]
public abstract class IDragAndDrop<T> : MonoBehaviour where T : Component
{
    [Title("Configuration")]
    public LayerMask dragOnLayer;
    public float dragOffset = 0.4f;
    public float doubleClickThreshold = 0.25f;
    public float dragLerpRate = 30f;


    public T data { get; private set; }
    public Vector3 worldPosition => transform.position;


    private Vector3 currentMousePos;
    private Vector3 prevPos;
    private Vector3 mouseOffset;
    private Vector3 mouseWorldPos;

    private float lastClickTime = 0;

    private bool isDrag = false;


    public Action<IDragAndDrop<T>> OnDoubleTouchEvent = null;
    public Action<IDragAndDrop<T>> OnTouchEvent = null;
    public Action<IDragAndDrop<T>> OnDropEvent = null;
    public Action<IDragAndDrop<T>> OnDragEvent = null;


    protected abstract bool autoScale { get; }

    private float initialDistance;
    private Vector3 initialScale;
    private Camera mainCamera;



    public virtual void Initialized(T data)
    {
        this.data = data;
    }


    private void Start()
    {
        initialScale = transform.localScale;
        mainCamera = Camera.main;
        initialDistance = Vector3.Distance(transform.position, mainCamera.transform.position);
    }


    protected virtual void OnMouseDown()
    {
        OnTouchEvent?.Invoke(this);

        prevPos = transform.position;

        mouseWorldPos = Extensions.GetMouseWorldPosition(dragOnLayer);
        mouseOffset = transform.position - mouseWorldPos;

        if (Time.time - lastClickTime < doubleClickThreshold)
            OnDoubleTouchEvent.Invoke(this);

        lastClickTime = Time.time;
        currentMousePos = Input.mousePosition;
    }

    protected virtual void OnMouseDrag()
    {
        if (currentMousePos == Input.mousePosition) return;

        mouseWorldPos = Extensions.GetMouseWorldPosition(dragOnLayer);

        if (mouseWorldPos == Vector3.zero)
        {
            RevertToPrevPos();
            return;
        }
        
        OnDragEvent?.Invoke(this);

        isDrag = true;

        Vector3 newPosition = mouseWorldPos + mouseOffset;

        transform.position = Vector3.Lerp(transform.position, newPosition + (Vector3.up * dragOffset), dragLerpRate * Time.deltaTime);

        AutoScale();
    }

    protected virtual void OnMouseUp()
    {
        if (isDrag)
        {
            OnDropEvent?.Invoke(this);
            isDrag = false;
        }
    }

    public void RevertToPrevPos() => MoveToPos(prevPos);

    public void MoveToPos(Vector3 pos)
    {
        transform.DOJump(pos, 0.3f, 1, 0.25f);
        transform.DOScale(initialScale, 0.25f);
    }

    private void AutoScale()
    {
        if (!autoScale) return;

        float distanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);

        float ratio = distanceToCamera / initialDistance;

        transform.localScale = ratio * initialScale;
    }
}
