using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Transform targetPlayer;

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
                    break;
                case CameraDistanceState.Zoomin:
                    curDistance = zoomInDistance;
                    break;
            }
            curOffset = Quaternion.Euler(curAngle) * Vector3.back * curDistance;
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
            curOffset = Quaternion.Euler(curAngle) * Vector3.back * curDistance;
        }
    }

    //카메라 거리
    readonly float basicDistance = 10f;
    readonly float zoomInDistance = 7f;

    //카메라 각도
    readonly Vector3 basicAngle = new Vector3(45, 0, 0);  //바라보는 각도
    readonly Vector3 leftAngle = new Vector3(45, 45, 0);  //왼쪽으로 바라보는 각도
    readonly Vector3 rightAngle = new Vector3(45, -45, 0);  //왼쪽으로 바라보는 각도

    float curDistance;
    Vector3 curAngle;
    Vector3 curOffset;

    // Start is called before the first frame update
    void Start()
    {
        targetPlayer = PlayerManager.Instance.transform;
        DistanceState = CameraDistanceState.Basic;
        AngleState = CameraAngleState.Right;

        //transform.rotation = Quaternion.Euler(curAngle);
        //transform.position = targetPlayer.position + curOffset;
    }

    //변신할때만 사용
    private void Update()
    {
        if(PlayerManager.Instance.IsChange)
        {
            transform.position = Vector3.Lerp(transform.position, targetPlayer.position + curOffset, Time.unscaledDeltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(curAngle), Time.unscaledDeltaTime * 5);
        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetPlayer.position + curOffset, Time.fixedDeltaTime * 5);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(curAngle), Time.fixedDeltaTime * 5);
    }
}
