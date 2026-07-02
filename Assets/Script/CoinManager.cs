using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public int coins;

    CoinUI coinUI;

    void Awake()
    {
        // singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // load coin
        coins = PlayerPrefs.GetInt(
            "CoinCount",
            0
        );

        // cari UI
        coinUI = FindAnyObjectByType<CoinUI>();

        UpdateCoinUI();
    }

    public void AddCoins(int amount)
    {
        coins += amount;

        SaveCoins();

        UpdateCoinUI();
    }

    public void RemoveCoins(int amount)
    {
        coins -= amount;

        if (coins < 0)
        {
            coins = 0;
        }

        SaveCoins();

        UpdateCoinUI();

        Debug.Log("Coin sekarang: " + coins);
    }

    void SaveCoins()
    {
        PlayerPrefs.SetInt(
            "CoinCount",
            coins
        );

        PlayerPrefs.Save();
    }

    public void UpdateCoinUI()
    {
        // cari ulang kalau pindah scene
        if (coinUI == null)
        {
            coinUI =
                FindAnyObjectByType<CoinUI>();
        }

        if (coinUI != null)
        {
            coinUI.UpdateCoinText(coins);
        }
    }
}