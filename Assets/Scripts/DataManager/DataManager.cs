

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



        private static BuyItemsData _buyItemsData = null;
        public static BuyItemsData BuyItemsData
        {
            get
            {
                if (!ES3.KeyExists(GameConstants.BuyItemsDataKey))
                {
                    _buyItemsData = new BuyItemsData();
                    ES3.Save(GameConstants.BuyItemsDataKey, _buyItemsData);
                }

                if (_buyItemsData == null)
                    _buyItemsData = ES3.Load<BuyItemsData>(GameConstants.BuyItemsDataKey);

                return _buyItemsData;
            }
        }

        public static void Save(this BuyItemsData buyItemsData) => ES3.Save(GameConstants.BuyItemsDataKey, _buyItemsData);
    }
}

