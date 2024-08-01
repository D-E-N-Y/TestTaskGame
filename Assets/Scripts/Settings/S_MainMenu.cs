using UnityEngine;
using UnityEngine.SceneManagement;

public class S_MainMenu : MonoBehaviour
{
    public void StartButtonPressed()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
    }
}
