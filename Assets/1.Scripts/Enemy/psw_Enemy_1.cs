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
    // Start is called before the first frame update
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
    }

    private void OnTriggerEnter(Collider other)
    {
        
            anim.SetTrigger("damage");
            Destroy(this.gameObject,1);
        
    }
    private void OnDestroy()
    {
        if (coin != null)
        {
            coin.SetActive(true); // 게임 오브젝트를 활성화
        }
    }
}
