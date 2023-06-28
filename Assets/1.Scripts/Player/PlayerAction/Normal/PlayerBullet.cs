using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    //발사 방향
    Vector3 direction;
    Vector3 bulletRotate;

    public void Set(Vector3 direction)
    {
        this.direction = direction;
        //기존 콜라이더 비활성화
        GetComponent<Collider>().enabled = false;
        //새로운 원콜라이더 추가
        this.AddComponent<SphereCollider>().radius = 0.75f;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        //중력 제거
        rigidbody.useGravity = false;
        //랜덤한 회전 방향 설정
        bulletRotate = new Vector3(1, 1, 1);

        //레이어 변경
        gameObject.layer = LayerMask.NameToLayer("PlayerBullet");
    }
    // Update is called once per frame
    void Update()
    {
        //날아가기
        transform.position += direction * Time.deltaTime * PlayerManager.Instance.Data.spitItemSpeed;
        //랜덤하게 회전하기
        transform.Rotate(bulletRotate * Time.deltaTime * 180f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //레이어가 스테이지 인경우
        if(collision.gameObject.layer == LayerMask.NameToLayer("Stage"))
        {
            //닿은 지점의 법선벡터를 구한다
            Vector3 normal = collision.contacts[0].normal;
            //법선벡터가 위방향을 가리키지 않는 경우
            if(normal.y < 0.9f)
                Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        //충돌한 대상이 적인 경우
        //적에게 데미지를 준다.

        //충돌한 대상이 물건인 경우
        //물건 파괴

    }
}
