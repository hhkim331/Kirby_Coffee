using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_EnemyDestroy : MonoBehaviour
{
    
    public Animator anim;
    public GameObject coin; // 활성화할 게임 오브젝트
    public GameObject particle;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        anim.SetTrigger("Damaged");
        Destroy(this.gameObject, 2);
    }

    private void OnDestroy()
    {
        GameObject co = Instantiate(coin);
        co.transform.position = this.transform.position;
        ItemCoin itemcoin = co.GetComponent<ItemCoin>();
        itemcoin.GetItem();
        GameObject pa = Instantiate(particle);
        pa.transform.position = this.transform.position;
        Destroy(pa, 2);
    }
}
