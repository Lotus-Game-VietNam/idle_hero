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

    public CharacterBrain monsterFarm { get; private set; }
    

    private void Awake()
    {
        InitEvents();
        InitializedCharacter();
    }

    private void InitEvents()
    {
        this.AddListener<CharacterBrain>(EventName.OnCharacterDead, OnCharacterDead);
    }

    private void InitializedCharacter()
    {
        hero = SpawnHero();
        monsterFarm = SpawnMonster();

        hero.SetTargetAttack(monsterFarm).Initial(DataManager.HeroData).Show();
        monsterFarm.SetTargetAttack(hero).Initial(ConfigManager.GetMonster(monsterFarm.type)).ResetTransform()
            .SetPosition(spawnPoint[(int)SpawnPoint.Monster].position).SetRotation(spawnPoint[(int)SpawnPoint.Monster].rotation).Show();
    }

    private CharacterBrain SpawnHero()
    {
        GameObject heroObj = heroAsset.InstantiateAsync(spawnPoint[(int)SpawnPoint.Hero].position, spawnPoint[(int)SpawnPoint.Hero].rotation, null).WaitForCompletion();
        return heroObj.GetComponent<CharacterBrain>();
    }

    private CharacterBrain SpawnMonster()
    {
        int monsterIndex = UnityEngine.Random.Range(0, 100) % 2 == 0 ? 1 : 2;
        string monsterName = $"Monster_{DataManager.WorldData.currentLevel}_{2}";
        return this.DequeueCharacter(monsterName);
    }

    private void OnCharacterDead(CharacterBrain character)
    {
        if (character.CharacterType == CharacterType.Hero)
        {

        }
        else
        {
            monsterFarm = SpawnMonster();
            monsterFarm.SetTargetAttack(hero).Initial(ConfigManager.GetMonster(monsterFarm.type)).ResetTransform()
            .SetPosition(spawnPoint[(int)SpawnPoint.Monster].position).SetRotation(spawnPoint[(int)SpawnPoint.Monster].rotation).Show();
            hero.SetTargetAttack(monsterFarm);
        }
    }
}
