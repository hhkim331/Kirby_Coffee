using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionPistol : PlayerAction
{
    [SerializeField] Transform rightFirePos;
    [SerializeField] Transform leftFirePos;
    [SerializeField] LineRenderer rightLine;
    [SerializeField] LineRenderer leftLine;
    [SerializeField] GameObject bulletFactory;

    [SerializeField] RectTransform imageAim;

    //공격 시전중
    bool isFire = false;

    //발사키 입력중
    bool isFireKey = false;
    //차지공격
    bool isCharge = false;
    //차지 시간
    float chargeTime = 0.0f;
    //차지 최대 시간
    float chargeMaxTime = 1.0f;

    //에임 활성화
    bool isAim = false;
    //에임 활성화 시간
    float aimTime = 0.1f;
    //에임 이동 방향
    Vector3 aimDir = Vector3.zero;
    //에임 이동 속도
    float aimSpeed = 3f;
    //오른쪽 에임 방향
    Vector3 rightAimDir = Vector3.zero;
    //왼쪽 에임 방향
    Vector3 leftAimDir = Vector3.zero;

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

        if (isFireKey && !isCharge)
        {
            chargeTime += Time.deltaTime;
            if (chargeTime > aimTime)
            {
                //에임 활성화
                if (!isAim)
                {
                    isAim = true;
                    IsHardAction = true;
                    imageAim.gameObject.SetActive(true);
                    rightLine.enabled = true;
                    leftLine.enabled = true;
                    //에임 위치 설정
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
            }

            if (chargeTime > chargeMaxTime)
            {
                isCharge = true;
            }
        }

        //에임 활성화 상태
        if (isAim)
        {
            imageAim.position += aimDir;
            Ray ray = Camera.main.ScreenPointToRay(imageAim.position);
            // Raycast를 수행하여 가장 먼저 만나는 Collider를 찾습니다.
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //에임 방향을보는 캐릭터의 Y축회전각도 구하기
                Vector3 dir = hit.point - PlayerManager.Instance.transform.position;
                dir.Normalize();
                //dir.y = 0;
                //PlayerManager.Instance.transform.forward = dir;
                float rotY = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                PlayerManager.Instance.PlayerMovement.NextFixedRotation = Quaternion.Euler(0, rotY, 0);

                //라인그리기
                rightLine.SetPosition(0, rightFirePos.position);
                rightLine.SetPosition(1, hit.point + Vector3.right * 0.1f);
                leftLine.SetPosition(0, leftFirePos.position);
                leftLine.SetPosition(1, hit.point + Vector3.left * 0.1f);

                //발사 방향 설정
                rightAimDir = hit.point + Vector3.right * 0.1f - rightFirePos.position;
                rightAimDir.Normalize();
                leftAimDir = hit.point + Vector3.left * 0.1f - leftFirePos.position;
                leftAimDir.Normalize();
            }
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
            isAim = false;
            isCharge = false;
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

    void Fire()
    {
        isFire = true;
        isFireKey = false;
        isAim = false;
        rightLine.enabled = false;
        leftLine.enabled = false;
        imageAim.gameObject.SetActive(false);
        chargeTime = 0;
        if (!isCharge)
            StartCoroutine(BasicFireCoroutine());
        else
            StartCoroutine(ChargeFireCoroutine());
    }

    IEnumerator BasicFireCoroutine()
    {
        IsAction = true;
        yield return new WaitForSeconds(0.1f);
        Instantiate(bulletFactory, rightFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(transform.forward);
        yield return new WaitForSeconds(0.1f);
        Instantiate(bulletFactory, leftFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(transform.forward);
        isFire = false;
        IsAction = false;
        IsHardAction = false;
    }

    IEnumerator ChargeFireCoroutine()
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
}
