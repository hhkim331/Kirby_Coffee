using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_EnemyDestroy : MonoBehaviour
{

    public Animator anim;
    public GameObject coin; // 활성화할 게임 오브젝트
    public GameObject particle;

    bool needDestroy = false;
    float destroyTime = 0f;
    float destroyDelay = 2f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
        anim.SetTrigger("Damaged");
        destroyTime = 0;
        needDestroy = true;
    }
}
