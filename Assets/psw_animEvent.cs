﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_animEvent : MonoBehaviour
{
    //public static psw_animEvent instance;
    public bool isJump = false;

    public psw_starPosition anim;
    public psw_starPosition anim1;
    public psw_starPosition anim2;
    public psw_starPosition anim3;
    public psw_starPosition anim4;

    public GameObject particle;

    public void ATJump()
    {
        print("콘솔아 나 이벤트 찍혔나 확인 부탁행");
        isJump = true;
        anim.makeStar();
        anim1.makeStar();
        anim2.makeStar();
        anim3.makeStar();
        anim4.makeStar();

        GameObject obj = Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(obj, 3.2f);
    }

    public void StarAttack()
    {
        print("ㅇㅇㅇㅇㅇㅇ");
        anim.makeStar1();
    }
}
