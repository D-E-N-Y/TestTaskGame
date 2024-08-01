using UnityEngine;
using UnityEngine.AI;

public class S_GroundEnemy : S_BaseEnemy
{
    protected override Vector3 FindMovePosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * moveRange;
        randomDirection += transform.position;
        NavMeshHit hit;
        
        NavMesh.SamplePosition(randomDirection, out hit, moveRange, 1);
        Vector3 finalPosition = hit.position;

        return finalPosition;
    }
}
