using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_Enemy_1 : MonoBehaviour
{
    public Transform Player;
    public Animator anim;
   
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>(); // 애니메이터 컴포넌트 취득
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(Player);
        Vector3 v = transform.forward;
        v.y = 0;
        transform.forward = v;
    }

    private void OnTriggerEnter(Collider other)
    {
        
            anim.SetTrigger("damage");
            Destroy(this.gameObject,1);
        
    }
}
