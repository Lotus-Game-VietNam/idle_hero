using DG.Tweening;
using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider))]
public class IDragAndDrop : MonoBehaviour
{
    [Title("Configuration")]
    public LayerMask dragOnLayer;
    public float dragOffset = 0.2f;
    public float doubleClickThreshold = 0.25f;
    public float dragLerpRate = 30f;


    private Vector3 currentMousePos;
    private Vector3 prevPos;
    private Vector3 mouseOffset;
    private Vector3 mouseWorldPos;

    private float lastClickTime = 0;

    private bool isDrag = false;


    public UnityEvent OnDoubleClickEvent { get; set; } = new UnityEvent();
    public UnityEvent OnClickDownEvent { get; set; } = new UnityEvent();
    public UnityEvent OnClickUpEvent { get; set; } = new UnityEvent();
    public UnityEvent OnDragEvent { get; set; } = new UnityEvent();


    private bool autoScale = true;
    private float initialDistance;
    private Vector3 initialScale;
    private Camera mainCamera;


    private void Start()
    {
        initialScale = transform.localScale;
        mainCamera = Camera.main;
        initialDistance = Vector3.Distance(transform.position, mainCamera.transform.position);
    }

    public void SetAutoScale(bool value) => autoScale = value;


    private void OnMouseDown()
    {
        OnClickDownEvent?.Invoke();

        prevPos = transform.position;

        mouseWorldPos = Extensions.GetMouseWorldPosition(dragOnLayer);
        mouseOffset = transform.position - mouseWorldPos;

        if (Time.time - lastClickTime < doubleClickThreshold)
            OnDoubleClickEvent.Invoke();

        lastClickTime = Time.time;
        currentMousePos = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        if (currentMousePos == Input.mousePosition) return;

        mouseWorldPos = Extensions.GetMouseWorldPosition(dragOnLayer);

        if (mouseWorldPos == Vector3.zero)
        {
            ResetToPrevPos();
            return;
        }
        
        OnDragEvent?.Invoke();

        isDrag = true;

        Vector3 newPosition = mouseWorldPos + mouseOffset;

        transform.position = Vector3.Lerp(transform.position, newPosition + (Vector3.up * dragOffset), dragLerpRate * Time.deltaTime);

        AutoScale();
    }

    private void OnMouseUp()
    {
        if (isDrag)
        {
            OnClickUpEvent?.Invoke();
            isDrag = false;
            ResetToPrevPos();
        }
    }

    public void ResetToPrevPos()
    {
        transform.DOJump(prevPos, 0.3f, 1, 0.25f);
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
