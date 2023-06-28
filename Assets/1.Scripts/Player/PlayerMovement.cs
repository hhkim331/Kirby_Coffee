using System.Globalization;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerData playerData;

    bool jumpFlag;
    bool isFly = false;
    bool isCanFly = true;
    float flyActiveTime;

    CharacterController cc;
    GroundChecker gc;
    Vector3 velocity;
    Vector3 planeVelocity;  //xz plane velocity
    Vector3 lastFixedPosition;
    Quaternion lastFixedRotation;
    Vector3 nextFixedPosition;
    Quaternion nextFixedRotation;

    public void Set(PlayerManager manager, PlayerData data)
    {
        playerManager = manager;
        playerData = data;
        cc = GetComponent<CharacterController>();
        gc = GetComponent<GroundChecker>();

        velocity = new Vector3(0, 0, 0);
        lastFixedPosition = transform.position;
        lastFixedRotation = transform.rotation;
        nextFixedPosition = transform.position;
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
        }

        //날고 있는 상태
        if (isFly)
        {
            flyActiveTime -= Time.deltaTime;
            if (flyActiveTime <= 0)
                isCanFly = false;
        }

        float interpolationAlpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        cc.Move(Vector3.Lerp(lastFixedPosition, nextFixedPosition, interpolationAlpha) - transform.position);
        transform.rotation = Quaternion.Slerp(lastFixedRotation, nextFixedRotation, interpolationAlpha);
    }

    private void FixedUpdate()
    {
        lastFixedPosition = nextFixedPosition;
        lastFixedRotation = nextFixedRotation;

        float yVelocity = GetYVelocity();
        velocity = new Vector3(planeVelocity.x, yVelocity, planeVelocity.z);

        if (planeVelocity != Vector3.zero)
            nextFixedRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(planeVelocity), playerData.rotateSpeed * Time.fixedDeltaTime);
        nextFixedPosition += velocity * Time.fixedDeltaTime;
    }

    /// <remarks>
    /// This function must be called only in FixedUpdate()
    /// </remarks>
    private float GetYVelocity()
    {
        if (!gc.IsGrounded())
        {
            if(jumpFlag && isCanFly)
            {
                jumpFlag = false;
                if (velocity.y < playerData.flyPower)
                {
                    if (!isFly)
                    {
                        isFly = true;
                        flyActiveTime = playerData.flyTime;
                    }
                    return velocity.y + playerData.flyPower;
                }
            }

            return velocity.y + playerData.gravity * Time.fixedDeltaTime;
        }

        isFly = false;
        isCanFly = true;
        if (jumpFlag)
        {
            jumpFlag = false;
            return velocity.y + playerData.jumpPower;
        }
        else
        {
            return Mathf.Max(0.0f, velocity.y);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.rigidbody)
        {
            hit.rigidbody.AddForce(velocity / hit.rigidbody.mass);
        }
    }

    public void keyMove()
    {
        float h = Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow) ? 0 : Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
        float v = Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow) ? 0 : Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;

        Vector3 dir = new Vector3(h, 0, v);
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        dir.Normalize();
        planeVelocity = dir * playerData.speed;

        ////향하는 방향으로 균일한 속도로 회전
        //if (dir != Vector3.zero)
        //    transform.forward = Vector3.RotateTowards(transform.forward, dir, playerData.rotateSpeed * Time.deltaTime, 0f);

        if (Input.GetKeyDown(KeyCode.X))
            jumpFlag = true;
    }
}