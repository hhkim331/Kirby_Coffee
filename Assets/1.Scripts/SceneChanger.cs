using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;

    [SerializeField] Image starHoleImage;
    [SerializeField] Image rotStarImage;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //IEnumerator Start()
    //{
    //    yield return new WaitForSeconds(1f);
    //    yield return StartCoroutine(ChangeSceneStart(""));

    //    yield return new WaitForSeconds(5f);
    //    ChangeSceneEnd();
    //}

    //씬 전환
    IEnumerator ChangeSceneStart(string sceneName)
    {
        starHoleImage.gameObject.SetActive(true);
        starHoleImage.rectTransform.DOSizeDelta(new Vector2(150, 150), 1f).From(new Vector2(3600, 3600)).SetEase(Ease.OutSine);
        rotStarImage.rectTransform.DOSizeDelta(new Vector2(150, 150), 1f).From(new Vector2(4500, 4500)).SetEase(Ease.Linear);
        rotStarImage.transform.DORotate(Vector2.zero, 1f).From(new Vector3(0, 0, 144)).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1.2f);
        starHoleImage.rectTransform.sizeDelta = Vector2.zero;
        rotStarImage.rectTransform.sizeDelta = Vector2.zero;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    //씬 전환
    public void ChangeSceneEnd()
    {
        starHoleImage.rectTransform.DOSizeDelta(new Vector2(3600, 3600), 0.8f).From(Vector2.zero).SetDelay(0.2f).SetEase(Ease.InSine);
        rotStarImage.rectTransform.DOSizeDelta(new Vector2(4500, 4500), 1f).From(Vector2.zero).SetEase(Ease.Linear);
        rotStarImage.transform.DORotate(new Vector3(0, 0, -72), 1f).From(Vector3.zero).SetEase(Ease.OutQuint).OnComplete(() => { starHoleImage.gameObject.SetActive(false); });
    }
}
