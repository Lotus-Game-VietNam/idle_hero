using DG.Tweening;
using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using UnityEngine;
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



    private void Awake()
    {
        button.onClick.AddListener(OnCountingDown);
        dimed.enabled = false;
    }


    private void OnCountingDown()
    {
        if (isCountingDown)
            return;

        this.SendMessage(EventName.OnTriggerSkill, "HeroBrain", skillIndex);

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
