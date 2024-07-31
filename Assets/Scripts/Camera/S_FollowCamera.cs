using UnityEngine;

public class S_FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed;

    private void FixedUpdate() 
    {
        if(!target) return;

        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, 17f, target.position.z - 10f), followSpeed * Time.fixedDeltaTime);   
    }
}
