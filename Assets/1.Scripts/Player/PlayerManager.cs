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

    public enum ChangeType
    {
        Normal,
        Pistol,
    }
    public ChangeType changeType;
    [SerializeField] GameObject[] changeBubbles;


    PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement { get { return playerMovement; } }
    PlayerActionManager playerActionManager;
    public PlayerActionManager PlayerActionManager { get { return playerActionManager; } }
    [SerializeField] PlayerMouth playerMouth;
    public PlayerMouth PlayerMouth { get { return playerMouth; } }

    //현재 플레이어 정보
    float health;
    float maxHealth;
    //무적
    float hitDelay;
    float hitBlinkTime;
    [SerializeField] Renderer[] renderers;
    [SerializeField] Material myMaterial;
    Color curColor = Color.black;
    Color blinkColor = new Color(0.5f, 0.5f, 0, 1);

    private void Awake()
    {
        Instance = this;
        changeType = ChangeType.Normal;
        playerMovement = GetComponent<PlayerMovement>();
        playerActionManager = GetComponent<PlayerActionManager>();
        maxHealth = playerData.health;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Input.keyaction += KeyAction;

        playerMovement.Set(playerData);
        GameManager.Input.keyaction += playerMovement.keyMove;

        playerMouth.Set(changeType);
        GameManager.Input.keyaction += playerMouth.KeyAction;

        playerActionManager.Set(changeType);
        if (changeType != ChangeType.Normal)
            GameManager.Input.keyaction += playerActionManager.GetCurAction().KeyAction;
    }

    void Update()
    {
        //무적시간
        if (hitDelay > 0f)
        {
            hitDelay -= Time.deltaTime;
            hitBlinkTime += Time.deltaTime;
            if (hitDelay < 0f)
            {
                hitDelay = 0f;
                //emission으로 발광효과주기
                //foreach (var renderer in renderers)
                curColor= Color.black;
                myMaterial.SetColor("_EmissionColor", Color.black);
            }
            else if (hitBlinkTime > playerData.hitBlinkDelay)
            {
                hitBlinkTime = 0f;
                //foreach (var renderer in renderers)
                {
                    if (curColor == Color.black)
                    {
                        curColor = blinkColor;
                        myMaterial.SetColor("_EmissionColor", blinkColor);

                    }
                    else
                    {
                        curColor = Color.black;
                        myMaterial.SetColor("_EmissionColor", Color.black);
                    }
                }
            }
        }
    }

    void KeyAction()
    {
        if (changeType == ChangeType.Normal) return;
        if (Input.GetKeyDown(KeyCode.S))
        {
            UnChange(-transform.forward);
        }
    }

    public void Hit(Vector3 hitDir, float damage, bool drop)
    {
        if (hitDelay > 0f) return; //무적상태
        hitDelay = playerData.hitDelay; //무적시간 설정
        playerData.health -= damage;
        if (playerData.health <= 0f)
            Die();
        else
        {
            //피격 애니메이션 실행
            playerMovement.Hit(hitDir);
            //아이템 드롭
            if (drop)
            {
                UnChange(hitDir);
            }
        }
    }

    void Die()
    {

    }

    #region 변신
    //변신하면 호출되는 함수(나중에 사용)
    public void Change(ChangeType type)
    {
        if (type == changeType) return;

        //기존 액션 해제
        if (changeType != ChangeType.Normal)
            GameManager.Input.keyaction -= playerActionManager.GetCurAction().KeyAction;
        //액션정보 변경
        changeType = type;
        playerMouth.Set(changeType);
        playerActionManager.Set(changeType);
        //새 액션 설정
        if (changeType != ChangeType.Normal)
            GameManager.Input.keyaction += playerActionManager.GetCurAction().KeyAction;

        //변신 애니메이션
        StartCoroutine(ChangeCoroutine());
    }

    IEnumerator ChangeCoroutine()
    {
        yield return null;
        ChangeEnd();
    }

    public void ChangeStart()
    {
        //카메라 줌인
        //카메라의 방향으로 부드럽게 회전한다.
    }

    public void ChangeEnd()
    {
        //카메라 줌아웃
    }

    //변신해제
    void UnChange(Vector3 bubbleDir)
    {
        if (changeType == ChangeType.Normal) return;

        //버블 생성
        ChangeBubble bubble = Instantiate(changeBubbles[(int)changeType - 1], transform.position + Vector3.up, Quaternion.identity).GetComponent<ChangeBubble>();
        bubble.Set(changeType, bubbleDir);

        //기존 액션 해제
        Change(ChangeType.Normal);
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            ChangeItem item = collision.gameObject.GetComponent<ChangeItem>();
            if (item != null)
            {
                item.GetItem();
            }
        }
    }
}
