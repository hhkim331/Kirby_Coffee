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
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
