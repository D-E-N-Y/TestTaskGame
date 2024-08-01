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
    private float health;
    private bool isDeath;
    private Camera _cam;

    // паметры атаки
    [SerializeField] private float shootSpeed;
    private float shootTime;
    private bool isCanShoot;
    private bool isFindEnemy;
    
    // список врагов, которые можно атаковать
    private List<GameObject> targets = new List<GameObject>();
    
    // параметры пули
    [SerializeField] private GameObject ammo;
    [SerializeField] private Transform shootPosition;
    
    
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

        UpdateHealthBar();
    }

    private void Update() 
    {
        if(isDeath) return;
        
        RotateHealthBar();
        
        if(!isCanShoot) return;

        // скорость стрельбы игрока
        shootTime += Time.deltaTime;

        if(shootTime >= shootSpeed && isFindEnemy)
        {
            shootTime = 0;

            Shoot();
        }
    }

    private void FixedUpdate() 
    {
        if(isDeath) return;
        
        Move();
    }

    // метод атаки
    private void Shoot()
    {
        if(!targets[0]) return;

        Vector3 Direction = targets[0].transform.position - transform.position;
        Direction.y = 0;

        transform.rotation = Quaternion.LookRotation(Direction);
        
        Instantiate(ammo, shootPosition.position, shootPosition.rotation);
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

    
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Enemy")
        {
            if(targets.Count == 0) isFindEnemy = true;
            
            targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == "Enemy")
        {
            RemoveTarget(other.gameObject);
        }
    }

    public void RemoveTarget(GameObject target)
    {
        targets.Remove(target);

        if(targets.Count == 0) isFindEnemy = false;
    }
}
