﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerActionPistol : PlayerAction
{
    [SerializeField] Transform rightFirePos;
    [SerializeField] Transform leftFirePos;
    [SerializeField] LineRenderer rightLine;
    [SerializeField] LineRenderer leftLine;
    [SerializeField] GameObject bulletFactory;

    [SerializeField] RectTransform imageAim;

    enum ChargeLevel
    {
        None,
        Level1,
        Level2,
        Level3
    }
    ChargeLevel chargeLevel = ChargeLevel.None;

    //공격 시전중
    bool isFire = false;

    //발사키 입력중
    bool isFireKey = false;
    //차지 시간
    float chargeTime = 0.0f;
    float charge1Time = 0.5f;
    float charge2Time = 1.0f;
    float charge3Time = 1.5f;

    //에임 활성화
    bool isAim = false;
    //에임 이동 방향
    Vector3 aimDir = Vector3.zero;
    //에임 이동 속도
    float aimSpeed = 3f;
    //오른쪽 에임 방향
    Vector3 rightAimDir = Vector3.zero;
    //왼쪽 에임 방향
    Vector3 leftAimDir = Vector3.zero;


    //락온
    bool isLockOn = false;
    //락온 거리
    float lockOnDistance = 50f;
    //락온 타겟
    Collider lockOnTarget = null;
    //락온 해제 벡터
    Vector3 lockOnReleaseVec = Vector3.zero;
    //락온 해제 거리
    float lockOnReleaseDistance = 60f;



    // Update is called once per frame
    void Update()
    {
        //인풋이 없는 경우
        if (!GameManager.Input.isInput)
        {
            if (isFireKey)
                Fire();

            if (isAim) aimDir = Vector3.zero;
        }

        if (isFireKey)
        {
            chargeTime += Time.deltaTime;
            switch (chargeLevel)
            {
                case ChargeLevel.None:
                    if (chargeTime > charge1Time)
                    {
                        chargeLevel = ChargeLevel.Level1;
                        //에임 활성화
                        if (!isAim) ActiveAim();
                    }
                    break;
                case ChargeLevel.Level1:
                    if (chargeTime > charge2Time)
                    {
                        chargeLevel = ChargeLevel.Level2;
                    }
                    break;
                case ChargeLevel.Level2:
                    if (chargeTime > charge3Time)
                    {
                        chargeLevel = ChargeLevel.Level3;
                    }
                    break;
                case ChargeLevel.Level3:
                    break;
            }

            //에임 활성화 상태
            if (isAim)
                UpdateAim();
        }
    }

    public override void Set()
    {
        gameObject.SetActive(true);
    }

    public override void Unset()
    {
        gameObject.SetActive(false);
    }

    public override void KeyAction()
    {
        if (isFire) return;
        if (PlayerManager.Instance.PlayerMovement.IsFly) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isFireKey = true;
            IsHardAction = true;
            isAim = false;
            chargeLevel = ChargeLevel.None;
            rightLine.enabled = false;
            leftLine.enabled = false;
            imageAim.gameObject.SetActive(false);
            chargeTime = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            Fire();
        }

        //에임 이동
        if (isAim)
        {
            float h = Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow) ? 0 : Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
            float v = Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow) ? 0 : Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
            aimDir = new Vector3(h, v, 0);
            aimDir.Normalize();
            aimDir *= aimSpeed;
        }
    }

    #region 에임
    void ActiveAim()
    {
        isAim = true;
        imageAim.gameObject.SetActive(true);
        rightLine.enabled = true;
        leftLine.enabled = true;
        //에임 초기위치 설정
        //플레이어의 정면에서 최대 10미터까지 떨어진 곳
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 10f))
        {
            //월드좌표를 스크린좌표로 변환
            Vector3 imagePos = Camera.main.WorldToScreenPoint(hitInfo.point);
            imageAim.position = imagePos;
        }
        else
        {
            Vector3 imagePos = Camera.main.WorldToScreenPoint(ray.GetPoint(10f));
            imageAim.position = imagePos;
        }
    }

    void DeactiveAim()
    {
        isAim = false;
        rightLine.enabled = false;
        leftLine.enabled = false;
        imageAim.gameObject.SetActive(false);
    }

    void UpdateAim()
    {
        if (isLockOn)
        {
            lockOnReleaseVec += aimDir;
            if (lockOnReleaseVec.magnitude > lockOnReleaseDistance)
                LockOnRelease();
            else
                UpdateAimPlayer(lockOnTarget.bounds.center);
        }
        else
        {
            imageAim.position += aimDir;
            Ray ray = Camera.main.ScreenPointToRay(imageAim.position);
            // Raycast를 수행하여 가장 먼저 만나는 Collider를 찾습니다.
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                UpdateAimPlayer(hit.point);

                Collider collider = hit.collider;
                //콜라이더의 중심점을 구한다.
                Vector3 center = collider.bounds.center;
                //중심점의 위치를 구한다.
                Vector3 centerPos = Camera.main.WorldToScreenPoint(center);
                centerPos.z = 0;
                Vector3 imagePos = imageAim.position;
                imagePos.z = 0;

                //중심점과 광선과의 거리를 구한다.
                float distance = Vector3.Distance(centerPos, imagePos);
                if (distance < lockOnDistance)
                    LockOn(hit.collider);
            }
        }
    }

    void UpdateAimPlayer(Vector3 lookPoint)
    {
        //에임 방향을보는 캐릭터의 Y축회전각도 구하기
        Vector3 dir = lookPoint - PlayerManager.Instance.transform.position;
        dir.Normalize();
        float rotY = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        PlayerManager.Instance.PlayerMovement.NextFixedRotation = Quaternion.Euler(0, rotY, 0);

        //라인그리기
        rightLine.SetPosition(0, rightFirePos.position);
        rightLine.SetPosition(1, lookPoint + Vector3.right * 0.1f);
        leftLine.SetPosition(0, leftFirePos.position);
        leftLine.SetPosition(1, lookPoint + Vector3.left * 0.1f);

        //발사 방향 설정
        rightAimDir = lookPoint + Vector3.right * 0.1f - rightFirePos.position;
        rightAimDir.Normalize();
        leftAimDir = lookPoint + Vector3.left * 0.1f - leftFirePos.position;
        leftAimDir.Normalize();
    }

    /// <summary>
    /// 락온기능
    /// </summary>
    void LockOn(Collider lockOnCollider)
    {
        isLockOn = true;
        lockOnTarget = lockOnCollider;
        lockOnReleaseVec = Vector3.zero;

        imageAim.position = Camera.main.WorldToScreenPoint(lockOnTarget.bounds.center);
    }

    /// <summary>
    /// 락온 업데이트
    /// </summary>
    void UpdateLockOn()
    {

    }

    /// <summary>
    /// 락온 해제
    /// </summary>
    void LockOnRelease()
    {
        imageAim.position = Camera.main.WorldToScreenPoint(lockOnTarget.bounds.center) + lockOnReleaseVec;  //거리보정
        isLockOn = false;
        lockOnTarget = null;
    }
    #endregion

    #region 발사
    void Fire()
    {
        switch (chargeLevel)
        {
            case ChargeLevel.Level1:
                StartCoroutine(Charge1FireCoroutine());
                break;
            case ChargeLevel.Level2:
                StartCoroutine(Charge2FireCoroutine());
                break;
            case ChargeLevel.Level3:
                StartCoroutine(Charge3FireCoroutine());
                break;
            default:
                StartCoroutine(BasicFireCoroutine());
                break;
        }

        isFire = true;
        isFireKey = false;
        DeactiveAim();
        chargeTime = 0;
        chargeLevel = ChargeLevel.None;
    }

    IEnumerator BasicFireCoroutine()
    {
        IsAction = true;
        IsHardAction = false;
        yield return new WaitForSeconds(0.1f);
        Instantiate(bulletFactory, rightFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(transform.forward);
        yield return new WaitForSeconds(0.1f);
        Instantiate(bulletFactory, leftFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(transform.forward);
        isFire = false;
        IsAction = false;
        IsHardAction = false;
    }

    IEnumerator Charge1FireCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, rightFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(rightAimDir);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, leftFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(leftAimDir);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, rightFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(rightAimDir);
        isFire = false;
        IsHardAction = false;
    }

    IEnumerator Charge2FireCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, rightFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(rightAimDir);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, leftFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(leftAimDir);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, rightFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(rightAimDir);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, leftFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(leftAimDir);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, rightFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(rightAimDir);
        isFire = false;
        IsHardAction = false;
    }

    IEnumerator Charge3FireCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, rightFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(rightAimDir);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, leftFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(leftAimDir);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, rightFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(rightAimDir);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, leftFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(leftAimDir);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, rightFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(rightAimDir);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, leftFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(leftAimDir);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, rightFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(rightAimDir);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, leftFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(leftAimDir);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, rightFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(rightAimDir);
        isFire = false;
        IsHardAction = false;
    }
    #endregion
}
