using UnityEngine;

public class S_Victory : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player" && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
