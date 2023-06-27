using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Transform targetPlayer;

    public enum CameraState
    {
        Normal,
        Transformation,
        MapView,
    }

    // Start is called before the first frame update
    void Start()
    {
        targetPlayer = PlayerManager.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //타겟을 정해진 각도로 따라가는 카메라
        transform.position = targetPlayer.position + new Vector3(0, 10, -10);
        transform.LookAt(targetPlayer);
        
        

    }

    //플레이어가 변신을 하는 경우 살짝 줌인
    


    //카메라가 맵을 보는 시야가 왼쪽으로 45도 돌아가있는 상태



    //타겟 맵을 높은 상단에서 바라보는 상태
}
