using DG.Tweening;
using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_SkillButton : MonoUI
{
    [Title("Configuration")]
    public int skillIndex = 0;
    public float timesCD = 5f;

    [Title("Object Reference")]
    public Image dimed = null;


    public bool isCountingDown { get; private set; }



    private Button _button = null;
    public Button button => this.TryGetComponent(ref _button);


    public static UnityEvent<int> OnTriggerSkillEvent = new UnityEvent<int>();


    private void Awake()
    {
        OnTriggerSkillEvent.AddListener(OnTriggerSkill);
        button.onClick.AddListener(OnCountingDown);
        dimed.enabled = false;
    }


    private void OnCountingDown()
    {
        if (isCountingDown)
            return;

        this.SendMessage(EventName.OnTriggerSkill, "HeroBrain", skillIndex);
    }

    private void OnTriggerSkill(int skillIndex)
    {
        if (this.skillIndex != skillIndex)
            return;

        isCountingDown = true;
        dimed.fillAmount = 1;
        dimed.enabled = true;

        dimed.DOFillAmount(0, timesCD).OnComplete(() =>
        {
            dimed.enabled = false;
            isCountingDown = false;
        });
    }
}
