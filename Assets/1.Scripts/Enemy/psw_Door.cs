using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_Door : MonoBehaviour
{
    public float speed = 5;
    public GameObject target;
    public GameObject pswcamera;
    public GameObject[] coins;

    // Start is called before the first frame update
    void Start()
    {

    }

    Vector3 dir = Vector3.up;
    float currentTime;
    bool CanDoor = true;
    // Update is called once per frame
    void Update()
    {

        // 태어날 때 Mark를 찾고 싶다.
        target = GameObject.Find("Mark");
        if (target.GetComponent<MeshRenderer>().enabled == false && CanDoor)
        {
            Time.timeScale = 0;
            PlayerManager.Instance.Anim.updateMode = AnimatorUpdateMode.Normal;
            dir = Vector3.up;
            transform.position += dir * speed * Time.unscaledDeltaTime;
            currentTime += Time.unscaledDeltaTime;
            pswcamera.SetActive(true);
            if (currentTime > 2)
            {
                pswcamera.SetActive(false);
                Time.timeScale = 1;
                PlayerManager.Instance.Anim.updateMode = AnimatorUpdateMode.UnscaledTime;
                CanDoor = false;
                currentTime = 0;
            }
        }
        if (CanDoor == false)
        {
            currentTime += Time.deltaTime;
            if (currentTime > 1f)
            {
                if (coins[0] != null)
                    coins[0].SetActive(true);
            }
            if (currentTime > 1.2f)
            {
                if (coins[1] != null)
                    coins[1].SetActive(true);
            }
            if (currentTime > 1.4f)
            {
                if (coins[2] != null)
                    coins[2].SetActive(true);
            }
            if (currentTime > 1.6f)
            {
                if (coins[3] != null)
                    coins[3].SetActive(true);
            }
            if (currentTime > 1.8f)
            {
                if (coins[4] != null)
                    coins[4].SetActive(true);
            }
        }
    }
}
