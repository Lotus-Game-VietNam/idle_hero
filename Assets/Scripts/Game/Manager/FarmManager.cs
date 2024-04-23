using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class FarmManager : MonoBehaviour
{
    [Title("Initial Setting")]
    [SerializeField] private Transform[] spawnPoint = null;
    [SerializeField] private Transform bossRenderParent = null;


    [Title("Assets Reference")]
    [SerializeField] private AssetReference heroAsset = null;


    public CharacterBrain hero { get; private set; }

    public CharacterBrain monsterFarm { get; private set; }


    public bool onAutoTap { get; private set; }

    public bool onX2Income { get; private set; }

    private EffectBase fireAuraVfx = null;



    private void Awake()
    {
        hero = SpawnHero();
        monsterFarm = SpawnMonster();

        hero.SetTargetAttack(monsterFarm).Initial(DataManager.HeroData).Show();

        Transform point = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)];
        monsterFarm.SetTargetAttack(hero).Initial(ConfigManager.GetMonster(monsterFarm.type))
            .SetPosition(point.position).SetRotation(point.rotation).Show();

        ComponentReference.BossRenderParent = () => bossRenderParent;
    }

    private void OnEnable()
    {
        this.AddListener(EventName.RefreshMonsterTarget, RefreshMonsterTarget);
        this.AddListener<CharacterBrain>(EventName.OnCharacterDead, OnCharacterDead);
        this.AddListener<bool>(EventName.X2Income, X2Income);
        this.AddListener<bool>(EventName.AutoTap, AutoTap);
    }

    private void OnDisable()
    {
        this.RemoveSubscribers();
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
        monsterFarm.SetTargetAttack(hero).Initial(ConfigManager.GetMonster(monsterFarm.type))
            .SetPosition(point.position).SetRotation(point.rotation).Show();
    }

    private void RefreshMonsterTarget()
    {
        hero.SetTargetAttack(monsterFarm);
    }

    private void ReciveRewards()
    {
        float gemToAdd = onX2Income ? GetGemsReward() * 2 : GetGemsReward();
        hero.characterStats.OnHealthChanged(10);
        CollectionIcons.Instance.Show(onAutoTap ? 10 : 5, monsterFarm.center.ConvertToRectTransform());
        this.DelayCall(1, () => { ResourceManager.Gem += gemToAdd; });
    }

    private void X2Income(bool value) => onX2Income = value;

    private void AutoTap(bool value)
    {
        onAutoTap = value;
        hero.animatorState.Ator.speed = value ? 1.5f : 1f;
        StartCoroutine(IEAuraFollowHero());
    }

    private IEnumerator IEAuraFollowHero()
    {
        if (!onAutoTap)
            yield break;

        fireAuraVfx = this.DequeueEffect("FireAura");
        fireAuraVfx.transform.position = hero.center + (transform.up * (hero.characterAttack.height / 2));
        fireAuraVfx.transform.rotation = Quaternion.identity;
        fireAuraVfx.Show();

        while (onAutoTap)
        {
            fireAuraVfx.transform.position = hero.center + (transform.up * (hero.characterAttack.height / 2));
            yield return null;
        }

        this.PushEffect(fireAuraVfx);
    }

    private float GetGemsReward() => onAutoTap ? DataManager.HeroData.GetMaxIncome() : DataManager.HeroData.GetMinIncome();


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
