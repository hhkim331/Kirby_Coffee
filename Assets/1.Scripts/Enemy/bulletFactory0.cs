using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletFactory0 : MonoBehaviour
{
    public GameObject bulletFactory;
    public Transform bullet;
    public float speed = 5;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject bullet = Instantiate(bulletFactory);
        if (Input.GetButtonDown("A"))
        {
            bullet.transform.position = player.transform.position;
           
        }
    }
}

