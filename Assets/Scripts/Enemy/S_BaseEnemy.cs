using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class S_BaseEnemy : MonoBehaviour
{
    //Health
    [SerializeField] private float maxHealth;
    private float health;
    private bool isDeath;
    
    //State
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float moveRange;
    [SerializeField] private float idleTime;
    private bool isBehavior, isMoving;
    
    //Attack
    [SerializeField] private float attackSpeed;
    private float attackTimer;
    [SerializeField] private float attackRange;
    private bool isPlayerInAttackRange;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform attackPosition;
    
    [SerializeField] private LayerMask _lm;
    private Animator _anim;
    protected NavMeshAgent _agent;
    private Transform player;
    
    protected virtual void Start() 
    {
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        player = S_Player.instance.transform;
        
        attackTimer = attackSpeed;
        health = maxHealth;

        isDeath = false;
        isBehavior = true;
        isMoving = false;

        StartCoroutine(nameof(Behavior));
    }

    private void Update() 
    {
        if(isDeath) return;
        
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, _lm);

        if(isPlayerInAttackRange && !isMoving)
        {
            isBehavior = false;
            StopCoroutine(nameof(Behavior));

            Attack();
        }
        else if(!isBehavior)
        {
            isBehavior = true;
            StartCoroutine(nameof(Behavior));
        }
    }

    private IEnumerator Behavior()
    {
        while(!isDeath)
        {
            yield return new WaitForSeconds(idleTime);
            
            isMoving = true;
            
            Move();

            isMoving = false;
            // _anim.SetBool("Run", isMoving);
        }
    }
    
    protected abstract void Move();

    protected void StartMoving()
    {
        isMoving = true;
    }

    protected void StopMoving()
    {
        isMoving = false;
    }

    private void Attack()
    {
        _agent.SetDestination(transform.position);

        transform.LookAt(player);

        // attackPosition.transform.LookAt(player);

        attackTimer += Time.deltaTime;

        if(attackTimer >= attackSpeed)
        {
            attackTimer = 0;
            
            Instantiate(bullet, attackPosition.position, attackPosition.rotation);
        }
    }

    
    public void GetDamage(float damage)
    {
        health -= damage;

        if(health < 0) Death();
    }

    private void Death()
    {
        isDeath = true;
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
