using UnityEngine;
using UnityEngine.UI;

public class S_Money : MonoBehaviour
{
    [SerializeField] private Text countCoinText;

    private void Start() 
    {
        if(PlayerPrefs.HasKey("Coins"))
        {
            UpdateText();
        }
        else
        {
            PlayerPrefs.SetInt("Coins", 0);
            UpdateText();
        }
    }

    public void AddCoin()
    {
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1);
        UpdateText();
    }

    private void UpdateText()
    {
        countCoinText.text = PlayerPrefs.GetInt("Coins").ToString();
    }
}
