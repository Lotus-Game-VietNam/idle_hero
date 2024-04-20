using DG.Tweening;
using System;
using TMPro;
using UnityEngine;


namespace Lotus.CoreFramework
{
    public static class Extensions
    {


        #region ObjectPooling

        public static CharacterBrain DequeueCharacter(this MonoBehaviour mono, string type, Transform newParent = null)
        {
            return ObjectPooling.Instance.DequeueCharacter(type, newParent);
        }

        public static void PushCharacter(this MonoBehaviour mono, CharacterBrain character)
        {
            ObjectPooling.Instance.PushCharacter(character);
        }

        public static void PushCharacter(this CharacterBrain character)
        {
            ObjectPooling.Instance.PushCharacter(character);
        }

        public static ProjectileVfx DequeueProjectileVfx(this MonoBehaviour mono, string type, Transform newParent = null)
        {
            return ObjectPooling.Instance.DequeueProjectileVfx(type, newParent);
        }

        public static void PushProjectileVfx(this MonoBehaviour mono, ProjectileVfx projectile)
        {
            ObjectPooling.Instance.PushProjectileVfx(projectile);
        }

        public static void PushProjectileVfx(this ProjectileVfx projectile)
        {
            ObjectPooling.Instance.PushProjectileVfx(projectile);
        }

        public static EffectBase DequeueEffect(this MonoBehaviour mono, string type, Transform newParent = null)
        {
            return ObjectPooling.Instance.DequeueEffect(type, newParent);
        }

        public static void PushEffect(this MonoBehaviour mono, EffectBase effect)
        {
            ObjectPooling.Instance.PushEffect(effect);
        }

        public static void PushEffect(this EffectBase effect)
        {
            ObjectPooling.Instance.PushEffect(effect);
        }

        public static IconSprite DequeueIcon(this MonoBehaviour mono, string type, Transform newParent = null)
        {
            return ObjectPooling.Instance.DequeueIcon(type, newParent);
        }

        public static void PushIcon(this MonoBehaviour mono, IconSprite icon)
        {
            ObjectPooling.Instance.PushIcon(icon);
        }

        public static void PushIcon(this IconSprite icon)
        {
            ObjectPooling.Instance.PushIcon(icon);
        }

        public static InventoryItem DequeueItem(this MonoBehaviour mono, string type, Transform newParent = null)
        {
            return ObjectPooling.Instance.DequeueItem(type, newParent);
        }

        public static void PushItem(this MonoBehaviour mono, InventoryItem item)
        {
            ObjectPooling.Instance.PushItem(item);
        }

        public static void PushItem(this InventoryItem item)
        {
            ObjectPooling.Instance.PushItem(item);
        }

        #endregion



        #region Doozy UI

        public static PopupBase DequeuePopup(MonoBehaviour mono, string popupName) => PopupManager.Instance.Dequeue(popupName);

        public static PopupBase GetPopupVisible(MonoBehaviour mono, string popupName) => PopupManager.Instance.GetPopupVisible(popupName);

        #endregion



        #region EventDispatcher


        public static void AddListener(this MonoBehaviour mono, EventName eventName, Action action) => EventDispatcher.Addlistener(eventName.ToString(), mono.GetType().Name, action);

        public static void AddListener<T1>(this MonoBehaviour mono, EventName eventName, Action<T1> action) => EventDispatcher.Addlistener(eventName.ToString(), mono.GetType().Name, action);

        public static void AddListener<T1, T2>(this MonoBehaviour mono, EventName eventName, Action<T1, T2> action) => EventDispatcher.Addlistener(eventName.ToString(), mono.GetType().Name, action);

        public static void AddListener<T1, T2, T3>(this MonoBehaviour mono, EventName eventName, Action<T1, T2, T3> action) => EventDispatcher.Addlistener(eventName.ToString(), mono.GetType().Name, action);

        public static void AddListener<T1, T2, T3, T4>(this MonoBehaviour mono, EventName eventName, Action<T1, T2, T3, T4> action) => EventDispatcher.Addlistener(eventName.ToString(), mono.GetType().Name, action);

        public static void AddListener<T1, T2, T3, T4, T5>(this MonoBehaviour mono, EventName eventName, Action<T1, T2, T3, T4, T5> action) => EventDispatcher.Addlistener(eventName.ToString(), mono.GetType().Name, action);

        public static void SendMessage(this MonoBehaviour mono, EventName eventName, string subscribers) => EventDispatcher.SendMessage(eventName.ToString(), subscribers);

        public static void SendMessage(this MonoBehaviour mono, EventName eventName) => EventDispatcher.SendMessage(eventName.ToString());

        public static void SendMessage<T1>(this MonoBehaviour mono, EventName eventName, string subscribers, T1 args1) => EventDispatcher.SendMessage(eventName.ToString(), subscribers, args1);

        public static void SendMessage<T1>(this MonoBehaviour mono, EventName eventName, T1 args1) => EventDispatcher.SendMessage(eventName.ToString(), args1);

        public static void SendMessage<T1, T2>(this MonoBehaviour mono, EventName eventName, string subscribers, T1 args1, T2 args2) => EventDispatcher.SendMessage(eventName.ToString(), subscribers, args1, args2);

