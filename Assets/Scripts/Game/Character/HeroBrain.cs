using DG.Tweening;
using Lotus.CoreFramework;
using UnityEngine;

public class HeroBrain : CharacterBrain
{
    public override CharacterType CharacterType => CharacterType.Hero;


    private HeroCostumes _heroCostumes = null;
    public HeroCostumes heroCostumes => this.TryGetComponentInChildren(ref _heroCostumes);

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


    private float blendSpeed = 0f;

    public Joystick joyStick { get; private set; }



    protected override void Initialized(CharacterConfig data)
    {
        base.Initialized(data);
        heroCostumes.Initialized();
    }

    protected override void InitEvents()
    {
        base.InitEvents();
        this.AddListener<ItemType, int>(EventName.ChangeCostume, ChangeCostume);
    }

    public override void SetJoystick(Joystick joystick) => this.joyStick = joystick;


    protected override string GetProjectileName(AttackType type) => $"Hero_Projectile_{DataManager.HeroData.items[ItemType.Bow].itemLevel}";

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
        if (currentScene != SceneName.Boss)
            return;

        if (joyStick.Direction != Vector2.zero)
            characterMovement.MoveToDirection(GetWorldSpaceDirection(joyStick.Direction));

        blendSpeed = Mathf.Lerp(blendSpeed, joyStick.Direction == Vector2.zero ? 0 : 1, 5f * Time.deltaTime);
        animatorState.Ator.SetFloat("Speed", blendSpeed);
        characterMovement.SetMoveSpeed(blendSpeed);
    }

    public override void TakedDamage(float damage, CharacterBrain sender)
    {
        base.TakedDamage(damage, sender);

        if (characterStats.Alive)
            animatorState.ChangeState(AnimationStates.TakeDamage);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        Movement();
    }

    protected override void OnDead()
    {
        
    }
}
