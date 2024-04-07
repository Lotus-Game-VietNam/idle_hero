using Cinemachine;
using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BattleManager : MonoBehaviour
{
    [Title("Initial Setting")]
    [SerializeField] private Transform[] spawnPoint = null;


    [Title("Assets Reference")]
    [SerializeField] private AssetReference heroAsset = null;


    [Title("Cinemachine Camera")]
    [SerializeField] private CinemachineVirtualCamera topdownCamera = null;


    [Title("Game UI")]
    [SerializeField] private Joystick joystick = null;



    public CharacterBrain hero { get; private set; }

    public CharacterBrain boss { get; private set; }



    private void Awake()
    {
        hero = SpawnHero();
        //boss = SpawnMonster();

        hero.SetJoystick(joystick);
        hero.SetTargetAttack(boss).Initial(DataManager.HeroData).Show();
        
        //boss.SetTargetAttack(hero).Initial(ConfigManager.GetMonster(boss.type))
        //    .SetPosition(spawnPoint[0].position).SetRotation(spawnPoint[0].rotation).Show();

        topdownCamera.Follow = hero.animatorState.transform;
    }

    private void OnEnable()
    {
        this.AddListener<CharacterBrain>(EventName.OnCharacterDead, OnCharacterDead);
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

    private void OnCharacterDead(CharacterBrain character)
    {
        if (character.CharacterType == CharacterType.Hero)
        {

        }
        else
        {

        }
    }
}
