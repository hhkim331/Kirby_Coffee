using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // 플레이어를 가져온다.
    public GameObject player;
    // 체크 포인트를 가져온다.
    public GameObject checkPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player.transform.position = checkPoint.transform.position;
        }
    }
}
