using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_Snow : MonoBehaviour
{
   
    Rigidbody rb;

    public float delayTime = 5f;
    public string Player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(0.003f, 0.003f, 0.003f);
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject, delayTime);
        var snow = other.gameObject.GetComponent<Rigidbody>();
        var playerController = other.gameObject.GetComponent<CharacterController>();
        if (snow != null)
        {
            snow.AddForce(transform.forward * snow.mass * 5, ForceMode.Impulse);
        }
        
        if (playerController != null)
        {
            Destroy(gameObject);
        }
    }
}
       