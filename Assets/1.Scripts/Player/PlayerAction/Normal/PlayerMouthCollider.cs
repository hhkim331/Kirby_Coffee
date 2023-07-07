using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouthCollider : MonoBehaviour
{
    [SerializeField] PlayerMouth actionNormal;

    private void OnCollisionEnter(Collision collision)
    {
        //해당 오브젝트가 물건인 경우
        if (collision.gameObject.layer == LayerMask.NameToLayer("MoveableObj"))
        {
            //태그가 버블인 경우
            if (collision.gameObject.CompareTag("Bubble"))
            {
                if (PlayerManager.Instance.ChangeType == PlayerManager.CHANGETYPE.Normal)
                {
                    PlayerManager.Instance.PMouth.IsSuction = false;
                    collision.gameObject.GetComponent<ChangeBubble>().GetItem();
                }
                Destroy(collision.gameObject);
            }
            else
            {
                //오브젝트를 액션에 넘긴다.
                actionNormal.SetStack(collision.gameObject);
            }
        }
    }
}
