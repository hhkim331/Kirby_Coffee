using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_starrrrr : MonoBehaviour
{
    float currentTime;
    public float speed = 5.0f; // 원하는 총알 속도를 설정합니다.
    public float distanceTime = 1f;
    Rigidbody rb;
    public GameObject particle;
    public bool isJump = false;
    public static psw_starrrrr instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        starMove();
    }

    // Update is called once per frame
    void Update()
    {
        starMove();
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            //적의 반대를 향하는 벡터
            Vector3 dir = transform.position - other.transform.position;
            dir.y = 0;
            dir.Normalize();
            PlayerManager.Instance.PHealth.Hit(dir, 1, true);
        }

        Destroy(gameObject, 3);
    }

    private void OnDestroy()
    {
        GameObject pa = Instantiate(particle);
        pa.transform.position = this.transform.position;
        Destroy(pa, 1);
    }

    public void starMove()
    {
        if (psw_animEvent.instance.isJump == true)
        {
            Vector3 dir = transform.forward + transform.up;
            dir.Normalize();
            rb.AddForce(dir * speed, ForceMode.Impulse);
        }
    }
}
