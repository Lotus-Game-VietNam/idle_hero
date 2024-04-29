using Lotus.CoreFramework;
using UnityEngine;

public class UIWin : MonoUI
{
    public Transform[] pointLeftRight = null;
    public Transform selected = null;


    private CanvasGroup _canvasGr = null;
    public CanvasGroup canvasGr => this.TryGetComponent(ref _canvasGr);


    private readonly int[] xRewardsValue = new int[] { 2, 3, 5, 3, 2};

    public Animator selectAnimator { get; private set; }


    protected void Awake()
    {
        selectAnimator = selected.GetComponent<Animator>();
    }

    public void OnClaimXReward()
    {
        selectAnimator.speed = 0;

        float totalDistance = Vector3.Distance(pointLeftRight[0].position, pointLeftRight[1].position);

        float distanceAC = Vector3.Distance(pointLeftRight[0].position, selected.position);

        float ratio = distanceAC / totalDistance;

        int segment = Mathf.FloorToInt(ratio * 5);

        int gemsToAdd = DataManager.WorldData.GetGemsReward() * xRewardsValue[segment];

        CollectionIcons.Instance.Show(10, selected.position);

        this.DelayCall(1, () => 
        {
            ResourceManager.Gem += gemsToAdd; 
            DataManager.WorldData.NextLevel().Save();
        });
    }
}
