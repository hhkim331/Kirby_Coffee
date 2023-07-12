using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    [Header("Data")]
    [SerializeField] PlayerData playerData;
    public PlayerData Data { get { return playerData; } }

    [SerializeField] FollowCamera followCamera;
    public FollowCamera FCamera { get { return followCamera; } }

    [SerializeField] Animator anim;
    public Animator Anim { get { return anim; } }

    //피격상태
    bool isHit = false;
    public bool IsHit { get { return isHit; } }
    float hitTime; //조작불가능시간,밀려나는 시간

    //변신
    public enum CHANGETYPE
    {
        Normal,
        Pistol,
    }
    CHANGETYPE changeType;
    public CHANGETYPE ChangeType { get { return changeType; } }
    bool isChange = false;
    public bool IsChange { get { return isChange; } }
    bool isUnChange = false;
    public bool IsUnChange { get { return isUnChange; } }
    [SerializeField] GameObject[] changeBubbles;
    [SerializeField] ParticleSystem changeEffect;

    //캐릭터 컴포넌트
    PlayerMovement playerMovement;
    public PlayerMovement PMovement { get { return playerMovement; } }
    PlayerHealth playerHealth;
    public PlayerHealth PHealth { get { return playerHealth; } }
    PlayerCoin playerCoin;
    public PlayerCoin PCoin { get { return playerCoin; } }
    PlayerActionManager playerActionManager;
    public PlayerActionManager PActionManager { get { return playerActionManager; } }
    [SerializeField] PlayerMouth playerMouth;
    public PlayerMouth PMouth { get { return playerMouth; } }

    private void Awake()
    {
        Instance = this;
        changeType = CHANGETYPE.Normal;
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        playerCoin = GetComponent<PlayerCoin>();
        playerActionManager = GetComponent<PlayerActionManager>();
    }

    void Start()
    {
        GameManager.Input.keyaction += KeyAction;

        playerMovement.Set(playerData);
        GameManager.Input.keyaction += playerMovement.keyMove;

        playerHealth.Set(playerData);

        playerMouth.Set(changeType);
        GameManager.Input.keyaction += playerMouth.KeyAction;

        playerActionManager.Set(changeType);
        if (changeType != CHANGETYPE.Normal)
            GameManager.Input.keyaction += playerActionManager.GetCurAction().KeyAction;

        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update()
    {
        if (isHit)
        {
            hitTime -= Time.deltaTime;
            if (hitTime <= 0)
                isHit = false;
        }
    }

    void KeyAction()
    {
        if (isChange || isUnChange) return;
        if (changeType == CHANGETYPE.Normal) return;
        if (Input.GetKeyDown(KeyCode.S))
        {
            UnChange(-transform.forward, false);
        }
    }

    public void Hit()
    {
        isHit = true;
        hitTime = playerData.hitTime;
    }

    #region 변신
    //변신하면 호출되는 함수(나중에 사용)
    public void Change(CHANGETYPE type)
    {
        if (type == changeType) return;

        //기존 액션 해제
        if (changeType != CHANGETYPE.Normal)
            GameManager.Input.keyaction -= playerActionManager.GetCurAction().KeyAction;
        //액션정보 변경
        changeType = type;
        playerMouth.Set(changeType);
        playerActionManager.Set(changeType);
        //새 액션 설정
        if (changeType != CHANGETYPE.Normal)
        {
            GameManager.Input.keyaction += playerActionManager.GetCurAction().KeyAction;
            //변신 애니메이션
            StartCoroutine(ChangeCoroutine());
        }
    }

    IEnumerator ChangeCoroutine()
    {
        Time.timeScale = 0;
        anim.SetTrigger("ChangeStart");
        yield return new WaitForSecondsRealtime(0.75f);
        //파티클
        changeEffect.Play();
        yield return new WaitForSecondsRealtime(0.75f);
        ChangeEndEffect();
        anim.SetTrigger("ChangeEnd");
        yield return new WaitForSecondsRealtime(0.5f);
        ChangeEnd();
        Time.timeScale = 1;
    }

    public void ChangeStart()
    {
        isChange = true;
        //카메라 줌인
        followCamera.DistanceState = FollowCamera.CameraDistanceState.Zoomin;
        //카메라의 방향으로 부드럽게 회전한다.

        GameManager.Instance.PlayerChangeStart();
    }

    public void ChangeEndEffect()
    {
        followCamera.DistanceState = FollowCamera.CameraDistanceState.Basic;
        GameManager.Instance.PlayerChangeEnd();
    }

    public void ChangeEnd()
    {
        isChange = false;
    }

    //변신해제
    public void UnChange(Vector3 bubbleDir, bool isHit)
    {
        if (isUnChange) return;
        if (changeType == CHANGETYPE.Normal) return;

        if (isHit)
        {
            ChangeBubble bubble = Instantiate(changeBubbles[(int)changeType - 1], transform.position + Vector3.up, Quaternion.identity).GetComponent<ChangeBubble>();
            bubble.Set(changeType, bubbleDir);
            //기존 액션 해제
            Change(CHANGETYPE.Normal);
        }
        else
        {
            isUnChange = true;
            StartCoroutine(UnChangeCoroutine(bubbleDir));
        }
    }

    IEnumerator UnChangeCoroutine(Vector3 bubbleDir)
    {
        yield return new WaitForSeconds(0.2f);
        //버블 생성
        ChangeBubble bubble = Instantiate(changeBubbles[(int)changeType - 1], transform.position + Vector3.up, Quaternion.identity).GetComponent<ChangeBubble>();
        bubble.Set(changeType, bubbleDir);
        //기존 액션 해제
        Change(CHANGETYPE.Normal);
        changeEffect.Play();
        yield return new WaitForSeconds(0.1f);
        isUnChange = false;
    }
    #endregion
}
