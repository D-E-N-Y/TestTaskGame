using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class S_GroundEnemy : S_BaseEnemy
{
    protected override void Move()
    {
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
{
    StartMoving();
    
    Vector3 randomDirection = Random.insideUnitSphere * moveRange;
    randomDirection += transform.position;
    NavMeshHit hit;
    NavMesh.SamplePosition(randomDirection, out hit, moveRange, 1);
    Vector3 finalPosition = hit.position;

    _agent.SetDestination(finalPosition);

    // Ожидание завершения перемещения
    while (_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance)
    {
        yield return null;
    }

    StopMoving();
}
}
