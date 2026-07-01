using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    public TextMeshProUGUI CoinText;

    private int currentCoins;

    public static CoinUI instance;

    void Start()
    {
        instance = this;
        currentCoins = PlayerPrefs.GetInt("CoinCount", 0);
        UpdateCoinUI();
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;

        PlayerPrefs.SetInt("CoinCount", currentCoins);
        PlayerPrefs.Save();

        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        CoinText.text = "Coins: " + currentCoins.ToString();
    }
}