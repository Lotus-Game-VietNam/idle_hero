using System.Collections.Generic;


namespace Lotus.CoreFramework
{
    public class GameConfig
    {
        public Dictionary<ResourceType, float> DefaultResources = null;
        public Dictionary<ItemType, ItemData[]> ItemsConfig = null;
        public Dictionary<int, Ratio[]> BuyItemsRatio = null;
        public Dictionary<string, CharacterConfig> MonstersConfig = null;
        public IncomeConfig[] IncomeConfig = null;


        public GameConfig()
        {

        }
    }
}


