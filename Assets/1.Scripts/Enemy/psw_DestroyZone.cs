using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_DestroyZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Snow"))
        {
            other.gameObject.SetActive(false);
        }
        else
        {
            //Destroy(other.gameObject);
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
