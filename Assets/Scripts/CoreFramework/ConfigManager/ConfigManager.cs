using Newtonsoft.Json;
using UnityEngine;


namespace Lotus.CoreFramework
{
    public static class ConfigManager
    {
        private static GameConfig _gameConfig = null;
        public static GameConfig GameConfig
        {
            get
            {
                if (_gameConfig == null)
                {
                    TextAsset textAsset = Resources.Load<TextAsset>("Config/GameConfig");
                    _gameConfig = JsonConvert.DeserializeObject<GameConfig>(textAsset.text);
                }

                return _gameConfig;
            }
        }


        public static ItemData GetItem(ItemType type, int level) => level >= GameConfig.ItemsConfig[type].Length ? null : GameConfig.ItemsConfig[type][level];
    }
}

