using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_Player : MonoBehaviour
{
    [SerializeField] private float speedMove;
    private Rigidbody _rb;

    [SerializeField] private float maxHealth;
    [SerializeField] private Slider hp_bar;
    [SerializeField] private GameObject _hitParticle;
    private float health;
    private bool isDeath;
    private Camera _cam;

    [SerializeField] private float shootSpeed;
    private float shootTime;
    private bool isCanShoot;
    private bool isFindEnemy;

    private List<GameObject> targets = new List<GameObject>();

    [SerializeField] private GameObject ammo;
    [SerializeField] private Transform shootPosition;
    
    [SerializeField] private FixedJoystick _joystick;
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
        
        shootTime += Time.deltaTime;

        if(shootTime >= shootSpeed && isCanShoot && isFindEnemy)
        {
            shootTime = 0;

            Shoot();
        }

        if(Input.GetKeyDown(KeyCode.D)) GetDamage(10f);
    }

    private void FixedUpdate() 
    {
        if(isDeath) return;
        
        Move();
    }

    private void Shoot()
    {
        if(!targets[0]) return;

        Vector3 Direction = targets[0].transform.position - transform.position;
        Direction.y = 0;

        transform.rotation = Quaternion.LookRotation(Direction);
        
        Instantiate(ammo, shootPosition.position, shootPosition.rotation);
    }

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

    public void GetDamage(float damage)
    {
        health -= damage;
        
        Destroy(Instantiate(_hitParticle, transform.position, _hitParticle.transform.rotation), 1f);

        UpdateHealthBar();
    
        if(health < 0) Death();
    }

    private void Death()
    {
        isDeath = true;
    }

    private void RotateHealthBar()
    {
        hp_bar.transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }

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
