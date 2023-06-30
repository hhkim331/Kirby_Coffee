using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_Enemy_1 : MonoBehaviour
{
    public Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(Player);
        Vector3 v = transform.forward;
        v.y = 0;
        transform.forward = v;
    }
}
