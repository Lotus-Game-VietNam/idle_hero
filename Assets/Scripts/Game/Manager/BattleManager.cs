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
    [SerializeField] private RectTransform mainRect = null;
    [SerializeField] private CanvasGroup manipulationCvgr = null;
    [SerializeField] private UIWin uiWin = null;
    [SerializeField] private Joystick joystick = null;
    [SerializeField] private UI_SkillButton[] skillButtons = null;



    public CharacterBrain hero { get; private set; }

    public CharacterBrain boss { get; private set; }



    private void Awake()
    {
        hero = SpawnHero();
        boss = SpawnMonster();

        hero.SetJoystick(joystick);
        hero.SetSkillButtons(skillButtons);
        hero.SetTargetAttack(boss).Initial(DataManager.HeroData).Show();

        boss.SetTargetAttack(hero).Initial(ConfigManager.GetMonster(boss.type))
            .SetPosition(spawnPoint[1].position).SetRotation(spawnPoint[1].rotation).Show();

        topdownCamera.Follow = hero.animatorState.transform;
        ComponentReference.MainRect = () => mainRect;
    }

    private void OnEnable()
    {
        this.AddListener<CharacterBrain>(EventName.OnCharacterDead, OnCharacterDead);
        this.AddListener(EventName.OnWin, OnWin);
    }

    private void OnDisable()
    {
        this.RemoveListener(EventName.OnCharacterDead);
        this.RemoveListener(EventName.OnWin);
    }


    private CharacterBrain SpawnHero()
    {
        GameObject heroObj = heroAsset.InstantiateAsync(spawnPoint[0].position, spawnPoint[0].rotation, null).WaitForCompletion();
        return heroObj.GetComponent<CharacterBrain>();
    }

    private CharacterBrain SpawnMonster()
    {
        string monsterName = $"Boss_{DataManager.WorldData.currentLevel}";
        return this.DequeueCharacter(monsterName);
    }

    private void OnCharacterDead(CharacterBrain character)
    {
        if (character.CharacterType == CharacterType.Hero)
        {

        }
        else
        {
            this.DelayCall(2f, () => { this.SendMessage(EventName.OnWin); });
        }
    }

    private void OnWin()
    {
        manipulationCvgr.DeActive();
        uiWin.UpdateContent();
    }


    [Title("Debugger")]
    [Button("Win Now")]
    public void WinNow()
    {
        boss.TakedDamage(99999999, hero);
    }
}
