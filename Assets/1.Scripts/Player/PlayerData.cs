using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Object/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("�ӵ�")]
    public float speed;
    [Header("�����Ŀ�")]
    public float jumpPower;
    [Header("���� ��")]
    public float flyPower;
    [Header("�����ִ� �ð�")]
    public float flyTime;
    [Header("�߷�")]
    public float gravity;
}