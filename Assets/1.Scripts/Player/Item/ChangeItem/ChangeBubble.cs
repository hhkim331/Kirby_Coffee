using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBubble : MonoBehaviour
{
    PlayerManager.ChangeType changeType;

    public void GetItem()
    {
        if (changeType != PlayerManager.Instance.changeType)
        {
            //아이템을 먹으면 플레이어 변신
            PlayerManager.Instance.ChangeStart();
            PlayerManager.Instance.Change(changeType);
        }
        Destroy(gameObject);
    }

    //캐릭터가 피격되서 드랍
    public void Set(PlayerManager.ChangeType changeType, Vector3 dropDir)
    {
        this.changeType = changeType;
        Rigidbody rigid = GetComponent<Rigidbody>();
        //드롭방향 + 위쪽방향으로 힘을 준다.
        rigid.AddForce(dropDir * 0.15f + Vector3.up * 0.75f, ForceMode.Impulse);
    }
}
