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
        //Ÿ���� ������ ������ ���󰡴� ī�޶�
        transform.position = targetPlayer.position + new Vector3(0, 10, -10);
        transform.LookAt(targetPlayer);
        
        

    }

    //�÷��̾ ������ �ϴ� ��� ��¦ ����
    


    //ī�޶� ���� ���� �þ߰� �������� 45�� ���ư��ִ� ����



    //Ÿ�� ���� ���� ��ܿ��� �ٶ󺸴� ����
}
