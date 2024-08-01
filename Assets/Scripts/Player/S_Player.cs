using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_Player : MonoBehaviour
{
    // парметры движения
    [SerializeField] private float speedMove;
    private Rigidbody _rb;
    [SerializeField] private FixedJoystick _joystick;

    // параметры жизни
    [SerializeField] private float maxHealth;
    [SerializeField] private Slider hp_bar;
    [SerializeField] private GameObject _hitParticle;
    [SerializeField] private GameObject losePanel;
    private float health;
    private bool isDeath;
    private Camera _cam;

    // паметры атаки
    [SerializeField] private float shootSpeed;
    [SerializeField] private float shootRange;
    [SerializeField] private LayerMask enemy_lm;
    private float shootTime;
    private bool isCanShoot;
    private bool isFindEnemy;
    
    // список врагов, которые можно атаковать
    private List<GameObject> targets = new List<GameObject>();
    private GameObject currentTarget;
    
    // параметры пули
    [SerializeField] private GameObject ammo;
    [SerializeField] private Transform shootPosition;
    private Vector3 startSP;
    private Quaternion startSR;
    
    
    private Animator _anim;

    public static S_Player instance;

    private void Awake() 
    {
        instance = this;
    }

    private void Start() 
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _cam = Camera.main;
        
        health = maxHealth;
        isDeath = false;

        shootTime = shootSpeed;
        startSP = shootPosition.localPosition;
        startSR = shootPosition.localRotation;

        UpdateHealthBar();
    }

    private void Update() 
    {
        if(isDeath) return;
        
        RotateHealthBar();
        
        if(!isCanShoot) return;

        isFindEnemy = Physics.CheckSphere(transform.position, shootRange, enemy_lm);

        if(isFindEnemy)
        {
            CheckForEnemies();
            
            _anim.SetBool("Shoot", true);
            Attack();
        }
        else
        {
            _anim.SetBool("Shoot", false);
        }
    }

    private void FixedUpdate() 
    {
        if(isDeath) return;
        
        Move();
    }

    private void CheckForEnemies()
    {
        targets.Clear();
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, shootRange, enemy_lm);
        
        foreach (var unit in hitColliders)
        {
            if(unit.gameObject.tag == "Enemy")
            {
                targets.Add(unit.gameObject);
            }
        }

        FindNearestTarget();
    }

    private void FindNearestTarget()
    {
        currentTarget = targets[0];

        foreach (var unit in targets)
        {
            if(Vector3.Distance(currentTarget.transform.position, transform.position) > Vector3.Distance(unit.transform.position, transform.position))
            {
                currentTarget = unit;
            }
        }
    }

    // метод атаки
    private void Attack()
    {
        if(!currentTarget) return;

        // скорость стрельбы игрока
        shootTime += Time.deltaTime;
        
        if(shootTime >= shootSpeed)
        {
            shootTime = 0;

            Vector3 direction = currentTarget.transform.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
            
            direction = currentTarget.transform.position - shootPosition.position;

            // Если угол больше 0, позиция стрельбы смотрит на врага
            if (direction.y > 0)
            {
                shootPosition.LookAt(currentTarget.transform);
            }
            else
            {
                shootPosition.localPosition = startSP;
                shootPosition.localRotation = startSR;
            }
            
            Instantiate(ammo, shootPosition.position, shootPosition.rotation);
        }
    }

    // метод движения
    private void Move()
    {
        _rb.velocity = new Vector3(_joystick.Horizontal * speedMove, _rb.velocity.y, _joystick.Vertical * speedMove);

        if(_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(_rb.velocity);

            _anim.SetBool("Run", true);
            isCanShoot = false;
        }
        else
        {
            _anim.SetBool("Run", false);
            isCanShoot = true;
        }
    }

    // метод получение урона
    public void GetDamage(float damage)
    {
        health -= damage;
        
        Destroy(Instantiate(_hitParticle, transform.position, _hitParticle.transform.rotation), 1f);

        UpdateHealthBar();
    
        if(health < 0) Death();
    }

    // метод смерти
    private void Death()
    {
        isDeath = true;

        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // метод вращения полоски хп в сторону камеры
    private void RotateHealthBar()
    {
        hp_bar.transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }
    
    // обновление значения в полоске хп
    private void UpdateHealthBar()
    {
        hp_bar.value = health / maxHealth;
    }

    // отрисовка зоны атаки
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}