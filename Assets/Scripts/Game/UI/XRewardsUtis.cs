using Lotus.CoreFramework;
using TMPro;
using UnityEngine;

public class XRewardsUtis : MonoBehaviour
{
    public TMP_Text gemsValueTxt = null;


    private void Awake()
    {
        SetGemsValue(2);
    }

    public void SetGemsValue(int xValue)
    {
        gemsValueTxt.text = $"CLAIM {DataManager.WorldData.GetGemsReward() * xValue}";
    }
}
