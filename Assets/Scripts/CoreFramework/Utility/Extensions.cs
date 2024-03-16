using System;
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

        public static ProjectileVfx DequeueProjectileVfx(this MonoBehaviour mono, string type, Transform newParent = null)
        {
            return ObjectPooling.Instance.DequeueProjectileVfx(type, newParent);
        }

        public static void PushProjectileVfx(this MonoBehaviour mono, ProjectileVfx projectile)
        {
            ObjectPooling.Instance.PushProjectileVfx(projectile);
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

        #endregion
    }
}

