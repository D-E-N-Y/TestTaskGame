using UnityEngine;

public class S_SpawnEnemy : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject[] enemies;

    private void Start() 
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        if(spawnPositions.Length == 0 || enemies.Length == 0) return;
        
        foreach(var spawnPoint in spawnPositions)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPoint.position, Quaternion.identity);
        }
    }    
}
