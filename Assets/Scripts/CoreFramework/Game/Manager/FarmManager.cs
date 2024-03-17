using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class FarmManager : MonoBehaviour
{
    [Title("Initial Setting")]
    [SerializeField] private Transform[] spawnPoint = null; public enum SpawnPoint { Hero, Monster };


    [Title("Assets Reference")]
    [SerializeField] private AssetReference heroAsset = null;
    

    public CharacterBrain hero { get; private set; }

    public CharacterBrain enemyFarm { get; private set; }
    

    private void Awake()
    {
        InitEvents();
        InitializedCharacter();
    }

    private void InitEvents()
    {

    }

    private void InitializedCharacter()
    {
        GameObject heroObj = heroAsset.InstantiateAsync(spawnPoint[(int)SpawnPoint.Hero].position, spawnPoint[(int)SpawnPoint.Hero].rotation, null).WaitForCompletion();
        hero = heroObj.GetComponent<CharacterBrain>();

        Transform trans = spawnPoint[(int)SpawnPoint.Monster];
        int monsterIndex = UnityEngine.Random.Range(0, 100) % 2 == 0 ? 1 : 2;
        string monsterName = $"Monster_{DataManager.WorldData.currentLevel}_{2}";
        enemyFarm = this.DequeueCharacter(monsterName);

        hero.SetTargetAttack(enemyFarm).Initial(DataManager.HeroData).Show();
        enemyFarm.SetTargetAttack(hero).Initial(ConfigManager.GetMonster(monsterName)).ResetTransform().SetPosition(trans.position).SetRotation(trans.rotation).Show();
    }

}
