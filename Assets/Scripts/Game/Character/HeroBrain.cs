using Cinemachine;
using DG.Tweening;
using Lotus.CoreFramework;
using System.Linq;
using UnityEngine;

public class HeroBrain : CharacterBrain
{
    public override CharacterType CharacterType => CharacterType.Hero;


    private HeroCostumes _heroCostumes = null;
    public HeroCostumes heroCostumes => this.TryGetComponentInChildren(ref _heroCostumes);

    private CinemachineVirtualCamera winCamera = null;

    private Transform _mainCamera = null;
    public Transform mainCamera
    {
        get
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main.transform;
            return _mainCamera;
        }
    }


    

    public Joystick joyStick { get; private set; }

    public UI_SkillButton[] skillButtons { get; private set; }


    private readonly float timeDelayNormalAttackToSkill = 0.5f;

    private readonly float timeOnSkill = 1f;

    private readonly float[] skillsDamage = new float[3] { 2.5f, 1, 2f };

    private float countTimeDelayNormalAttack = 0f;

    private float countTimeOnSkill = 0f;

    private bool onSkill = false;



    protected override void Awake()
    {
        base.Awake();
        winCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }


    protected override void Initialized(CharacterConfig data)
    {
        base.Initialized(data);
        heroCostumes.Initialized();
    }

    protected override void SetStarterValues()
    {
        base.SetStarterValues();
        countTimeDelayNormalAttack = 0f;
        countTimeOnSkill = 0f;
        onSkill = false;
    }

    protected override void InitEvents()
    {
        base.InitEvents();
        this.AddListener<ItemType, int>(EventName.ChangeCostume, ChangeCostume);
        this.AddListener<int>(EventName.OnTriggerSkill, OnTriggerSkill);
        this.AddListener(EventName.OnWin, OnWin);
        this.AddListener(EventName.OnRevive, OnRevive);
    }

    public override void SetJoystick(Joystick joystick) => this.joyStick = joystick;

    public override void SetSkillButtons(UI_SkillButton[] skillButtons) => this.skillButtons = skillButtons;


    protected override string GetProjectileName(AttackType type) => type == AttackType.NormalAttack ? $"Hero_Projectile_{DataManager.HeroData.items[ItemType.Bow].itemLevel}" : $"Hero_Projectile_Skill_{(int)type}";

    protected override float GetFinalDamage(int attackType) => attackType == 0 ? base.GetFinalDamage(attackType) : characterStats.ATK * skillsDamage[attackType - 1];

    private void ChangeCostume(ItemType itemType, int itemLevel)
    {
        transform.DOPunchScale(Vector3.one * 0.1f, 0.5f).SetEase(Ease.InOutElastic);
    }

    protected override void OnShotFinish()
    {
        base.OnShotFinish();
        if (targetAttack == null || !targetAttack.characterStats.Alive)
            this.SendMessage(EventName.RefreshMonsterTarget, "FarmManager");
    }

    protected override void OnFarm()
    {
        Shot(AttackType.NormalAttack);
    }

    private Vector3 GetWorldSpaceDirection(Vector2 joystickDirection)
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraForwardPlane = Vector3.ProjectOnPlane(cameraForward, Vector3.up).normalized;
        Vector3 worldSpaceDirection = (cameraForwardPlane * joystickDirection.y + mainCamera.transform.right * joystickDirection.x).normalized;
        return worldSpaceDirection;
    }

    private void Movement()
    {
        if (!currentScene.IsOnBattle() || animatorState.currentState.IsAttack() || onSkill)
            return;

        if (joyStick.Direction != Vector2.zero)
            characterMovement.MoveToDirection(GetWorldSpaceDirection(joyStick.Direction));

        SetBlendSpeed(joyStick.Direction == Vector2.zero ? 0 : 1);
    }

    private void NormalAttack()
    {
        if (!currentScene.IsOnBattle())
            return;

        if (onSkill || joyStick.Direction != Vector2.zero || animatorState.currentState.IsAttack())
        {
            countTimeDelayNormalAttack = 0f;
            return;
        }

        if (skillButtons.Any(b => !b.isCountingDown))
        {
            countTimeDelayNormalAttack += Time.deltaTime;
            if (countTimeDelayNormalAttack >= timeDelayNormalAttackToSkill)
                countTimeDelayNormalAttack = 0f;
        }

        if (countTimeDelayNormalAttack == 0)
            Shot(AttackType.NormalAttack);
    }

    public override void TakedDamage(float damage, CharacterBrain sender)
    {
        base.TakedDamage(damage, sender);

        if (characterStats.Alive)
            animatorState.ChangeState(AnimationStates.TakeDamage);
    }

    private void OnTriggerSkill(int skillIndex)
    {
        if (animatorState.currentState == AnimationStates.TakeDamage)
            return;

        onSkill = true;
        animatorState.ChangeState(AnimationStates.Idle);
        characterMovement.StopRotateCrt();
        Shot((AttackType)skillIndex);
        UI_SkillButton.OnTriggerSkillEvent?.Invoke(skillIndex);
    }

    private void OnSkill()
    {
        if (!onSkill)
            return;

        countTimeOnSkill += Time.deltaTime;
        if (countTimeOnSkill >= timeOnSkill)
        {
            countTimeOnSkill = 0f;
            onSkill = false;
        }
    }

    protected override void OnShot(int type)
    {
        base.OnShot(type);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        Movement();
        NormalAttack();
        OnSkill();
    }

    private void OnWin()
    {
        winCamera.Priority = 100;
    }

    protected override void OnTargetDead()
    {
        //if (!currentScene.IsOnBattle())
        //    return;

        //animatorState.ChangeState(AnimationStates.Cheer);

        if (currentScene.IsOnFarm())
            Shot(AttackType.NormalAttack);
    }

    private void OnRevive()
    {
        characterStats.Revive();
        animatorState.Ator.Rebind();
        animatorState.ChangeState(AnimationStates.Idle);
        characterMovement.ActiveAgent(true);
        characterAttack.ActiveCollider(true);
    }    

    protected override void OnDead()
    {
        
    }
}
