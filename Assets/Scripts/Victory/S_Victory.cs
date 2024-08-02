using UnityEngine;

public class S_Victory : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    private AudioSource victorySound;
    
    private void Start() 
    {
        victorySound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player" && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            victorySound.Play();
            victoryPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
