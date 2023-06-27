using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Object/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("속도")]
    public float speed;
    [Header("점프파워")]
    public float jumpPower;
    [Header("날기 힘")]
    public float flyPower;
    [Header("날수있는 시간")]
    public float flyTime;
    [Header("중력")]
    public float gravity;
}