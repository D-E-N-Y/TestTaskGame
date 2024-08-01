using UnityEngine;

public class S_Ammo : MonoBehaviour
{
    // параметры снаряда
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private GameObject hit;

    // паретр, какому юниту пренадлежит снаряд
    private enum Type
    {
        Player,
        Enemy
    }
    [SerializeField] private Type type;

    // перемещение снаряда
    private void Update() 
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        string tag = other.gameObject.tag;

        if(tag == "Barrier" || tag == "Ground")
        {
            Destroy(Instantiate(hit, transform.position, Quaternion.identity), 0.3f);
            
            Destroy(gameObject);
        }

        if(tag == "Enemy" && type == Type.Player)
        {
            other.gameObject.GetComponent<S_BaseEnemy>().GetDamage(damage);
            
            Destroy(gameObject);
        }

        if(tag == "Player" && type == Type.Enemy)
        {
            other.gameObject.GetComponent<S_Player>().GetDamage(damage);

            Destroy(gameObject);
        }
    }
}
