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

    
    public CharacterBrain hero { get; private set; }

    public CharacterBrain enemyFarm { get; private set; }
    

    private void Awake()
    {
        InitEvents();
        SpawnHero();
        SpawnMonster();
    }

    private void InitEvents()
    {
        
    }

    private void SpawnHero()
    {
        GameObject heroObj = heroAsset.InstantiateAsync(spawnPoint[(int)SpawnPoint.Hero].position, spawnPoint[(int)SpawnPoint.Hero].rotation, null).WaitForCompletion();
        hero = heroObj.GetComponent<CharacterBrain>();
        hero.SetTargetAttack(enemyFarm).Initial(DataManager.HeroData).Show();
    }

    private void SpawnMonster()
    {
        Transform trans = spawnPoint[(int)SpawnPoint.Monster];
        int monsterIndex = Random.Range(0, 100) % 2 == 0 ? 1 : 2;
        string monsterName = $"Monster_{DataManager.WorldData.currentLevel}_{2}";
        enemyFarm = this.DequeueCharacter(monsterName);
        enemyFarm.SetTargetAttack(hero).Initial(ConfigManager.GetMonster(monsterName)).ResetTransform().SetPosition(trans.position).SetRotation(trans.rotation).Show();
    }
}
