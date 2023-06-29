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

    PlayerMovement playerMove;
    PlayerActionManager playerActionManager;


    private void Awake()
    {
        Instance = this;
        changeType = ChangeType.Normal;
        playerMove = GetComponent<PlayerMovement>();
        playerActionManager = GetComponent<PlayerActionManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMove.Set(this, playerData);
        GameManager.Input.keyaction += playerMove.keyMove;

        playerActionManager.Set(changeType);
        GameManager.Input.keyaction += playerActionManager.GetCurAction().KeyAction;
    }

    //변신하면 호출되는 함수(나중에 사용)
    public void Change(ChangeType type)
    {
        if (type == changeType) return;
        changeType = type;

        //기존 액션 해제
        GameManager.Input.keyaction -= playerActionManager.GetCurAction().KeyAction;
        //액션정보 변경
        playerActionManager.Set(changeType);
        //새 액션 설정
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Item"))
        {
            ChangeItem item = hit.gameObject.GetComponent<ChangeItem>();
            if (item != null)
            {
                item.GetItem();
            }
        }
    }
}
