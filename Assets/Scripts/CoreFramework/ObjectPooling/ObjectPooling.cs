using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace Lotus.CoreFramework
{
    [DefaultExecutionOrder(-100)]
    public class ObjectPooling : Singleton<ObjectPooling>
    {
        private readonly string charactersPath = "Assets/AddressableLocalAssets/Prefabs/Character/{0}.prefab";


        private Dictionary<string, List<CharacterBrain>> characters = new Dictionary<string, List<CharacterBrain>>();


        private Transform  charactersContainer = null;




        protected override void Awake()
        {
            base.Awake();
            CreateContainersParent();
        }



        private void CreateContainersParent()
        {
            this.CreateContainer("Characters", ref charactersContainer);
        }


        public CharacterBrain DequeueCharacter(string characterName, Transform newParent = null) => Dequeue(characterName, characters, string.Format(charactersPath, characterName), newParent);

        public void PushCharacter(CharacterBrain character) => Push(character, characters, charactersContainer).Hide();



        private T Push<T>(T obj, Dictionary<string, List<T>> collection, Transform parent) where T : MonoBehaviour, IStruct
        {
            if (collection != null && collection.ContainsKey(obj.type) && !collection[obj.type].Contains(obj))
                collection[obj.type].Add(obj);

            obj.transform.SetParent(parent);

            return obj;
        }

        private T Dequeue<T>(string type, Dictionary<string, List<T>> collection, string path, Transform newParent) where T : MonoBehaviour
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

