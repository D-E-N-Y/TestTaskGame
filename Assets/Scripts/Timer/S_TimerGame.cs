using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class S_TimerGame : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    [SerializeField] private Text timerText;
    private int timer;

    private void Start() 
    {
        timer = 3;

        StartCoroutine(nameof(Timer));
    }

    private IEnumerator Timer()
    {
        while(timer != 0)
        {
            timerText.text = timer.ToString();

            yield return new WaitForSeconds(1f);

            timer--;
        }
        
        timerText.gameObject.SetActive(false);

        SetActiveGame();
    }

    private void SetActiveGame()
    {
        foreach(var obj in objects)
        {
            obj.SetActive(true);
        }
    }
}
