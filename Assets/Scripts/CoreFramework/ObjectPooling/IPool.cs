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


        protected abstract void Initialized(T data);

        public virtual IPool<T> Initial(T data)
        {
            Initialized(data);
            return this;
        }


        public IPool<T> Show()
        {
            gameObject.SetActive(true);
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
    }
}


