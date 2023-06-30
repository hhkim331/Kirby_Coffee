﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_rtmap_2 : MonoBehaviour
{
    public float speed = 5;
    public float rotSpeed = 200;
    float rx;
    float currentTime;
    public bool Rotation = true;
    bool rotationDirection = true;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
    }

    void CanRotation()
    {
        rx = Mathf.Clamp(rx, -20, 20);
    }

    // Update is called once per frame
    void Update()
    {
      if (Rotation)
        {
            currentTime += Time.deltaTime;
            if (currentTime > 2f)
            {
                rotationDirection = !rotationDirection;
                currentTime = 0;
            }

            if (rotationDirection)
            {
                rx += Time.deltaTime * speed;
            }
            else
            {
                rx -= Time.deltaTime * speed;
            }

            rx = Mathf.Clamp(rx, -20, 20);
           // Vector3 Rotation = transform.rotation.eulerAngles;
           //// transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, rx);

        }
    }
}
