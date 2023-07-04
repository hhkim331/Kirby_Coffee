using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_Snow : MonoBehaviour
{

    Rigidbody rb;

   
    public string Player;
    public float speed = 5;
    public float sizespeed = 3;
    float size = 0;
    public float maxsize = 3; 
    public float slopeForce = 5; // 경사로 올라갈 때 가해지는 힘의 크기
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.right * speed;
        //rb.useGravity = false; // 중력 비활성화
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
            // dir의 크기를 1로 만들고싶다.
            dir.Normalize();
            // 3. 그 방향으로 이동하고싶다. P = P + vt
            Vector3 velocity = dir * speed;
            rb.velocity = velocity;
            //transform.position += velocity * Time.deltaTime;
            //ApplySlopeForce();
        }
    }

    void ApplySlopeForce()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.right, out hit))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.right);
            if (slopeAngle > 0f && slopeAngle < 90f)
            {
                float slopeFactor = Mathf.Clamp01(slopeAngle / 500f); // 경사로 올라갈 때 적용할 힘의 크기 조정
                Vector3 slopeForceVector = hit.normal * slopeForce * slopeFactor;
                rb.AddForce(slopeForceVector, ForceMode.Force);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        //Destroy(gameObject, delayTime);
        var snow = other.gameObject.GetComponent<Rigidbody>();
        var playerController = other.gameObject.GetComponent<Rigidbody>();
        if (playerController != null)
        {
            Destroy(gameObject);
        }
    }
  
}
       
