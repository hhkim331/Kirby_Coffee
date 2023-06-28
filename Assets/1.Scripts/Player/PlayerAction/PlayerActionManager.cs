using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    [SerializeField] PlayerAction normalAction;

    PlayerAction curAction;

    // Start is called before the first frame update
    public void Set(PlayerManager.PlayerType type)
    {
        //기존 액션 설정해제
        if (curAction != null)
            curAction.Unset();

        switch (type)
        {
            case PlayerManager.PlayerType.Normal:
                curAction = normalAction;
                break;
            case PlayerManager.PlayerType.Pistol:
                break;
        }
        
        //새 액션 설정
        curAction.Set();
    }

    public PlayerAction GetCurAction()
    {
        return curAction;
    }
}
