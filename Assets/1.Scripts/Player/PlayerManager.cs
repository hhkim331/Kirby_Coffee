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

    public enum PlayerType
    {
        Normal,
        Pistol,
    }
    public PlayerType playerType;

    PlayerMovement playerMove;
    PlayerActionManager playerActionManager;


    private void Awake()
    {
        Instance = this;
        playerType = PlayerType.Normal;
        playerMove = GetComponent<PlayerMovement>();
        playerActionManager = GetComponent<PlayerActionManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMove.Set(this, playerData);
        GameManager.Input.keyaction += playerMove.keyMove;

        playerActionManager.Set(playerType);
        GameManager.Input.keyaction += playerActionManager.GetCurAction().KeyAction;
    }

    //변신하면 호출되는 함수(나중에 사용)
    public void ChangePlayer(PlayerType type)
    {
        if (type == playerType) return;
        playerType = type;

        //기존 액션 해제
        GameManager.Input.keyaction -= playerActionManager.GetCurAction().KeyAction;
        //액션정보 변경
        playerActionManager.Set(playerType);
        //새 액션 설정
        GameManager.Input.keyaction += playerActionManager.GetCurAction().KeyAction;
    }
}
