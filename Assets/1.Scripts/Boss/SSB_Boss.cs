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
        Attack2,//2.
        Damage,//3.
        Drop,//4.
    };

    public BossState m_state = BossState.Idle;
    //리지드바디
    Rigidbody rb;
    //첫번째 각도
    Quaternion originRotation;

    private void Awake()
    {
        //Hammer를 가져온다
        hammer = GameObject.Find("Hammer");
    }

    void Start()
    {   
        //리지드 바디
        rb = GetComponent<Rigidbody>();
        
        
        
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
        }


    }

    //필요속성 : 일정거리 , 스피드
    public float moveRange = 3;
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
        transform.position += dir * speed * Time.deltaTime;
        //moveRange가 1, moveRange보다 현재 거리가 좁으면
        moveRange = 2;
        if(distance < moveRange)
        {
            Vector3 Angle = -Vector3.right;

            //Gameobject Hammer를 x축으로 -90도 까지 올렸다가 
            hammer.transform.Rotate(Angle * Time.deltaTime * 300);
            //1초가 지나면 Hammer를 x축으로 20도로 변경한다.
        }
    }

    private void Attack2()
    {

    }

    private void Damage()
    {

    }

    private void Drop()
    {

    }
}
