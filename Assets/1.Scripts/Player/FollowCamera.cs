using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Transform targetPlayer;

    public enum CameraState
    {
        Basic,
        Zoomin,
        MapView,
    }
    CameraState state;
    public CameraState State
    {
        set
        {
            state = value;
            switch (value)
            {
                case CameraState.Basic:
                    curOffset = basicOffset;
                    Quaternion rot = Quaternion.LookRotation(Vector3.zero - basicOffset);
                    transform.rotation = rot;
                    break;
                case CameraState.Zoomin:
                    curOffset = zoomInOffset;
                    break;
                case CameraState.MapView:
                    break;
            }
        }
    }

    Vector3 curOffset;

    //기본 카메라
    Vector3 basicOffset = new Vector3(0, 8, -8); //배치
    //줌인 카메라
    Vector3 zoomInOffset = new Vector3(0, 6, -6);  //배치

    // Start is called before the first frame update
    void Start()
    {
        targetPlayer = PlayerManager.Instance.transform;
        State = CameraState.Basic;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetPlayer.position + curOffset, Time.fixedDeltaTime * 5);
    }
}
