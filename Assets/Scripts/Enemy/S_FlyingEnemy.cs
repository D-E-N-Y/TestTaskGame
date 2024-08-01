using UnityEngine;

public class S_FlyingEnemy : S_BaseEnemy
{
    private float height = 3.75f;

    protected override void Start()
    {
        base.Start();

        Physics.IgnoreLayerCollision(10, 11);

        _agent.baseOffset = height;
        _agent.avoidancePriority = 50;
    }

    protected override Vector3 FindMovePosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * moveRange;
        randomDirection += transform.position;
        randomDirection.y = height;

        // Переопределяем высоту в конечной позиции
        Vector3 targetPosition = new Vector3(randomDirection.x, height, randomDirection.z);

        return targetPosition;
    }
}
