using Lotus.CoreFramework;
using UnityEngine;

public class UIWin : MonoUI
{
    public Transform[] pointLeftRight = null;
    public Transform selected = null;
    public Transform nothankButton = null;
    public GameObject vfx = null;


    private CanvasGroup _canvasGr = null;
    public CanvasGroup canvasGr => this.TryGetComponent(ref _canvasGr);


    private readonly int[] xRewardsValue = new int[] { 2, 3, 5, 3, 2};

    public Animator selectAnimator { get; private set; }



    protected void Awake()
    {
        selectAnimator = selected.GetComponent<Animator>();
    }

    public void UpdateContent()
    {
        canvasGr.Active();
        selectAnimator.speed = 1;
        vfx.SetActive(true);
    }

    public void OnClaimXReward()
    {
        selectAnimator.speed = 0;

        float totalDistance = Vector3.Distance(pointLeftRight[0].position, pointLeftRight[1].position);

        float distanceAC = Vector3.Distance(pointLeftRight[0].position, selected.position);

        float ratio = distanceAC / totalDistance;

        int segment = Mathf.FloorToInt(ratio * 5);

        CollectionIcons.Instance.Show(20, selected.position);

        canvasGr.DeActive();

        this.DelayCall(1, () => { OnClaimReward(xRewardsValue[segment]); });
    }

    public void OnClaimNormalReward()
    {
        CollectionIcons.Instance.Show(10, nothankButton.position);
        canvasGr.DeActive();
        this.DelayCall(1, () => { OnClaimReward(1); });
    }

    private void OnClaimReward(int xValue)
    {
        int gemsToAdd = DataManager.WorldData.GetGemsReward() * xValue;

        ResourceManager.Gem += gemsToAdd;
        DataManager.WorldData.NextLevel().Save();

        this.DelayCall(1, () => { this.LoadSceneAsync(Utilities.GetCurrentFarmScene(), 0.5f); });
    }
}
