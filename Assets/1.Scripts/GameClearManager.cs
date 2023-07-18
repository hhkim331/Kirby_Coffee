using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameClearManager : MonoBehaviour
{
    public static GameClearManager Instance;

    [SerializeField] Image playerBossKillImage;

    [SerializeField] Image myBossKillImage;
    [SerializeField] RawImage myBossKillVideo;
    [SerializeField] VideoPlayer videoPlayer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        myBossKillVideo.enabled = false;
        videoPlayer.Stop();
    }

    public IEnumerator GameClear()
    {
        Time.timeScale = 0;

        playerBossKillImage.gameObject.SetActive(true);
        playerBossKillImage.color = new Color(1, 1, 1, 0.8f);
        playerBossKillImage.DOFade(0, 0.6f).SetUpdate(true).SetEase(Ease.InCubic);
        PlayerManager.Instance.FCamera.CameraShake(0.3f, 0.15f);
        yield return new WaitForSecondsRealtime(0.1f);
        PlayerManager.Instance.FCamera.CameraShake(0.2f, 0.15f);
        yield return new WaitForSecondsRealtime(0.1f);
        PlayerManager.Instance.FCamera.CameraShake(0.1f, 0.15f);
        yield return new WaitForSecondsRealtime(0.55f);
        myBossKillVideo.enabled = true;
        videoPlayer.Play();
    }
}