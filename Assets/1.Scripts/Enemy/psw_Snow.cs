using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_Snow : MonoBehaviour
{
   
    Rigidbody rb;

    public float delayTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(this.gameObject, delayTime);
        var snow = other.gameObject.GetComponent<Rigidbody>();
        if (snow != null)
        {
            snow.AddForce(transform.forward * snow.mass * 5, ForceMode.Impulse);
        }
    }
}
