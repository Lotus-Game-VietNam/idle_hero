using UnityEngine;


namespace Lotus.CoreFramework
{
    public abstract class IPool : MonoBehaviour
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



        public void Show()
        {
            gameObject.SetActive(true);
            OnShow();
        }

        protected abstract void OnShow();

        public void Hide()
        {
            gameObject.SetActive(false);
            OnHide();
        }

        protected abstract void OnHide();
    }
}


