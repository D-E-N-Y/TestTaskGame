using UnityEngine;
using UnityEngine.UI;

public class S_BaseEnemy : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float health;
    [SerializeField] private Slider hp_bar;

    [SerializeField] private float speedMove;
    [SerializeField] private float rangeMove;
    [SerializeField] private float timeImmobility;

    [SerializeField] private float speedShoot;
    private float timerShoot;

    private void Start() 
    {
        health = maxHealth;
        
        timerShoot = speedShoot;
    }

    private void Update() 
    {
        timerShoot += Time.deltaTime;

        if(timerShoot >= speedShoot)
        {
            timerShoot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        
    }

    public void GetDamage(float damage)
    {
        health -= damage;

        if(health < 0) Death();
    }

    private void Death()
    {

    }
}
