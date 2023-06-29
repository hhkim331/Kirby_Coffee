using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionPistol : PlayerAction
{
    [SerializeField] Transform rightFirePos;
    [SerializeField] Transform leftFirePos;
    [SerializeField] GameObject bulletFactory;

    [SerializeField] Image imageAim;

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

    //조준점 활성화
    bool isAim = false;
    //조준점 활성화 시간
    float aimTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //인풋이 없는 경우
        if (!GameManager.Input.isInput)
        {
            if (isFireKey)
                Fire();
        }

        if (isFireKey && !isCharge)
        {
            chargeTime += Time.deltaTime;
            if (chargeTime > aimTime)
            {
                isAim = true;
                //에임 활성화
            }

            if (chargeTime > chargeMaxTime)
            {
                isCharge = true;
            }
        }

        //에임 활성화 상태
        if(isAim)
        {

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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isFireKey = true;
            isAim = false;
            isCharge = false;
            chargeTime = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            Fire();
        }
    }

    void Fire()
    {
        isFire = true;
        isFireKey = false;
        if (!isCharge)
            StartCoroutine(BasicFireCoroutine());
        else
        {
            isAim = false;
            //에임 비활성화
            
            StartCoroutine(ChargeFireCoroutine());
        }
    }

    IEnumerator BasicFireCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        Instantiate(bulletFactory, rightFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(transform.forward);
        yield return new WaitForSeconds(0.1f);
        Instantiate(bulletFactory, leftFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(transform.forward);
        isFire = false;
    }

    IEnumerator ChargeFireCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, rightFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(transform.forward);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, leftFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(transform.forward);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, rightFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(transform.forward);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, leftFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(transform.forward);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bulletFactory, rightFirePos.position, Quaternion.identity).GetComponent<PistolBullet>().Set(transform.forward);
        isFire = false;
    }
}
