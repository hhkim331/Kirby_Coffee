using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSB_Boss : MonoBehaviour
{
    // state ����
    //1.�÷��̾�� �ٰ����� ��ġ�� ������� ���������� ���� ���´�
    //2.���߿��� ���� �� ���ۺ��� ���ٰ� ���������� �������� ������ ���Ÿ� ����
    //3.anystate - �÷��̾��� ������ ������ �Ѿ�����
    //4.HP�� 40%������ ����1�� ����߸���
    //5.���߿� ���ٰ� Ŀ�� �������� ��ġ ����1�� ������´�
    //6.��չ��⸦ ������ ���� �ó׸ӽ�
    //7.���߿� ���ٰ� Ŀ�� �������� ����2�� ������´�
    //8.HP�� 20% ������ �������� �ٴڿ� Ŀ��������� ����2�� 3�� ������´�
    //9.�������� �� ���⸦ Ŀ�� �������� �ֵθ���
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
    //������ٵ�
    Rigidbody rb;
    //ù��° ����
    Quaternion originRotation;

    private void Awake()
    {
        //Hammer�� �����´�
        hammer = GameObject.Find("Hammer");
    }

    void Start()
    {   
        //������ �ٵ�
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

    //�÷��̾�� �����Ÿ� �̻� ��������� ���¸� Move�� �����Ѵ�
    //�ʿ�Ӽ� : Ÿ��, �����ð� , ����ð�
    public GameObject target;
    public float creatTime;
    float currentTime = 0;

    private void Idle()
    {
        //3�� �Ŀ� ���¸� Move�� �����Ѵ�

        //�ʿ�Ӽ� : �����ð�
        creatTime = 3;
        //1.3�ʰ� ������
        currentTime += Time.deltaTime;
        if (currentTime > creatTime)
        {
            //3. ���¸� Move�� �����Ѵ�
            m_state = BossState.Move;
        }


    }

    //�ʿ�Ӽ� : �����Ÿ� , ���ǵ�
    public float moveRange = 3;
    public float speed = 10;
    private void Move()
    {
        //�÷��̾�� �����Ÿ� �̻� ��������� ���¸� �������� ��ȯ�Ѵ�
        //�ʿ�Ӽ� : Ÿ�������� ���� , ó�� �÷��̾���� �Ÿ� , �����Ÿ�
        Vector3 dir = target.transform.position - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();
        //�ٴڿ� �پ��ֵ��� �Ѵ�
        dir.y = 0;
        //���������� �̵��Ѵ�
        transform.position += dir * speed * Time.deltaTime;
        //Ÿ�ٹ������� �� ȸ����Ű�� LookRotation�� �ش纤�͸� �ٶ󺸴� �Լ�
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        //�÷��̾�� �����Ÿ� �̻� ��������� ���¸� �������� ��ȯ�Ѵ�
        //�����Ÿ� �̻� ��������
        if (distance < moveRange)
        {
            //���¸� �������� ��ȯ�Ѵ�
            m_state = BossState.Attack;
        }

    }
    //Ÿ���̶� �����Ÿ� �̻� �������� weapon�� �� ������´�
    //�ʿ�Ӽ� : Hammer
    public GameObject hammer;
    private void Attack()
    {
        //�÷��̾������� �̵��Ѵ� 
        Vector3 dir = target.transform.position - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();
        transform.position += dir * speed * Time.deltaTime;
        //moveRange�� 1, moveRange���� ���� �Ÿ��� ������
        moveRange = 2;
        if(distance < moveRange)
        {
            Vector3 Angle = -Vector3.right;

            //Gameobject Hammer�� x������ -90�� ���� �÷ȴٰ� 
            hammer.transform.Rotate(Angle * Time.deltaTime * 300);
            //1�ʰ� ������ Hammer�� x������ 20���� �����Ѵ�.
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
