using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuction : MonoBehaviour
{
    [SerializeField] Transform mouth;   //도착지점

    //콜라이더에 닿은 물건들
    Dictionary<Transform, float> colliderDic = new Dictionary<Transform, float>();

    private void OnEnable()
    {
        //초기화
        colliderDic.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("MoveableObj") && other.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            return;
        //닿은 대상을 목록에 추가시킨다
        colliderDic.Add(other.transform, 0f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("MoveableObj") && other.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            return;

        //이미 목록에 있는 대상이라면
        if (colliderDic.ContainsKey(other.transform))
        {
            //당기는 상대를 일시적으로 버티는 상태로 만든이후
            colliderDic[other.transform] += Time.fixedDeltaTime;
            //일정시간이 지나면
            if (colliderDic[other.transform] >= 0.3f)
            {
                //상대를 목표지점으로 이동시킨다
                other.transform.position = Vector3.MoveTowards(other.transform.position, mouth.position, Time.fixedDeltaTime * PlayerManager.Instance.Data.suctionPower);
            }
            //당기는 상대의 위치를 목표지점으로 이동시킨다.
        }
        //목록에 없는 대상이라면
        else
        {
            colliderDic.Add(other.transform, 0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("MoveableObj") && other.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            return;
        //나간대상을 목록에서 제거한다
        colliderDic.Remove(other.transform);
    }
}
