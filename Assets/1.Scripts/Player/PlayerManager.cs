using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerManager;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    [Header("Data")]
    [SerializeField] PlayerData playerData;
    public PlayerData Data { get { return playerData; } }

    [SerializeField] FollowCamera followCamera;

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

    // Start is called before the first frame update
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
    }

    void KeyAction()
    {
        if (isChange || isUnChange) return;
        if (changeType == CHANGETYPE.Normal) return;
        if (Input.GetKeyDown(KeyCode.S))
        {
            UnChange(-transform.forward);
        }
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
        yield return new WaitForSeconds(0.5f);
        //파티클
        changeEffect.Play();
        yield return new WaitForSeconds(0.5f);
        ChangeEndEffect();
        yield return new WaitForSeconds(0.5f);
        ChangeEnd();
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
    public void UnChange(Vector3 bubbleDir)
    {
        if (isUnChange) return;
        if (changeType == CHANGETYPE.Normal) return;

        isUnChange = true;
        StartCoroutine(UnChangeCoroutine(bubbleDir));
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            Item item = collision.gameObject.GetComponent<Item>();
            if (item != null)
            {
                item.GetItem();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CameraBasic"))
        {
            followCamera.AngleState = FollowCamera.CameraAngleState.Basic;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CameraBasic"))
        {
            followCamera.AngleState = FollowCamera.CameraAngleState.Basic;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CameraBasic"))
        {
            followCamera.AngleState = FollowCamera.CameraAngleState.Right;
        }
    }

}
