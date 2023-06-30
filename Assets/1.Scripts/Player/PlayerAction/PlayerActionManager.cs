﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    [SerializeField] PlayerAction actionPistol;

    PlayerAction curAction;

    // Start is called before the first frame update
    public void Set(PlayerManager.ChangeType type)
    {
        //기존 액션 설정해제
        if (curAction != null)
            curAction.Unset();

        switch (type)
        {
            case PlayerManager.ChangeType.Normal:
                curAction = null;
                break;
            case PlayerManager.ChangeType.Pistol:
                curAction = actionPistol;
                break;
        }

        //새 액션 설정
        if (curAction != null)
            curAction.Set();
    }

    public PlayerAction GetCurAction()
    {
        return curAction;
    }

    public void ActionChange()
    {

    }
}
