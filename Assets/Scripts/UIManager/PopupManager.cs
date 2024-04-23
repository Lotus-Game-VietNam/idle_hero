using Doozy.Runtime.UIManager.Containers;
using System.Collections.Generic;
using UnityEngine;


namespace Lotus.CoreFramework
{
    public class PopupManager : Singleton<PopupManager>
    {
        private RectTransform _visibleParent = null;
        public RectTransform visibleParent
        {
            get
            {
                if (_visibleParent == null)
                    _visibleParent = transform.GetChild(1) as RectTransform;
                return _visibleParent;
            }
        }


        private RectTransform _poolingParent = null;
        public RectTransform poolingParen
        {
            get
            {
                if (_poolingParent == null)
                    _poolingParent = transform.GetChild(0) as RectTransform;
                return _poolingParent;
            }
        }



        public Dictionary<string, List<PopupBase>> popups { get; private set; } = new Dictionary<string, List<PopupBase>>();



        public PopupBase Dequeue(string popupName)
        {
            PopupBase popup = null;

            if (!popups.ContainsKey(popupName))
                popups.Add(popupName, new List<PopupBase>());

            if (popups[popupName].Count > 0)
                popup = popups[popupName][0];

            if (popup == null)
            {
                UIPopup _popup = UIPopup.Get(popupName);
                popup = _popup.GetComponent<PopupBase>();
            }    
                
            else
            {
                popups[popupName].Remove(popup);
            }

            popup.SetParent(visibleParent);

            popup.popup.OnHiddenCallback.Event.RemoveAllListeners();

            popup.popup.OnHiddenCallback.Event.AddListener(() => { Push(popup); });

            return popup;
        }

        public void Push(PopupBase popup)
        {
            if (popups != null && popups.ContainsKey(popup.popupName) && !popups[popup.popupName].Contains(popup))
                popups[popup.popupName].Add(popup);

            popup.SetParent(poolingParen);
        }

        public PopupBase GetPopupVisible(string popupName)
        {
            if (visibleParent.childCount <= 0)
                return null;

            for (int i = 0; i < visibleParent.childCount; i++)
            {
                PopupBase currentPopup = visibleParent.GetChild(i).GetComponent<PopupBase>();

                if (currentPopup == null)
                    continue;

                if (currentPopup.popupName.Equals(popupName))
                    return currentPopup;
            }

            return null;
        }

        public bool IsVisible(string popupName)
        {
            return GetPopupVisible(popupName) != null;
        }
    }
}

