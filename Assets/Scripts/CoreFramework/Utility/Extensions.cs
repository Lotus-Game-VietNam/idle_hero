using System;
using UnityEngine;


namespace Lotus.CoreFramework
{
    public static class Extensions
    {


        #region ObjectPooling

        public static void ResetTransform(this IPool obj)
        {
            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
        }



        #endregion



        #region Doozy UI

        public static PopupBase DequeuePopup(MonoBehaviour mono, string popupName) => PopupManager.Instance.Dequeue(popupName);

        public static PopupBase voidGetPopupVisible(MonoBehaviour mono, string popupName) => PopupManager.Instance.GetPopupVisible(popupName);

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

        #endregion
    }
}

