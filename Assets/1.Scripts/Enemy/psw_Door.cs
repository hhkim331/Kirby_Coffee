using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_Door : MonoBehaviour
{
    public float speed = 5;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3 dir = Vector3.up;
    float currentTime;

    // Update is called once per frame
    void Update()
    {
        // 태어날 때 Mark를 찾고 싶다.
        target = GameObject.Find("Mark");
        if (target.GetComponent<MeshRenderer>().enabled == false)
        {
            dir = Vector3.up;
            transform.position += dir * speed * Time.deltaTime;
            currentTime += Time.deltaTime;
            if (currentTime > 3)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
