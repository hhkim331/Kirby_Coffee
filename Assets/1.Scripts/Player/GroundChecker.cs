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
            return true;
        else
            return false;
    }

}