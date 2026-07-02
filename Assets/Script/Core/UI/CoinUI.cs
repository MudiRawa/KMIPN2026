using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    void Start()
    {
        CoinManager.instance.UpdateCoinUI();
    }

    public void UpdateCoinText(int coins)
    {
        coinText.text = coins.ToString();
    }
}