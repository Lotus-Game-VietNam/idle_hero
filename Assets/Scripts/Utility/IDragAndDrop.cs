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
            transform.DOJump(prevPos, 0.3f, 1, 0.25f);
            return;
        }
        
        OnDragEvent?.Invoke();

        isDrag = true;

        Vector3 newPosition = mouseWorldPos + mouseOffset;

        transform.position = Vector3.Lerp(transform.position, newPosition + (Vector3.up * dragOffset), dragLerpRate * Time.deltaTime);
    }

    private void OnMouseUp()
    {
        if (isDrag)
        {
            OnClickUpEvent?.Invoke();
            isDrag = false;
            transform.DOJump(prevPos, 0.3f, 1, 0.25f);
        }
    }
}
