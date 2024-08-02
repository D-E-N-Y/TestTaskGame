using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class S_BaseEnemy : MonoBehaviour
{
    // параметры здоровья
    [SerializeField] private float maxHealth;
    [SerializeField] private S_HealthBar _healthBar;
    [SerializeField] private GameObject _hitDamage;
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
    
    // параметры звуков
    [SerializeField] AudioClip shoot, die;
    private AudioSource _audio;

    [SerializeField] private LayerMask _lm;
    private Camera _cam;
    private Animator _anim;
    protected NavMeshAgent _agent;
    private Transform player;
    
    protected virtual void Start() 
    {
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _healthBar = GetComponentInChildren<S_HealthBar>();
        _cam = Camera.main;
        _audio = GetComponent<AudioSource>();
        player = S_Player.instance.transform;
        
        attackTimer = attackSpeed;
        health = maxHealth;

        isDeath = false;
        isBehavior = true;
        isMoving = false;

        _healthBar.UpdateHealthBar(health, maxHealth);

        // в начале игры запускаем обычное поведение врага
        StartCoroutine(nameof(Behavior));
    }

    private void Update() 
    {
        if(isDeath) return;

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

            if(_anim) _anim.SetBool("Run", true);
            
            _agent.SetDestination(FindMovePosition());
            
            float moveStartTime = Time.time;

            // Ожидание завершения перемещения
            while (_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance)
            {
                if (Time.time - moveStartTime > 5f)
                {
                    // Прекращение движения, если истекло максимальное время
                    _agent.SetDestination(transform.position);
                    break;
                }
                yield return null;
            }

            if(_anim) _anim.SetBool("Run", false);

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
            
            _audio.Stop();
            _audio.clip = shoot;
            _audio.Play();
            
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
    
    // получание урона
    public void GetDamage(float damage)
    {
        health -= damage;

        Destroy(Instantiate(_hitDamage, transform.position, Quaternion.identity), 0.5f);

        _healthBar.UpdateHealthBar(health, maxHealth);

        if(health < 0) Death();
    }

    // метод смерти
    private void Death()
    {
        PlayDeathSound();
        
        isDeath = true;
        _cam.GetComponent<S_Money>().AddCoin();

        Destroy(gameObject);
    }

    private void PlayDeathSound()
    {
        // Создаем временный объект для проигрывания звука
        GameObject tempAudioSource = new GameObject("TempAudio");
        AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();

        // Настраиваем AudioSource
        audioSource.clip = die;
        audioSource.volume = 0.5f;
        audioSource.Play();

        // Уничтожаем временный объект после завершения проигрывания звука
        Destroy(tempAudioSource, die.length);
    }

    // отрисовка зоны атаки
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}