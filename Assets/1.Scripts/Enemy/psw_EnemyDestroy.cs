using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_EnemyDestroy : MonoBehaviour
{
    
    public Animator anim;
    
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
}
