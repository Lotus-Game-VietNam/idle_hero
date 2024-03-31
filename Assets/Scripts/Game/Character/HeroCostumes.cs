using DG.Tweening;
using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class HeroCostumes : MonoBehaviour
{
    [Title("Object Reference")]
    [SerializeField] private Transform[] costumesParent = null;


    
    public Dictionary<ItemType, GameObject> currentCostumes { get; private set; }


    public Transform hero { get; private set; }


    private void Awake()
    {
        this.AddListener<ItemType, int>(EventName.ChangeCostume, ChangeCostume);

        hero = ComponentReference.HeroTrans.Invoke();
    }


    public void Initialized()
    {
        currentCostumes = new Dictionary<ItemType, GameObject>();

        foreach (var item in DataManager.HeroData.items)
        {
            currentCostumes.Add(item.Key, costumesParent[(int)item.Key].GetChild(item.Value.itemLevel).gameObject);
            currentCostumes[item.Key].SetActive(true);
        }
    }

    private void ChangeCostume(ItemType itemType, int itemLevel)
    {
        if (itemLevel == currentCostumes[itemType].transform.GetSiblingIndex())
            itemLevel++;

        GameObject lastCostume = currentCostumes[itemType];
        currentCostumes[itemType] = costumesParent[(int)itemType].GetChild(itemLevel).gameObject;

        currentCostumes[itemType].SetActive(true);
        lastCostume.SetActive(false);

        this.DequeueEffect("EquipCostume").SetPosition(transform.position + transform.up * 4f).SetRotation(transform.rotation).Show();
        hero.DOPunchScale(Vector3.one * 0.1f, 0.5f).SetEase(Ease.InOutElastic);

        DataManager.HeroData.items[itemType] = ConfigManager.GetItem(itemType, itemLevel);
        DataManager.HeroData.Save();
    }
}
