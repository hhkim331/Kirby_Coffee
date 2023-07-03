using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSB_Boss : MonoBehaviour
{
    // state 구성
    //1.플레이어에게 다가가서 망치로 내려찍기 내려찍으면 별이 나온다
    //2.공중에서 점프 후 빙글빙글 돌다가 내려찍으면 원형으로 퍼지는 원거리 공격
    //3.anystate - 플레이어의 공격을 받으면 넘어진다
    //4.HP가 40%남으면 무기1을 떨어뜨린다
    //5.공중에 떴다가 커비 방향으로 망치 무기1을 내려찍는다
    //6.기둥무기를 가지고 오는 시네머신
    //7.공중에 떴다가 커비 방향으로 무기2를 내려찍는다
    //8.HP가 20% 남으면 연속으로 바닥에 커비방향으로 무기2를 3번 내려찍는다
    //9.옆쪽으로 휙 무기를 커비 방향으로 휘두른다
    //10.anystate - Die

    public enum BossState
    {
        Idle,
        Move,
        Attack, //1.
        JumpSpin,//2.
        JumpStop,//2
        Attack2,//2.
        Damage,//3.
        Drop,//4.
    };

    public BossState m_state = BossState.Idle;
    //리지드바디
    Rigidbody rb;
    //해머를 가지고 있는지
    bool isHammer = false;
    //첫번째 각도
    Quaternion originRot;

    void Start()
    {   
        //리지드 바디
        rb = GetComponent<Rigidbody>();
        //Move때 저장할 해머 초기 회전값
        originRot = hammer.transform.rotation;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case BossState.Idle:
                Idle();
                break;
            case BossState.Move:
                Move();
                break;
            case BossState.Attack:
                Attack();
                break;
            case BossState.JumpSpin:
                JumpSpin();
                break;
            case BossState.JumpStop:
                JumpStop();
                break;
            case BossState.Attack2:
                Attack2();
                break;
            case BossState.Damage:
                Damage();
                break;
            case BossState.Drop:
                Drop();
                break;
        }
        //IsHammer가 true일때
        if (isHammer == true)
        {
            //게임오브젝트 해머를 활성화한다
            hammer.SetActive(true);

        }
    }

    //플레이어와 일정거리 이상 가까워지면 상태를 Move로 변경한다
    //필요속성 : 타겟, 일정시간 , 현재시간
    public GameObject target;
    public float creatTime;
    float currentTime = 0;

    private void Idle()
    {
        //3초 후에 상태를 Move로 변경한다

        //필요속성 : 일정시간
        creatTime = 3;
        //1.3초가 지나면
        currentTime += Time.deltaTime;
        if (currentTime > creatTime)
        {
            //3. 상태를 Move로 변경한다
            m_state = BossState.Move;
            currentTime = 0;
        }
    }

    //필요속성 : 일정거리 , 스피드
    public float moveRange = 5;
    public float speed = 10;
    private void Move()
    {
        //플레이어와 일정거리 이상 가까워지면 상태를 어택으로 전환한다
        //필요속성 : 타겟쪽으로 방향 , 처음 플레이어와의 거리 , 일정거리
        Vector3 dir = target.transform.position - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();
        //바닥에 붙어있도록 한다
        dir.y = 0;
        //방향쪽으로 이동한다
        transform.position += dir * speed * Time.deltaTime;
        //타겟방향으로 몸 회전시키기 LookRotation은 해당벡터를 바라보는 함수
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        //해머활성화
        isHammer = true;
        
        Quaternion secontRot = Quaternion.Euler(-90, 0, 0);

        currentTime += Time.deltaTime;
        if (currentTime < 3)
        {
            hammer.transform.localRotation = Quaternion.Lerp(originRot, secontRot, currentTime *10);
            currentTime = 0;
        }

        //플레이어와 일정거리 이상 가까워지면 상태를 어택으로 전환한다
        //일정거리 이상 좁혀지면
        if (distance < moveRange)
        { 
            //상태를 어택으로 전환한다
            m_state = BossState.Attack;
        }

    }
    //타겟이랑 일정거리 이상 좁혀지면 weapon을 들어서 내려찍는다
    //필요속성 : Hammer
    public GameObject hammer;
    private void Attack()
    {
        //플레이어쪽으로 이동한다 
        Vector3 dir = target.transform.position - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();
        //바닥에 붙어있도록 한다
        dir.y = 0;
        transform.position += dir * speed * Time.deltaTime;
        //moveRange가 1, moveRange보다 현재 거리가 좁으면
        moveRange = 1.5f;
        if(distance < moveRange)
        {
            //가까이 다가갔으면 멈추고 
            speed = 0;

           //해머활성화
            isHammer = true;
            Quaternion secontRot = Quaternion.Euler(-90, 0, 0);
            Quaternion thirdRot = Quaternion.Euler(20, 0, 0);

            currentTime += Time.deltaTime;

            if(currentTime < 3)
            {
                hammer.transform.localRotation = Quaternion.Lerp(secontRot, thirdRot, (currentTime / 2)*20 );
                //currentTime = 0;
            }
            //Invoke 2초
            Invoke("TimeLimit", 2);
        }
    }
    void TimeLimit()
    {
        m_state = BossState.JumpSpin;
    }
    private void JumpSpin()
    {

        // 플레이어 쪽으로 바라보고 y축으로 점프한다.
        float jumpPower = 10;
        Vector3 dir = Vector3.up;
        dir.Normalize(); 
        //점프를 한다
        transform.position += dir * jumpPower * Time.deltaTime;
        //Invoke 1초
        Invoke("JumpStop", 0.6f);
    }

    private void JumpStop()
    {
        m_state = BossState.Attack2;
    }

    private void Attack2()
    {
        float jumpPower = 30;
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();
        transform.position += dir * jumpPower * Time.deltaTime;
        dir.y = 0; //바닥으로 내려찍기
        m_state = BossState.Damage;
    }

    private void Damage()
    {

    }

    private void Drop()
    {

    }
}
