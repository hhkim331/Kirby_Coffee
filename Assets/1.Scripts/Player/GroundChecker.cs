using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Header("Boxcast Property")]
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private LayerMask groundLayer;

    public bool IsGrounded()
    {
        //바닥체크
        var colliders = Physics.OverlapBox(transform.position, boxSize, transform.rotation, groundLayer);
        if (colliders.Length > 0)
        {
            foreach(var collider in colliders)
            {
                psw_Demap map = collider.GetComponent<psw_Demap>();
                if (map!=null)
                {
                    map.BreakGround();
                }
            }
            return true;
        }
        else
            return false;
    }

}