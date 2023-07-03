using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyTest : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //적의 반대를 향하는 벡터
            Vector3 dir = collision.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            PlayerManager.Instance.Hit(dir, 1, true);
        }
    }
}
