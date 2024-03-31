using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class FarmManager : MonoBehaviour
{
    [Title("Initial Setting")]
    [SerializeField] private Transform[] spawnPoint = null;


    [Title("Assets Reference")]
    [SerializeField] private AssetReference heroAsset = null;
    

    public CharacterBrain hero { get; private set; }

    public CharacterBrain monsterFarm { get; private set; }



    public bool onX2Income { get; private set; }

    

    private void Awake()
    {
        InitEvents();
        InitializedCharacter();
    }

    private void InitEvents()
    {
        this.AddListener<CharacterBrain>(EventName.OnCharacterDead, OnCharacterDead);
        this.AddListener<bool>(EventName.X2Income, X2Income);
    }

    private void InitializedCharacter()
    {
        hero = SpawnHero();
        monsterFarm = SpawnMonster();

        hero.SetTargetAttack(monsterFarm).Initial(DataManager.HeroData).Show();

        Transform point = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)];
        monsterFarm.SetTargetAttack(hero).Initial(ConfigManager.GetMonster(monsterFarm.type))/*.ResetTransform()*/
            .SetPosition(point.position).SetRotation(point.rotation).Show();
    }

    private CharacterBrain SpawnHero()
    {
        GameObject heroObj = heroAsset.InstantiateAsync(spawnPoint[0].position, spawnPoint[0].rotation, null).WaitForCompletion();
        return heroObj.GetComponent<CharacterBrain>();
    }

    private CharacterBrain SpawnMonster()
    {
        int monsterIndex = UnityEngine.Random.Range(0, 100) % 2 == 0 ? 1 : 2;
        string monsterName = $"Monster_{DataManager.WorldData.currentLevel}_{monsterIndex}";
        return this.DequeueCharacter(monsterName);
    }

    private void ReSpawnMonster()
    {
        monsterFarm = SpawnMonster();
        Transform point = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)];
        monsterFarm.SetTargetAttack(hero).Initial(ConfigManager.GetMonster(monsterFarm.type))/*.ResetTransform()*/
            .SetPosition(point.position).SetRotation(point.rotation).Show();
        hero.SetTargetAttack(monsterFarm);
    }

    private void ReciveRewards()
    {
        float gemToAdd = onX2Income ? DataManager.HeroData.GetMaxIncome() : DataManager.HeroData.GetMinIncome();
        hero.characterStats.OnHealthChanged(10);
        CollectionIcons.Instance.Show(5, monsterFarm.center.ConvertToRectTransform());
        this.DelayCall(1, () => { ResourceManager.Gem += gemToAdd; });
    }

    private void X2Income(bool value)
    {
        onX2Income = value;
    }


    private void OnCharacterDead(CharacterBrain character)
    {
        if (character.CharacterType == CharacterType.Hero)
        {

        }
        else
        {
            ReciveRewards();
            ReSpawnMonster();
        }
    }
}
