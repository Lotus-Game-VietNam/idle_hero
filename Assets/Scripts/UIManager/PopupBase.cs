using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

namespace Lotus.CoreFramework
{
    public abstract class PopupBase : MonoBehaviour
    {
        private string _popupName = string.Empty;
        public string popupName
        {
            get
            {
                if (string.IsNullOrEmpty(_popupName))
                    _popupName = this.GameObjectName();
                return _popupName;
            }
        }


        public UIPopup popup { get; private set; }


        protected virtual void Awake()
        {
            popup = GetComponent<UIPopup>();
        }

        public void Show()
        {
            UpdateContent();
            popup.Show();
        }

        public void Hide() => popup.Hide();

        protected abstract void UpdateContent();

        public UIPopup SetParent(RectTransform parent) => popup.SetParent(parent);
    }


    
}



