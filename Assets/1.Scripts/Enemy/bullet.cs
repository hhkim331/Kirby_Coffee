﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 위로 이동하고싶다.
public class bullet : MonoBehaviour
{
    public float speed = 10;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("A"))
        {
        transform.position += transform.right * speed * Time.deltaTime;
         
        }
    }
}
