using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCoin : MonoBehaviour
{
    //코인
    int coin;
    public int Coin { get { return coin; } }

    [SerializeField] CanvasGroup coinInfoCG;
    [SerializeField] Image coinImage;
    [SerializeField] Image coinEffectImage;
    [SerializeField] TextMeshProUGUI coinText;

    //나중에 이펙트 추가
    public void GetCoin(int add)
    {
        coin = coin + add;
        coinImage.material.DOKill();
        coinImage.material.DOColor(Color.black, "_Color", 0.1f).From(Color.white);
        coinEffectImage.DOKill();
        coinEffectImage.DOFade(0, 0.2f).From(1).SetEase(Ease.OutCubic);
        coinEffectImage.transform.DOKill();
        coinEffectImage.transform.DOScale(2f, 0.2f).From(1).SetEase(Ease.OutCubic);

        coinText.text = string.Format("{0:000}", coin);
        coinText.DOKill();
        coinText.DOColor(Color.white, 0.1f).From(new Color(1, 1, 0.5f)).SetEase(Ease.OutCubic);
        coinText.font.material.DOKill();
        coinText.font.material.DOColor(Color.black, "_UnderlayColor", 0.1f).From(new Color(1, 1, 0.5f)).SetEase(Ease.OutCubic);
        coinText.rectTransform.DOKill();
        coinText.rectTransform.DOLocalMoveY(20, 0.1f).From(0).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            coinText.rectTransform.DOLocalMoveY(0, 0.1f).SetEase(Ease.Linear);
        });
    }

    public IEnumerator Die()
    {
        coinInfoCG.transform.DOScale(1.5f, 0.5f).SetEase(Ease.Linear).SetUpdate(true);
        coinInfoCG.transform.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.Linear).SetUpdate(true);
        coinInfoCG.transform.DOLocalMove(new Vector3(-32, 30, 0), 0.5f).SetEase(Ease.Linear).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.5f);
        coinInfoCG.transform.DOScale(1.8f, 0.1f).SetEase(Ease.OutCubic).SetLoops(2, LoopType.Yoyo).SetUpdate(true);

        yield return new WaitForSecondsRealtime(0.5f);

        coinText.rectTransform.DOLocalMoveY(-10, 0.2f).SetLoops(5, LoopType.Restart).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => coinText.rectTransform.localPosition = new Vector3(110, 0, 0));
        for (int i = 0; i < 20; i++)
        {
            coin -= 5;
            coinText.text = string.Format("{0:000}", coin);
            yield return new WaitForSecondsRealtime(0.05f);
        }

        yield return new WaitForSecondsRealtime(0.2f);
        coinInfoCG.DOFade(0, 0.5f).SetEase(Ease.Linear).SetUpdate(true);
        yield return new WaitForSecondsRealtime(1f);

        //coinText.DOKill();
        //coinText.DOColor(Color.white, 0.1f).From(new Color(1, 1, 0.5f)).SetEase(Ease.OutCubic);
        //coinText.rectTransform.DOKill();
    }
}
