using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class S_BaseEnemy : MonoBehaviour
{
    // параметры здоровья
    [SerializeField] private float maxHealth;
    [SerializeField] private Slider hp_bar;
    private float health;
    private bool isDeath;
    
    // парематры движения и неподвижности
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float moveRange;
    [SerializeField] private float idleTime;
    private bool isBehavior, isMoving;
    
    // параматеры атаки
    [SerializeField] private float meleeDamage;
    [SerializeField] private float attackSpeed;
    private float attackTimer;
    [SerializeField] private float attackRange;
    private bool isPlayerInAttackRange;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform attackPosition;
    
    [SerializeField] private LayerMask _lm;
    private Camera _cam;
    private Animator _anim;
    protected NavMeshAgent _agent;
    private Transform player;
    
    protected virtual void Start() 
    {
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _cam = Camera.main;
        player = S_Player.instance.transform;
        
        attackTimer = attackSpeed;
        health = maxHealth;

        isDeath = false;
        isBehavior = true;
        isMoving = false;

        UpdateHealthBar();

        // в начале игры запускаем обычное поведение врага
        StartCoroutine(nameof(Behavior));
    }

    private void Update() 
    {
        if(isDeath) return;
        
        RotateHealthBar();

        // область атаки противника
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

    // обычное повеление протвника    
    private IEnumerator Behavior()
    {
        while(!isDeath)
        {
            yield return new WaitForSeconds(idleTime);
            
            isMoving = true;
            
            _agent.SetDestination(FindMovePosition());

            // Ожидание завершения перемещения
            while (_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance)
            {
                yield return null;
            }

            isMoving = false;
        }
    }
    
    // метод нахождение позиции для перемещения
    protected abstract Vector3 FindMovePosition();

    // поведения атаки
    private void Attack()
    {
        // останавливаем врага и поворачиваемся в сторону игрока
        _agent.SetDestination(transform.position);
        transform.LookAt(player);

        // запускаем таймер атаки
        attackTimer += Time.deltaTime;

        if(attackTimer >= attackSpeed)
        {
            attackTimer = 0;
            
            Instantiate(bullet, attackPosition.position, attackPosition.rotation);
        }
    }

    // наносение игроку урон если тот подошел слишком близко к врагу
    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<S_Player>().GetDamage(meleeDamage);
        }
    }

    // поворот полоски хп в сторону камеры
    private void RotateHealthBar()
    {
        hp_bar.transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }

    // обновление полоски хп
    private void UpdateHealthBar()
    {
        hp_bar.value = health / maxHealth;
    }
    
    // получание урона
    public void GetDamage(float damage)
    {
        health -= damage;

        UpdateHealthBar();

        if(health < 0) Death();
    }

    // метод смерти
    private void Death()
    {
        isDeath = true;

        Destroy(gameObject);
    }

    // отрисовка зоны атаки
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}