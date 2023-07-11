using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public enum CameraDistanceState
    {
        Basic,
        Zoomin,
    }

    public CameraDistanceState DistanceState
    {
        set
        {
            switch (value)
            {
                case CameraDistanceState.Basic:
                    curDistance = basicDistance;
                    curOffset = Quaternion.Euler(curAngle) * Vector3.back * curDistance + Vector3.up * targetYOffset;
                    break;
                case CameraDistanceState.Zoomin:
                    curDistance = zoomInDistance;
                    curOffset = Quaternion.Euler(curAngle) * Vector3.back * curDistance;
                    break;
            }
            //curOffset = Quaternion.Euler(curAngle) * Vector3.back * curDistance + Vector3.up * targetYOffset;
        }
    }

    public enum CameraAngleState
    {
        Basic,
        Left,
        Right,
    }

    public CameraAngleState AngleState
    {
        set
        {
            switch (value)
            {
                case CameraAngleState.Basic:
                    curAngle = basicAngle;
                    break;
                case CameraAngleState.Left:
                    curAngle = leftAngle;
                    break;
                case CameraAngleState.Right:
                    curAngle = rightAngle;
                    break;
            }
            curOffset = Quaternion.Euler(curAngle) * Vector3.back * curDistance + Vector3.up * targetYOffset;
        }
    }

    //카메라가 타겟을 바라보는 위치 보정
    readonly float targetYOffset = 1.5f;

    //카메라 거리
    readonly float basicDistance = 10f;
    readonly float zoomInDistance = 7f;

    //카메라 각도
    readonly Vector3 basicAngle = new Vector3(30, 0, 0);  //바라보는 각도
    readonly Vector3 leftAngle = new Vector3(30, 45, 0);  //왼쪽으로 바라보는 각도
    readonly Vector3 rightAngle = new Vector3(30, -45, 0);  //왼쪽으로 바라보는 각도

    float curDistance;
    Vector3 curAngle;
    Vector3 curOffset;

    // Start is called before the first frame update
    void Start()
    {
        DistanceState = CameraDistanceState.Basic;
        AngleState = CameraAngleState.Right;

        //transform.rotation = Quaternion.Euler(curAngle);
        //transform.position = targetPlayer.position + curOffset;
    }

    //변신할때만 사용
    private void LateUpdate()
    {
        if (PlayerManager.Instance.IsChange)
        {
            transform.position = Vector3.Lerp(transform.position, PlayerManager.Instance.PMovement.CameraViewPoint + curOffset, Time.unscaledDeltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(curAngle), Time.unscaledDeltaTime * 5);
        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, PlayerManager.Instance.PMovement.CameraViewPoint + curOffset, Time.fixedDeltaTime * 5);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(curAngle), Time.fixedDeltaTime * 5);
    }

    /// <summary>
    /// 카메라 쉐이크
    /// </summary>
    /// <param name="shakePower"></param>
    /// <param name="shakeTime"></param>
    public void CameraShake(float shakePower, float shakeTime)
    {
        StartCoroutine(CameraShakeCoroutine(shakePower, shakeTime));
    }

    IEnumerator CameraShakeCoroutine(float shakePower, float shakeTime)
    {
        Vector3 originPos = transform.position;
        float curTime = 0f;
        while (curTime < shakeTime)
        {
            transform.position = originPos + Random.insideUnitSphere * shakePower;
            curTime += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.position = originPos;
    }

    /// <summary>
    /// 카메라 쉐이크 한번만
    /// </summary>
    /// <param name="shakePower"></param>
    public void CameraShakeOnce(float shakePower)
    {
        StartCoroutine(CameraShakeOnceCoroutine(shakePower));
    }

    IEnumerator CameraShakeOnceCoroutine(float shakePower)
    {
        Vector3 originPos = transform.position;
        transform.position = originPos + Random.insideUnitSphere * shakePower;
        yield return null;
        transform.position = originPos;
    }
}
