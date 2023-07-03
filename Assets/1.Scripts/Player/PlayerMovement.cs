using System.Collections;
using System.Globalization;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerData playerData;

    //이동
    Rigidbody rb;
    GroundChecker gc;
    Vector3 velocity;
    Vector3 planeVelocity;  //xz plane velocity
    Quaternion lastFixedRotation;
    Quaternion nextFixedRotation;
    public Quaternion NextFixedRotation { set { nextFixedRotation = value; } }

    //점프
    bool isJump = false;
    public bool IsJump { get { return isJump; } }
    bool jumpFlag = false;
    float jumpFlagTime;

    //날기
    bool isFly = false;
    public bool IsFly { get { return isFly; } }
    bool flyFlag = false;
    bool isCanFly = true;
    float flyActiveTime;    //비행 활성화 시간
    float flyActionDelay;   //날개짓 딜레이

    //내뱉기
    bool isBreathAttack = false;
    float breathAttackDelay;    //내뱉기 공격 딜레이
    [SerializeField] GameObject breathFactory;

    //피격
    bool isHit = false;
    float hitTime; //밀려나는 시간
    Vector3 hitDir; //밀려나는 방향

    public void Set(PlayerData data)
    {
        playerData = data;
        //cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        gc = GetComponent<GroundChecker>();

        velocity = new Vector3(0, 0, 0);
        //lastFixedPosition = transform.position;
        lastFixedRotation = transform.rotation;
        //nextFixedPosition = transform.position;
        nextFixedRotation = transform.rotation;

        jumpFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        //인풋이 없는 경우
        if (!GameManager.Input.isInput)
        {
            planeVelocity = Vector3.zero;
            jumpFlag = false;
            flyFlag = false;
        }

        if (jumpFlag && jumpFlagTime > 0)
        {
            jumpFlagTime -= Time.deltaTime;
            if (jumpFlagTime <= 0)
            {
                jumpFlag = false;
            }
        }

        //날고 있는 상태
        if (isFly)
        {
            flyActiveTime -= Time.deltaTime;
            if (flyActiveTime <= 0)
                isCanFly = false;
        }

        float interpolationAlpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        //cc.Move(Vector3.Lerp(lastFixedPosition, nextFixedPosition, interpolationAlpha) - transform.position);
        transform.rotation = Quaternion.Slerp(lastFixedRotation, nextFixedRotation, interpolationAlpha);
    }

    private void FixedUpdate()
    {
        if (isBreathAttack)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        if (isHit)
        {
            hitTime -= Time.fixedDeltaTime;
            if (hitTime <= 0)
            {
                isHit = false;
                planeVelocity = Vector3.zero;
            }
            else
            {
                planeVelocity = hitDir * playerData.hitPower;
            }
        }

        //lastFixedPosition = nextFixedPosition;
        lastFixedRotation = nextFixedRotation;

        float yVelocity = GetYVelocity();
        velocity = new Vector3(planeVelocity.x, planeVelocity.y + yVelocity, planeVelocity.z);

        if (planeVelocity != Vector3.zero && !isHit)
            nextFixedRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(planeVelocity.x, 0, planeVelocity.z)), playerData.rotateSpeed * Time.fixedDeltaTime);
        //nextFixedPosition += velocity * Time.fixedDeltaTime;

        rb.velocity = velocity;
    }

    /// <remarks>
    /// This function must be called only in FixedUpdate()
    /// </remarks>
    private float GetYVelocity()
    {
        if (gc.IsGrounded())     //땅인 경우
        {
            if (isFly)
            {
                isFly = false;
                StartCoroutine(BreathAttackCoroutine());
            }

            isJump = false;
            isCanFly = true;
            if (jumpFlag)
            {
                //시작 y높이 
                isJump = true;
                jumpFlagTime = playerData.jumpFlagTime;
                return playerData.jumpPower;
            }

            return Mathf.Max(0.0f, velocity.y);
        }
        else                    //땅이 아닌 경우
        {
            if (jumpFlag)
            {
                return playerData.jumpPower;
            }
            if (flyFlag && isCanFly)
            {
                if (!isFly)
                {
                    isFly = true;
                    flyActiveTime = playerData.flyTime;
                    return playerData.flyPower;
                }
                else
                {
                    flyActionDelay -= Time.deltaTime;
                    if (flyActionDelay <= 0)
                    {
                        flyActionDelay = playerData.flyActionDelay;
                        return playerData.flyPower;
                    }
                }
            }

            float fallSpeed = velocity.y + playerData.gravity * Time.fixedDeltaTime;
            if (isFly && fallSpeed < playerData.maxFlyFallSpeed)
                fallSpeed = playerData.maxFlyFallSpeed;
            else if (fallSpeed < playerData.maxFallSpeed)
                fallSpeed = playerData.maxFallSpeed;
            return fallSpeed;
        }
    }

    public void keyMove()
    {
        if (isHit) return;

        float h = Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow) ? 0 : Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
        float v = Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow) ? 0 : Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;

        Vector3 dir = new Vector3(h, 0, v);
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        if (gc.IsGrounded() && gc.IsOnSlope())
        {
            dir = gc.AdjustDirectionToSlope(dir);
            if (dir.y > 0) dir.y = 0;
            dir.Normalize();
        }
        else
        {
            dir.Normalize();
        }

        planeVelocity = dir * playerData.speed * GetMoveSpeedRatio();

        ////향하는 방향으로 균일한 속도로 회전
        //if (dir != Vector3.zero)
        //    transform.forward = Vector3.RotateTowards(transform.forward, dir, playerData.rotateSpeed * Time.deltaTime, 0f);

        if (Input.GetKeyDown(KeyCode.X) && !PlayerManager.Instance.PlayerMouth.IsSuction)
        {
            //땅인경우 점프
            if (gc.IsGrounded()) jumpFlag = true;
            //입에 아무것도 없는 경우 날기가능
            else if (PlayerManager.Instance.PlayerMouth.Stack == PlayerMouth.MouthStack.None)
            {
                flyFlag = true;
                flyActionDelay = 0;
            }
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            jumpFlag = false;
            flyFlag = false;
        }

        //날고있는 경우
        if (isFly)
        {
            if (Input.GetKeyDown(KeyCode.Z))
                StartCoroutine(BreathAttackCoroutine());
        }
    }

    float GetMoveSpeedRatio()
    {
        if (PlayerManager.Instance.changeType != PlayerManager.ChangeType.Normal)
        {
            if (PlayerManager.Instance.PlayerActionManager.GetCurAction().IsAction) return 0.3f;
            if (PlayerManager.Instance.PlayerActionManager.GetCurAction().IsHardAction) return 0f;
        }
        if (PlayerManager.Instance.PlayerMouth.IsSuction) return 0.3f;
        if (isFly) return 0.5f;
        return 1f;
    }

    IEnumerator BreathAttackCoroutine()
    {
        isBreathAttack = true;
        breathAttackDelay = 0.2f;
        //탄환 생성
        Instantiate(breathFactory, transform.position, Quaternion.identity).GetComponent<PlayerBreath>().Set(transform.forward);

        while (breathAttackDelay > 0f)
        {
            breathAttackDelay -= Time.deltaTime;
            yield return null;
        }
        isFly = false;
        isBreathAttack = false;
    }

    public void Hit(Vector3 hitDir)
    {
        isHit = true;
        hitTime = playerData.hitTime;
        velocity = Vector3.zero;
        this.hitDir = hitDir;
    }
}