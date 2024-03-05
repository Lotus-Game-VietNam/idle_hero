using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace Lotus.CoreFramework
{
    public class ObjectPooling : Singleton<ObjectPooling>
    {



        protected override void Awake()
        {
            base.Awake();
            CreateContainersParent();
        }



        private void CreateContainersParent()
        {
            
        }


        private void Push<T>(T obj, Dictionary<string, List<T>> collection, Transform parent) where T : IPool
        {
            if (collection != null && collection.ContainsKey(obj.type) && !collection[obj.type].Contains(obj))
                collection[obj.type].Add(obj);

            obj.transform.SetParent(parent);

            obj.Hide();
        }

        private T Dequeue<T>(string type, Dictionary<string, List<T>> collection, string path, Transform newParent) where T : IPool
        {
            T obj = null;

            if (!collection.ContainsKey(type))
                collection.Add(type, new List<T>());

            if (collection[type].Count > 0)
                obj = collection[type][0];

            if (obj == null)
                obj = Addressables.InstantiateAsync(path).WaitForCompletion().GetComponent<T>();
            else
                collection[type].Remove(obj);

            obj.transform.SetParent(newParent);

            return obj;
        }
    }
}

