using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Object/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("속도")]
    public float speed;
    [Header("회전속도")]
    public float rotateSpeed;
    [Header("점프파워")]
    public float jumpPower;
    [Header("점프버튼유지시간")]
    public float jumpFlagTime;
    [Header("중력")]
    public float gravity;
    [Header("최대하강속도")]
    public float maxFallSpeed;
    [Header("비행 힘")]
    public float flyPower;
    [Header("비행가능 시간")]
    public float flyTime;
    [Header("날개짓 딜레이(키다운)")]
    public float flyActionDelay;
    [Header("비행최대하강속도")]
    public float maxFlyFallSpeed;

    [Space]
    [Header("빨아들이는 힘")]
    public float suctionPower;
    [Header("빨아들이기 준비시간")]
    public float suctionDelay;
    [Header("뱉은 물건의 속도")]
    public float spitItemSpeed;
}