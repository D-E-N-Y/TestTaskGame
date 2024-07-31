using UnityEngine;

public class S_Ammo : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    private enum Type
    {
        Player,
        Enemy
    }
    [SerializeField] private Type type;

    private void Update() 
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) 
    {
        string tag = other.gameObject.tag;

        if(tag == "Barrier")
        {
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
