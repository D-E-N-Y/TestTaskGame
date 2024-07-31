using System.Collections.Generic;
using UnityEngine;

public class S_Player : MonoBehaviour
{
    [SerializeField] private float speedMove;
    private Rigidbody _rb;

    [SerializeField] private float maxHealth;
    private float health;

    [SerializeField] private float shootSpeed;
    private float shootTime;
    private bool isCanShoot;
    private bool isFindEnemy;

    private List<GameObject> targets = new List<GameObject>();
    private GameObject  currentTarget;

    [SerializeField] private GameObject ammo;
    [SerializeField] private Transform shootPosition;
    
    [SerializeField] private FixedJoystick _joystick;
    private Animator _anim;

    

    private void Start() 
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        
        health = maxHealth;

        shootTime = shootSpeed;
    }

    private void Update() 
    {
        shootTime += Time.deltaTime;

        if(shootTime >= shootSpeed && isCanShoot && isFindEnemy)
        {
            shootTime = 0;

            Shoot();
        }
    }

    private void FixedUpdate() 
    {
        Move();
    }

    private void Shoot()
    {
        // if(!targets[0]) return;

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

    private void GetDamage()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Enemy")
        {
            if(targets.Count == 0) isFindEnemy = true;
            
            targets.Add(other.gameObject);

            currentTarget = targets[0];
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == "Enemy")
        {
            targets.Remove(other.gameObject);

            if(targets.Count == 0) isFindEnemy = false;
            else currentTarget = targets[0];
        }
    }
}
