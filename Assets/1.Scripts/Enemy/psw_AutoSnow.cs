using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_AutoSnow : MonoBehaviour
{
    float currentTime;
    public float makeTime = 1f;
    public GameObject Snow;
    public Transform SnowPosition;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > makeTime)
        {
            GameObject snow = Instantiate(Snow);
            snow.transform.position = SnowPosition.position;
            currentTime = 0;
        }
    }
}
