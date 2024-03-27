

namespace Lotus.CoreFramework
{
    public static class DataManager
    {

        private static HeroData _heroData = null;
        public static HeroData HeroData
        {
            get
            {
                if (!ES3.KeyExists(GameConstants.heroDataKey))
                {
                    _heroData = new HeroData();
                    ES3.Save(GameConstants.heroDataKey, _heroData);
                }

                if (_heroData == null)
                    _heroData = ES3.Load<HeroData>(GameConstants.heroDataKey);

                return _heroData;
            }
        }

        public static void Save(this HeroData heroData) => ES3.Save(GameConstants.heroDataKey, _heroData);



        private static WorldData _worldData = null;
        public static WorldData WorldData
        {
            get
            {
                if (!ES3.KeyExists(GameConstants.worldDataKey))
                {
                    _worldData = new WorldData();
                    ES3.Save(GameConstants.worldDataKey, _worldData);
                }

                if (_worldData == null)
                    _worldData = ES3.Load<WorldData>(GameConstants.worldDataKey);

                return _worldData;
            }
        }

        public static void Save(this WorldData worldData) => ES3.Save(GameConstants.worldDataKey, _worldData);



        private static InventoryData _inventoryData = null;
        public static InventoryData InventoryData
        {
            get
            {
                if (!ES3.KeyExists(GameConstants.BuyItemsDataKey))
                {
                    _inventoryData = new InventoryData();
                    ES3.Save(GameConstants.BuyItemsDataKey, _inventoryData);
                }

                if (_inventoryData == null)
                    _inventoryData = ES3.Load<InventoryData>(GameConstants.BuyItemsDataKey);

                return _inventoryData;
            }
        }

        public static void Save(this InventoryData buyItemsData) => ES3.Save(GameConstants.BuyItemsDataKey, _inventoryData);
    }
}

