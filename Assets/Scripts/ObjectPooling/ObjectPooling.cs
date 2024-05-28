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
        private readonly string modelsRenderPath = "Assets/AddressableLocalAssets/Prefabs/Render/{0}.prefab";
        private readonly string projectileVfxsPath = "Assets/AddressableLocalAssets/Prefabs/ProjectileVfx/{0}.prefab";
        private readonly string effectsPath = "Assets/AddressableLocalAssets/Prefabs/Effects/{0}.prefab";
        private readonly string iconsPath = "Assets/AddressableLocalAssets/Prefabs/Icons/{0}.prefab";
        private readonly string itemsPath = "Assets/AddressableLocalAssets/Prefabs/Items/{0}.prefab";
        private readonly string costumesPath = "Assets/AddressableLocalAssets/Prefabs/Costumes/{0}.prefab";


        private Dictionary<string, List<CharacterBrain>> characters = new Dictionary<string, List<CharacterBrain>>();
        private Dictionary<string, List<RenderModel>> modelsRender = new Dictionary<string, List<RenderModel>>();
        private Dictionary<string, List<ProjectileVfx>> projectileVfxs = new Dictionary<string, List<ProjectileVfx>>();
        private Dictionary<string, List<EffectBase>> effects = new Dictionary<string, List<EffectBase>>();
        private Dictionary<string, List<IconSprite>> icons = new Dictionary<string, List<IconSprite>>();
        private Dictionary<string, List<InventoryItem>> items = new Dictionary<string, List<InventoryItem>>();
        private Dictionary<string, List<CostumeModel>> costumes = new Dictionary<string, List<CostumeModel>>();


        private Transform  charactersContainer = null;
        private Transform modelsRenderContainer = null;
        private Transform projectileVfxsContainer = null;
        private Transform effectsContainer = null;
        private Transform iconsContainer = null;
        private Transform itemsContainer = null;
        private Transform costumesContainer = null;




        protected override void Awake()
        {
            base.Awake();
            CreateContainersParent();
        }



        private void CreateContainersParent()
        {
            this.CreateContainer("Characters", ref charactersContainer);
            this.CreateContainer("ProjectileVfs", ref projectileVfxsContainer);
            this.CreateContainer("Effects", ref effectsContainer);
            this.CreateContainer("Icons", ref iconsContainer);
            this.CreateContainer("Items", ref itemsContainer);
            this.CreateContainer("Models Render", ref modelsRenderContainer);
            this.CreateContainer("Costumes", ref costumesContainer);
        }


        public CostumeModel DequeueCostumeModel(string modelName, Transform newParent = null) => Dequeue(modelName, costumes, string.Format(costumesPath, modelName), newParent);

        public void PushCostumeModel(CostumeModel model) => Push(model, costumes, costumesContainer).Hide();


        public RenderModel DequeueRenderModel(string modelName, Transform newParent = null) => Dequeue(modelName, modelsRender, string.Format(modelsRenderPath, modelName), newParent);

        public void PushRenderModel(RenderModel model) => Push(model, modelsRender, modelsRenderContainer).Hide();



        public InventoryItem DequeueItem(string itemName, Transform newParent = null) => Dequeue(itemName, items, string.Format(itemsPath, itemName), newParent);

        public void PushItem(InventoryItem item) => Push(item, items, itemsContainer).Hide();



        public IconSprite DequeueIcon(string iconName, Transform newParent = null) => Dequeue(iconName, icons, string.Format(iconsPath, iconName), newParent);

        public void PushIcon(IconSprite icon) => Push(icon, icons, iconsContainer).Hide();



        public EffectBase DequeueEffect(string effectName, Transform newParent = null) => Dequeue(effectName, effects, string.Format(effectsPath, effectName), newParent);

        public void PushEffect(EffectBase effect) => Push(effect, effects, effectsContainer).Hide();



        public ProjectileVfx DequeueProjectileVfx(string projectileName, Transform newParent = null) => Dequeue(projectileName, projectileVfxs, string.Format(projectileVfxsPath, projectileName), newParent);

        public void PushProjectileVfx(ProjectileVfx projectileVfx) => Push(projectileVfx, projectileVfxs, projectileVfxsContainer).Hide();



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

