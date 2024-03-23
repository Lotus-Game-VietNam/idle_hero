using DG.Tweening;
using Lotus.CoreFramework;
using Sirenix.OdinInspector;
using UnityEngine;

public class CollectionIcons : Singleton<CollectionIcons>
{
    [Title("Configuration")]
    public string iconName;

    [Title("Object Reference")]
    public RectTransform targetIconToMove = null;


    private RectTransform _rect = null;
    public RectTransform rect => this.TryGetComponent(ref _rect);


    private Tween scaleTween = null;

    public void Show(int numberIcon, Vector2 center)
    {
        for (int i = 0; i < numberIcon; i++)
        {
            this.DequeueIcon(iconName, ComponentReference.MainRect.Invoke()).SetParent(rect).SetAnchoredPosition(center).Initial(new IconData(targetIconToMove.ConvertToAnchoredPosition(ComponentReference.MainRect.Invoke()))).
            SetShowFinishEvent(() =>
            {
                scaleTween.Stop();
                targetIconToMove.localScale = Vector3.one;
                scaleTween = targetIconToMove.DOPunchScale(Vector3.one * 0.2f, 0.5f).SetEase(Ease.InOutElastic);
            }).Show();
        }
    }
}
