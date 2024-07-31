using UnityEngine;

public class S_Ammo : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update() 
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) 
    {
        string tag = other.gameObject.tag;

        if(tag == "Barrier" || tag == "Enemy")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
