using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_animEvent : MonoBehaviour
{
    public static psw_animEvent instance;
    public GameObject enemyFactory;
    public bool isJump = false;
   
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ATJump()
    {
        print("콘솔아 나 이벤트 찍혔나 확인 부탁행");
        isJump = true;
        GameObject st = Instantiate(enemyFactory);
        GetComponent<psw_starrrrr>().starMove();
    } 
}
