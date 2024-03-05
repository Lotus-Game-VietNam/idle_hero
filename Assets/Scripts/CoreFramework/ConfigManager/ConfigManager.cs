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
    }
}

