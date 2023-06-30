using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouthCollider : MonoBehaviour
{
    [SerializeField]PlayerMouth actionNormal;

    private void OnCollisionEnter(Collision collision)
    {
        //해당 오브젝트가 물건인 경우
        if(collision.gameObject.layer == LayerMask.NameToLayer("MoveableObj"))
        {
            //오브젝트를 액션에 넘긴다.
            actionNormal.SetStack(collision.gameObject);
        }

        //해당 오브젝트가 적인 경우
        //죽이고 해당 데이터만 넘긴다.
    }
}
