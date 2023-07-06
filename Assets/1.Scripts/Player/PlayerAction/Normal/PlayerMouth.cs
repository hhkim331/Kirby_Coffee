using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouth : MonoBehaviour
{
    public enum MOUTHSTACK
    {
        None,
        Object,
    }
    MOUTHSTACK stack = MOUTHSTACK.None;
    public MOUTHSTACK Stack { get { return stack; } }

    [SerializeField] GameObject suction;
    bool canUse = true;
    bool isSuction = false;
    public bool IsSuction
    {
        get { return isSuction; }
        set
        {
            isSuction = value;
            if (suction.activeSelf != value)
                suction.SetActive(value);
        }
    }
    bool canSuction = true;
    float suctionDelay = 0f;

    //입 위치
    [SerializeField] Transform mouthTransform;
    //먹은 물건
    GameObject stackObject;

    private void Update()
    {
        //인풋이 없는 경우
        if (!GameManager.Input.isInput)
            IsSuction = false;

        //빨아들이기 쿨타임
        if (!canSuction)
        {
            suctionDelay -= Time.deltaTime;
            if (suctionDelay < 0f)
                canSuction = true;
        }
    }

    public void Set(PlayerManager.CHANGETYPE type)
    {
        if (type == PlayerManager.CHANGETYPE.Normal)
            canUse = true;
        else
            canUse = false;
    }

    public void KeyAction()
    {
        if (PlayerManager.Instance.IsChange || PlayerManager.Instance.IsUnChange) return;
        if (PlayerManager.Instance.PlayerMovement.IsFly) return;
        if (!canUse) return;

        if (stack != MOUTHSTACK.None)
        {
            //삼키기
            if (Input.GetKeyDown(KeyCode.A))
            {
                switch (stack)
                {
                    case MOUTHSTACK.Object:
                        Destroy(stackObject);
                        break;
                        //case Stack.Enemy_Pistol:
                        //    break;
                }
                stack = MOUTHSTACK.None;
            }

            //뱉기(발사만 가능함)
            if (Input.GetKeyDown(KeyCode.Z))
            {
                switch (stack)
                {
                    case MOUTHSTACK.Object:
                        //바라보는 방향으로 물건 뱉기
                        stackObject.SetActive(true);
                        stackObject.transform.parent = null;
                        stackObject.transform.position = mouthTransform.position + transform.forward;
                        PlayerNormalBullet playerBullet = stackObject.AddComponent<PlayerNormalBullet>();
                        playerBullet.Set(transform.forward);
                        //물건을 뱉은 후 쿨타임
                        canSuction = false;
                        suctionDelay = PlayerManager.Instance.Data.suctionDelay;
                        break;
                }
                stack = MOUTHSTACK.None;
            }
        }
        else
        {
            //빨아들이기
            if (Input.GetKey(KeyCode.Z) && canSuction)
                IsSuction = true;
            else
                IsSuction = false;
        }
    }

    public void SetStack(GameObject suctionObejct)
    {
        stack = MOUTHSTACK.Object;
        stackObject = suctionObejct;
        //물건을 비활성화 상태로 가지고 있는다.
        suctionObejct.transform.parent = transform;
        suctionObejct.transform.localPosition = Vector3.zero;
        suctionObejct.SetActive(false);
    }
}
