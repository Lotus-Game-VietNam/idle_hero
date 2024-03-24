using DG.Tweening;
using UnityEngine;

public class GemIcon : IconSprite
{
    public float jumpPower = 100f;
    public float randomLenght = 200f;
    public float jumpDuration = 0.5f;
    public float moveDuration = 1f;


    protected override void OnShow()
    {
        base.OnShow();
        Vector2 randomPoint = rect.anchoredPosition + (new Vector2(Random.Range(-1f, 1), Random.Range(-1f, 1)) * randomLenght);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(rect.DOJumpAnchorPos(randomPoint, jumpPower, 1, jumpDuration + Random.Range(-jumpDuration / 2, jumpDuration / 2)));
        sequence.Append(transform.DOMove(data.point, moveDuration).SetEase(Ease.InBack).OnComplete(() => { ShowFinishEvent?.Invoke(); }));
        sequence.Play();
    }
}
