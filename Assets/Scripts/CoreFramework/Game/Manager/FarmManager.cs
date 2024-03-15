using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class FarmManager : MonoBehaviour
{
    [Title("Initial Setting")]
    [SerializeField] private Transform[] spawnPoint = null; public enum SpawnPoint { Hero, Monster };


    [Title("Assets Reference")]
    [SerializeField] private AssetReference heroAsset = null;



    private void Awake()
    {
        SpawnHero();
        SpawnMonster();
    }

    private void SpawnHero()
    {
        GameObject heroObj = heroAsset.InstantiateAsync(spawnPoint[(int)SpawnPoint.Hero].position, spawnPoint[(int)SpawnPoint.Hero].rotation, null).WaitForCompletion();
        CharacterBrain hero = heroObj.GetComponent<CharacterBrain>();
        hero.Initial(DataManager.HeroData).Show();
    }

    private void SpawnMonster()
    {
        Transform trans = spawnPoint[(int)SpawnPoint.Monster];
        int monsterIndex = Random.Range(0, 100) % 2 == 0 ? 1 : 2;
        string monsterName = $"Monster_{DataManager.WorldData.currentLevel}_{2}";
        this.DequeueCharacter(monsterName).Initial(ConfigManager.GetMonster(monsterName)).ResetTransform().SetPosition(trans.position).SetRotation(trans.rotation).Show();
    }
}
