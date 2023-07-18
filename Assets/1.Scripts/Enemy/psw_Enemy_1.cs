using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_Enemy_1 : MonoBehaviour
{
    public GameObject target;
    public Transform Player;
    public Animator anim;
    public float attackRange = 3;
    public GameObject coin; // 활성화할 게임 오브젝트
    public GameObject particle;
    // Start is called before the first frame update

    bool needDestroy = false;
    float destroyTime = 0f;
    float destroyDelay = 1f;

    void Start()
    {
        anim = GetComponentInChildren<Animator>(); // 애니메이터 컴포넌트 취득
        target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(Player);
        Vector3 v = transform.forward;
        v.y = 0;
        transform.forward = v;
        // 거리 계산
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        if (distance <= attackRange)
        {
            anim.SetTrigger("Find");
        }

        if (needDestroy)
            destroyTime += Time.deltaTime;

        if (destroyTime > destroyDelay)
        {
            needDestroy = false;

            GameObject co = Instantiate(coin);
            co.transform.position = this.transform.position;
            ItemCoin itemcoin = co.GetComponent<ItemCoin>();
            itemcoin.GetItem();

            GameObject pa = Instantiate(particle);
            pa.transform.position = this.transform.position;
            Destroy(pa, 2);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        anim.SetTrigger("damage");
        destroyTime = 0;
        needDestroy = true; 
    }

    //private void OnDestroy()
    //{
    //    // coin 공장에서 coin 을 생성하자
    //    GameObject co = Instantiate(coin);
    //    // 생성된 coin을 나의 위치에 배치하자
    //    co.transform.position = this.transform.position;
    //    // 생성된 coin 에서 ItemCoin 콤포넌트를 가져오자
    //    ItemCoin itemcoin = co.GetComponent<ItemCoin>();
    //    // 가져온 컴포넌트에  GetItem 함수를 실행하자.
    //    itemcoin.GetItem();
    //    GameObject pa = Instantiate(particle);
    //    pa.transform.position = this.transform.position;
    //    Destroy(pa, 2);
    //    //particle.Play();
    //    //print("되고 있는거니");
    //}
}
