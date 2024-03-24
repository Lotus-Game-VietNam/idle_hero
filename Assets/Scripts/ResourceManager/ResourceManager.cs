using System.Collections.Generic;
using UnityEngine.Events;


namespace Lotus.CoreFramework
{
    public static class ResourceManager
    {
        public static UnityEvent<ResourceType, float> OnResourceChanged = new UnityEvent<ResourceType, float>();


        private static Dictionary<ResourceType, float> _resources = null;
        public static Dictionary<ResourceType, float> Resources
        {
            get
            {
                if (!ES3.KeyExists(GameConstants.resourcesKey))
                {
                    _resources = ConfigManager.GameConfig.DefaultResources;
                    ES3.Save(GameConstants.resourcesKey, _resources);
                }

                if (_resources == null)
                    _resources = ES3.Load<Dictionary<ResourceType, float>>(GameConstants.resourcesKey);

                return _resources;
            }
        }


        public static float Gem
        {
            get => Resources[ResourceType.Gem];
            set => SetValue(ResourceType.Gem, value);
        }


        public static void SetValue(ResourceType type, float newValue)
        {
            Resources[type] = newValue;
            ES3.Save(GameConstants.resourcesKey, Resources);
            OnResourceChanged?.Invoke(type, newValue);
        }
    }
}

