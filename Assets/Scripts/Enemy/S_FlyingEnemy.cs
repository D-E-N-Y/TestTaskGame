using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class S_FlyingEnemy : S_BaseEnemy
{
    private float height = 3.75f;
    
    protected override void Move()
    {
        StartCoroutine(MoveCoroutine());
    }

    protected override void Start()
    {
        base.Start();

        _agent.baseOffset = height; // Устанавливаем высоту
        _agent.avoidancePriority = 50;
    }

    private IEnumerator MoveCoroutine()
    {
        StartMoving();
        
        Vector3 randomDirection = Random.insideUnitSphere * moveRange;
        randomDirection += transform.position;
        randomDirection.y = height; // Устанавливаем высоту

        // Переопределяем высоту в конечной позиции
        Vector3 targetPosition = new Vector3(randomDirection.x, height, randomDirection.z);

        // Устанавливаем цель для NavMeshAgent
        _agent.SetDestination(targetPosition);

        // Ожидание завершения перемещения
        while (_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance)
        {
            yield return null;
        }

        StopMoving();
    }
}
