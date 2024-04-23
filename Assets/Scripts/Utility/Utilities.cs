using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lotus.CoreFramework
{
    public static class Utilities
    {
        public static List<string> units = new List<string> { "K", "M", "B", "T", "P", "E", "Z" };


        public static float Convert(this string value)
        {
            if (Char.IsDigit(value.Last()))
                return float.Parse(value);

            int unitIndex = units.IndexOf(value.Last().ToString());

            value = value.Remove(value.Length - 1);

            float number = float.Parse(value);

            if (unitIndex >= 0)
                number *= (float)Math.Pow(10, (unitIndex + 1) * 3);

            return number;
        }

        public static string Convert(this float value)
        {
            int exp = (int)Math.Floor(Math.Log10(value) / 3);
            float scaledValue = value / (float)Math.Pow(10, exp * 3);

            if (exp > 0)
                return scaledValue.ToString("F2") + units[exp - 1];
            else
                return value.ToString();
        }

        public static string Convert(this int value)
        {
            int exp = (int)Math.Floor(Math.Log10(value) / 3);
            int scaledValue = Mathf.RoundToInt(value / (float)Math.Pow(10, exp * 3));

            if (exp > 0)
                return scaledValue.ToString("F2") + units[exp - 1];
            else
                return value.ToString();
        }

        public static bool IsOnBattle(this SceneName sceneName) => (int)sceneName % 2 == 1;

        public static bool IsOnFarm(this SceneName sceneName) => (int)sceneName % 2 == 0;


        public static AnimationStates Convert(this AttackType type) => (AnimationStates)((int)AnimationStates.NormalAttack + (int)type);

        public static bool IsMovement(this AnimationStates state)
        {
            return (int)state >= (int)AnimationStates.Idle && (int)state <= (int)AnimationStates.Run;
        }

        public static bool IsAttack(this AnimationStates state)
        {
            return (int)state >= (int)AnimationStates.NormalAttack && (int)state <= (int)AnimationStates.SkillThird;
        }


        public static void Stop(this Tween tween)
        {
            if (tween != null)
                tween.Kill();
        }

        public static void Active(this CanvasGroup canvasGr, bool value)
        {
            if (value)
                canvasGr.Active();
            else
                canvasGr.DeActive();
        }

        public static void Active(this CanvasGroup canvasGr)
        {
            canvasGr.interactable = canvasGr.blocksRaycasts = true;
            canvasGr.alpha = 1;
        }

        public static void Active(this CanvasGroup canvasGr, float duration, Action Callback = null)
        {
            canvasGr.interactable = canvasGr.blocksRaycasts = true;
            canvasGr.alpha = 0;
            canvasGr.DOFade(1, duration).OnComplete(() => { Callback?.Invoke(); });
        }

        public static void DeActive(this CanvasGroup canvasGr)
        {
            canvasGr.interactable = false;
            canvasGr.blocksRaycasts = false;
            canvasGr.alpha = 0;
        }

        public static void DeActive(this CanvasGroup canvasGr, float duration, Action Callback = null)
        {
            canvasGr.interactable = canvasGr.blocksRaycasts = false;
            canvasGr.DOFade(0, duration).OnComplete(() => { Callback?.Invoke(); });
        }


        public static Coroutine DelayCall<T1, T2>(this MonoBehaviour mono, float time, Action<T1, T2> Callback, T1 param1, T2 param2)
        {
            return mono.StartCoroutine(IEDelayCall(time, Callback, param1, param2));
        }

        private static IEnumerator IEDelayCall<T1, T2>(float time, Action<T1, T2> Callback, T1 param1, T2 param2)
        {
            yield return new WaitForSeconds(time);
            Callback?.Invoke(param1, param2);
        }

        public static Coroutine DelayCall<T>(this MonoBehaviour mono, float time, Action<T> Callback, T param)
        {
            return mono.StartCoroutine(IEDelayCall(time, Callback, param));
        }

        private static IEnumerator IEDelayCall<T>(float time, Action<T> Callback, T param)
        {
            yield return new WaitForSeconds(time);
            Callback?.Invoke(param);
        }

        public static Coroutine DelayCall(this MonoBehaviour mono, float time, Action Callback)
        {
            return mono.StartCoroutine(IEDelayCall(time, Callback));
        }

        private static IEnumerator IEDelayCall(float time, Action Callback)
        {
            yield return new WaitForSeconds(time);
            Callback?.Invoke();
        }


        public static T TryGetComponent<T>(this MonoBehaviour mono, ref T component)
        {
            if (component == null)
                component = mono.GetComponent<T>();
            return component;
        }

        public static T TryGetComponent<T>(this T component, MonoBehaviour obj)
        {
            if (component == null)
                component = obj.GetComponent<T>();
            return component;
        }

        public static T TryGetComponentInChildren<T>(this MonoBehaviour mono, ref T component)
        {
            if (component == null)
                component = mono.GetComponentInChildren<T>();
            return component;
        }

        public static T[] TryGetComponentsInChildren<T>(this MonoBehaviour mono, ref T[] components)
        {
            if (components == null)
                components = mono.GetComponentsInChildren<T>();
            return components;
        }

        public static string GameObjectName(this MonoBehaviour mono) => mono.gameObject.name.Replace("(Clone)", "");

        public static void CreateContainer(string name, Transform parent, ref Transform container)
        {
            GameObject obj = new GameObject(name);
            container = obj.transform;
            container.SetParent(parent);
        }

        public static void CreateContainer(this MonoBehaviour parent, string name, ref Transform container)
        {
            GameObject obj = new GameObject(name);
            container = obj.transform;
            container.SetParent(parent.transform);
        }
    }
}

