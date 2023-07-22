using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_starPosition : MonoBehaviour
{
    
    public GameObject target;
    // 현재 시간
    float currentTime;
    // 생성 시간
    public float makeTime = 1;
    // 적공장
    public GameObject enemyFactory;
    public float bulletSpeed = 5.0f; // 원하는 총알 속도를 설정합니다.
    public bool isJump = false;

    
    void Start()
    {
        target = GameObject.Find("Player");
        //psw_animEvent.instance.ATJump();
    }

    // Update is called once per frame
    void Update()
    {
        // 거리 계산
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        // 2. 만약 현재시간이 생성시간이 되면
        if (isJump == true)
        {
            // 3. 적공장에서 적을 만들어서
            GameObject bullet = Instantiate(enemyFactory);
            // 4. 내 위치에 배치하고 싶다.
            bullet.transform.position = transform.position;
            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();
            bullet.transform.forward = direction;

            GameObject.FindWithTag("starEffect").GetComponent<psw_starrrrr>().starMove();

        }
    }
}
