using UnityEngine;
using UnityEngine.SceneManagement;

public class S_SceneSettings : MonoBehaviour
{
    [SerializeField] private GameObject PausePanel;

    public void PauseButtonPressed()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ContinueButtonPressed()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager .GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void ExitButtonPressed()
    {
        SceneManager.LoadScene(0);
    }
}
