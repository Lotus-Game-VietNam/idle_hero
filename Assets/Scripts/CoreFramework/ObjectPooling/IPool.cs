using Sirenix.OdinInspector;
using System;
using UnityEngine;


namespace Lotus.CoreFramework
{
    public interface IStruct
    {
        string type { get; }
    }

    public abstract class IPool<T> : MonoBehaviour, IStruct where T : class
    {
        [DetailedInfoBox("Deactive prefab này khi set up...", "Object này đang sử dụng object pooling, khi setup xong prefab này hãy Deactive game object đi để Pooling xử lý đúng logic")]

        private string _type = string.Empty;
        public string type
        {
            get
            {
                if (string.IsNullOrEmpty(_type))
                    _type = this.GameObjectName();
                return _type;
            }
        }

        private RectTransform _rect = null;
        public RectTransform rect => this.TryGetComponent(ref _rect);

        public abstract bool autoHide { get; }
        public virtual float timeToHide { get; }
        public abstract Action HideAct { get; }

        private Coroutine hideCrt = null;


        protected Action ShowFinishEvent = null;
        protected Action HideFinishEvent = null;


        public T data { get; private set; }


        protected abstract void Initialized(T data);

        public virtual IPool<T> Initial(T data)
        {
            this.data = data;
            Initialized(data);
            return this;
        }


        public IPool<T> Show()
        {
            gameObject.SetActive(true);
            if (autoHide) 
                WaitToHide();
            OnShow();
            return this;
        }

        protected abstract void OnShow();

        public IPool<T> Hide()
        {
            gameObject.SetActive(false);
            OnHide();
            return this;
        }

        protected abstract void OnHide();

        private void WaitToHide()
        {
            if (hideCrt != null)
                StopCoroutine(hideCrt);
            hideCrt = this.DelayCall(timeToHide, () => { HideAct?.Invoke(); });
        }

        public IPool<T> ResetTransform()
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            return this;
        }

        public IPool<T> SetPosition(Vector3 position)
        {
            transform.position = position;
            return this;
        }

        public IPool<T> SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
            return this;
        }

        public IPool<T> ResetRectTransform()
        {
            if (rect == null) throw null;
            rect.anchoredPosition = Vector3.zero;
            rect.rotation = Quaternion.identity;
            rect.localScale = Vector3.one;
            return this;
        }

        public IPool<T> SetAnchoredPosition(Vector2 anchoredPosition)
        {
            if (rect == null) throw null;
            rect.anchoredPosition = anchoredPosition;
            return this;
        }

        public IPool<T> SetParent(Transform parent)
        {
            transform.SetParent(parent);
            return this;
        }

        public IPool<T> SetParent(RectTransform parent)
        {
            if (rect == null) throw null;
            rect.SetParent(parent);
            return this;
        }

        public IPool<T> SetShowFinishEvent(Action Callback)
        {
            ShowFinishEvent = Callback;
            return this;
        }

        public IPool<T> SetHideFinishEvent(Action Callback)
        {
            HideFinishEvent = Callback;
            return this;
        }
    }
}


