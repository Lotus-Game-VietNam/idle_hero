using UnityEngine;


namespace Lotus.CoreFramework
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance = null;


        protected virtual void Awake()
        {
            if (Instance == null)
                Instance = GetComponent<T>();
            else
                Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
        }
    }
}

