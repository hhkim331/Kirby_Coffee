using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerData playerData;
    CharacterController cc;

    //중력
    float yVelocity;
    //날기
    bool isFly = false;
    bool isCanFly = true;
    float flyActiveTime;

    // Start is called before the first frame update
    public void MoveStart(PlayerManager manager, PlayerData data)
    {
        playerManager = manager;
        playerData = data;
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    public void MoveUpdate()
    {
        yVelocity += playerData.gravity * Time.deltaTime;
        float h = Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow) ? 0 : Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
        float v = Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow) ? 0 : Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;

        Vector3 dir = new Vector3(h, 0, v);
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        dir.Normalize();
        dir *= playerData.speed;

        if (Input.GetButtonDown("Jump"))
        {
            if (cc.isGrounded)  //첫점프
                yVelocity = playerData.jumpPower;
            else if (isCanFly)
            {
                if(!isFly)
                {
                    isFly = true;
                    flyActiveTime = playerData.flyTime;
                }
                yVelocity = playerData.flyPower;
            }
        }

        //날고 있는 상태
        if (isFly)
        {
            flyActiveTime -= Time.deltaTime;
            if (flyActiveTime <= 0)
                isCanFly = false;
        }

        //땅에 닿으면 날기 다시 가능
        if (cc.isGrounded)
        {
            isFly = false;
            isCanFly = true;
        }

        dir.y = yVelocity;
        cc.Move(dir * Time.deltaTime);
    }
}
