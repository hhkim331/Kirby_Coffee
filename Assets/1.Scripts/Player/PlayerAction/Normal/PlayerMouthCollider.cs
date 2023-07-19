using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouthCollider : MonoBehaviour
{
    [SerializeField] PlayerMouth actionNormal;

    private void OnCollisionEnter(Collision collision)
    {
        ////해당 오브젝트가 물건인 경우
        //if (collision.gameObject.layer == LayerMask.NameToLayer("MoveableObj"))
        //{
        //    //태그가 버블인 경우
        //    if (collision.gameObject.CompareTag("Bubble"))
        //    {
        //        if (PlayerManager.Instance.ChangeType == PlayerManager.CHANGETYPE.Normal)
        //        {
        //            PlayerManager.Instance.PMouth.IsSuction = false;
        //            collision.gameObject.GetComponent<ChangeBubble>().GetItem();
        //        }
        //        Destroy(collision.gameObject);
        //    }
        //    else
        //    {
        //        //오브젝트를 액션에 넘긴다.
        //        actionNormal.SetStack(collision.gameObject);
        //    }
        //}
        //else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        //{
        //    psw_Enemy_1 enemy1 = collision.gameObject.GetComponent<psw_Enemy_1>();
        //    psw_EnemyDestroy enemy2 = collision.gameObject.GetComponent<psw_EnemyDestroy>();

        //    if (enemy1 != null)
        //    {
        //        enemy1.isStack = true;
        //        if (enemy1.isChange)
        //        {
        //            PlayerManager.Instance.ChangeStart();
        //            PlayerManager.Instance.Change(PlayerManager.CHANGETYPE.Pistol, true);
        //            Destroy(collision.gameObject);
        //        }
        //        else
        //        {
        //            //오브젝트를 액션에 넘긴다.
        //            actionNormal.SetStack(collision.gameObject);
        //        }
        //    }
        //    else if (enemy2 != null)
        //    {
        //        enemy2.isStack = true;
        //        //오브젝트를 액션에 넘긴다.
        //        actionNormal.SetStack(collision.gameObject);
        //    }
        //}
    }
}
