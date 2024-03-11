using System.Collections.Generic;


namespace Lotus.CoreFramework
{
    public class GameConfig
    {
        public Dictionary<ResourceType, float> DefaultResources = null;
        public Dictionary<ItemType, ItemData[]> ItemsConfig = null;


        public GameConfig()
        {

        }
    }
}


