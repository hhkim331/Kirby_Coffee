﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_Snow_90 : MonoBehaviour
{
    Rigidbody rb;

    public float speed = 5;
    public float sizespeed = 3;
    float size = 0;
    public float maxsize = 3;
    public float slopeForce = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        size += sizespeed * Time.deltaTime;
        if (size > maxsize)
        {
            size = maxsize;
        }
        else
        {
            transform.localScale = Vector3.one * size;
            Vector3 dir = Vector3.right;
            dir.Normalize();
            Vector3 velocity = dir * speed;
            rb.velocity = velocity;
        }
        if (Mathf.Abs(rb.velocity.x) > 3)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * 3, rb.velocity.y);
        if (rb.velocity.magnitude < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        //DestroySelf(other.gameObject);

        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //DestroySelf(other.gameObject);

        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Destroy(gameObject);
        }
    }

    void DestroySelf(GameObject go)
    {
        Rigidbody rb = go.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Destroy(gameObject);
        }
    }
}
