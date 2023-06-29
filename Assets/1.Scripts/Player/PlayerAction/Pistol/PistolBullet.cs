using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float lifeTime = 3f;
    Vector3 moveDir;

    public void Set(Vector3 dir)
    {
        moveDir = dir;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        //정해진 방향으로 이동
        transform.position += moveDir * Time.deltaTime * bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
