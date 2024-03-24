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

        public static CharacterConfig GetMonster(string monsterName) => !GameConfig.MonstersConfig.ContainsKey(monsterName) ? null : GameConfig.MonstersConfig[monsterName];

        public static IncomeConfig GetIncome(int level) => level >= GameConfig.IncomeConfig.Length ? null : GameConfig.IncomeConfig[level];
    }
}