        public static void SendMessage<T1, T2>(this MonoBehaviour mono, EventName eventName, T1 args1, T2 args2) => EventDispatcher.SendMessage(eventName.ToString(), args1, args2);

        public static void SendMessage<T1, T2, T3>(this MonoBehaviour mono, EventName eventName, string subscribers, T1 args1, T2 args2, T3 args3) => 
            EventDispatcher.SendMessage(eventName.ToString(), subscribers, args1, args2, args3);

        public static void SendMessage<T1, T2, T3>(this MonoBehaviour mono, EventName eventName, T1 args1, T2 args2, T3 args3) => EventDispatcher.SendMessage(eventName.ToString(), args1, args2, args3);

        public static void SendMessage<T1, T2, T3, T4>(this MonoBehaviour mono, EventName eventName, string subscribers, T1 args1, T2 args2, T3 args3, T4 args4) =>
            EventDispatcher.SendMessage(eventName.ToString(), subscribers, args1, args2, args3, args4);

        public static void SendMessage<T1, T2, T3, T4>(this MonoBehaviour mono, EventName eventName, T1 args1, T2 args2, T3 args3, T4 args4) => EventDispatcher.SendMessage(eventName.ToString(), args1, args2, args3, args4);

        public static void SendMessage<T1, T2, T3, T4, T5>(this MonoBehaviour mono, EventName eventName, string subscribers, T1 args1, T2 args2, T3 args3, T4 args4, T5 args5) =>
            EventDispatcher.SendMessage(eventName.ToString(), subscribers, args1, args2, args3, args4, args5);

        public static void SendMessage<T1, T2, T3, T4, T5>(this MonoBehaviour mono, EventName eventName, T1 args1, T2 args2, T3 args3, T4 args4, T5 args5) => 
            EventDispatcher.SendMessage(eventName.ToString(), args1, args2, args3, args4, args5);

        public static void RemoveListener(this MonoBehaviour mono, EventName eventName) => EventDispatcher.RemoveListener(eventName.ToString(), mono.GetType().Name);

        public static void RemoveAllListener(this MonoBehaviour mono, EventName eventName) => EventDispatcher.RemoveAllListener(eventName.ToString());

        public static void RemoveSubscribers(this MonoBehaviour mono) => EventDispatcher.RemoveSubscribers(mono.GetType().Name);

        #endregion



        #region Helper

        public static void PlayVfx(this ParticleSystem vfx)
        {
            if (vfx != null)
                vfx.Play();
        }

        public static void StopVfx(this ParticleSystem vfx)
        {
            if (vfx != null)
                vfx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        public static Vector2 ConvertToAnchoredPosition(this RectTransform from, RectTransform to)
        {
            Vector2 localPoint;
            Vector2 fromPivotDerivedOffset = new Vector2(from.rect.width * from.pivot.x + from.rect.xMin, from.rect.height * from.pivot.y + from.rect.yMin);
            Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, from.position);
            screenP += fromPivotDerivedOffset;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out localPoint);
            Vector2 pivotDerivedOffset = new Vector2(to.rect.width * to.pivot.x + to.rect.xMin, to.rect.height * to.pivot.y + to.rect.yMin);
            return to.anchoredPosition + localPoint - pivotDerivedOffset;
        }

        public static Vector2 ConvertToRectTransform(this Vector3 position)
        {
            RectTransform mainRect = ComponentReference.MainRect.Invoke();
            Vector2 viewportPosition = Camera.main.WorldToViewportPoint(position);
            Vector2 screenPoint = new Vector2(
            ((viewportPosition.x * mainRect.rect.width) - (mainRect.rect.width * 0.5f)),
            ((viewportPosition.y * mainRect.rect.height) - (mainRect.rect.height * 0.5f)));
            return screenPoint;
        }

        public static Vector3 GetMouseWorldPosition(LayerMask mask)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                return hit.point;

            LogTool.LogErrorEditorOnly("Not Found Mouse World Point");
            return Vector3.zero;
        }

        public static bool IsMouseWorldPositionTriggerLayer(LayerMask mask)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                return true;

            return false;
        }

        public static RaycastHit GetMouseWorldHit(LayerMask mask)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                return hit;

            LogTool.LogErrorEditorOnly("Not Found Mouse World Point");
            return hit;
        }

        public static RaycastHit GetMouseWorldHit(LayerMask mask, Vector3 worldPos)
        {
            Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(worldPos));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                return hit;

            LogTool.LogErrorEditorOnly("Not Found Mouse World Point");
            return hit;
        }

        public static Vector3 GetQuadBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            Vector3 p01 = Vector3.Lerp(p0, p1, t);
            Vector3 p12 = Vector3.Lerp(p1, p2, t);
            Vector3 p23 = Vector3.Lerp(p2, p3, t);

            Vector3 p012 = Vector3.Lerp(p01, p12, t);
            Vector3 p123 = Vector3.Lerp(p12, p23, t);

            Vector3 p0123 = Vector3.Lerp(p012, p123, t);

            return p0123;
        }

        public static Vector3 GetBezierFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;

            return
                3f * oneMinusT * oneMinusT * (p1 - p0) +
                6f * oneMinusT * t * (p2 - p1) +
                3f * t * t * (p3 - p2);
        }
        #endregion
    }
}

