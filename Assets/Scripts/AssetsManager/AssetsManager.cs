using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace Lotus.CoreFramework
{
    public sealed class AssetsManager
    {

        private static readonly string rewardSpritePath = "Assets/AddressableAssets/Sprites/Rewards/{0}.png";


        private static Dictionary<string, Sprite> rewardSprites = new Dictionary<string, Sprite>();




        public static Sprite LoadRewardSprite(string rewardType) => LoadAsset(rewardType, rewardSprites, rewardSpritePath);




        public static T LoadAsset<T>(string type, Dictionary<string, T> colletion, string path)
        {
            if (!colletion.ContainsKey(type))
                colletion.Add(type, Addressables.LoadAssetAsync<T>(string.Format(path, type)).WaitForCompletion());
            return colletion[type];
        }
    }
}

