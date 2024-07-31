using System.Collections;
using UnityEngine;

public abstract class S_BaseEnemy : MonoBehaviour
{
    public float speed;
    public float moveDistance;
    public float idleTime;
    public int hp;
    public float fireRate;
    public int damage;

    private bool isMoving;
    private bool isIdle;
    private Vector3 targetPosition;
    private float nextFireTime;

    protected virtual void Start()
    {
        isMoving = false;
        isIdle = true;
        StartCoroutine(BehaviorRoutine());
    }

    protected abstract Vector3 FindNewPosition();

    protected virtual void Update()
    {
        if (isMoving)
        {
            MoveToPosition();
        }
        else if (!isIdle && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void MoveToPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
            isIdle = true;
            StartCoroutine(IdleRoutine());
        }
    }

    private IEnumerator IdleRoutine()
    {
        yield return new WaitForSeconds(idleTime);
        isIdle = false;
        targetPosition = FindNewPosition();
        isMoving = true;
    }

    protected abstract void Fire();

    public void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator BehaviorRoutine()
    {
        while (true)
        {
            if (!isMoving && !isIdle)
            {
                targetPosition = FindNewPosition();
                isMoving = true;
            }
            yield return null;
        }
    }
}
