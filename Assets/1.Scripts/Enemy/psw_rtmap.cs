using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_rtmap : MonoBehaviour
{
    public float speed = 5;
    public float rotSpeed = 200;
    float rz;
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
        rz = Mathf.Clamp(rz, -20, 20);
    }

    // Update is called once per frame
    void Update()
    {
        //rz += Time.deltaTime * speed;
        //transform.rotation = Quaternion.Euler(0, 0, rz );
        //rz = Mathf.Clamp(rz, -20, 20);
        
        
        if (Rotation)
        {
            currentTime += Time.deltaTime;
            if (currentTime > 2f)
            {
                rotationDirection = !rotationDirection; // ȸ�� ������ �ݴ�� ����
                currentTime = 0;
            }

            // ȸ�� ���⿡ ���� rz �� ���� �Ǵ� ����
            if (rotationDirection)
            {
                rz += Time.deltaTime * speed;
            }
            else
            {
                rz -= Time.deltaTime * speed;
            }

            // rz ���� -20�� 20 ���̷� ����
            rz = Mathf.Clamp(rz, -20, 20);
            transform.rotation = Quaternion.Euler(0, 0, rz);
        }
    }
}
