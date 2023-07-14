using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float lifeTime = 3f;
    bool isGuide = false;   //유도
    Vector3 moveDir;
    Collider targetCollider;

    //이펙트
    [SerializeField] GameObject hitEffect;

    public void Set(Vector3 dir)
    {
        isGuide = false;
        moveDir = dir;
    }

    public void Set(Collider target)
    {
        isGuide = true;
        targetCollider = target;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGuide && targetCollider != null)
        {
            moveDir = targetCollider.bounds.center - transform.position;
            transform.position += moveDir.normalized * Time.deltaTime * bulletSpeed;
        }
        else
        {
            //정해진 방향으로 이동
            transform.position += moveDir * Time.deltaTime * bulletSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Boss") || other.gameObject.layer == LayerMask.NameToLayer("Hammer"))
        {
            other.transform.root.GetComponent<SSB_Boss1>().DamageProcess();
            Destroy(Instantiate(hitEffect, transform.position, Quaternion.identity), 3);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(Instantiate(hitEffect, transform.position, Quaternion.identity), 3);
        }


        if (!other.CompareTag("CameraBasic"))
            Destroy(gameObject);
    }
}
