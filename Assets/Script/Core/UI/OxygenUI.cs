using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OxygenUI : MonoBehaviour
{
    public PlayerMovement player;
    public Slider oxygenSlider;
    public TextMeshProUGUI oxygenText;

    void Start()
    {
        if (player == null)
        {
            player = PlayerMovement.instance;
        }

        if (player != null && oxygenSlider != null)
        {
            oxygenSlider.minValue = 0f;
            oxygenSlider.maxValue = player.maxOxygen;
        }
    }

    void Update()
    {
        if (player == null || oxygenSlider == null || oxygenText == null)
        {
            return;
        }

        oxygenSlider.value = player.oxygen;
        float percent = player.maxOxygen > 0f ? (player.oxygen / player.maxOxygen) * 100f : 0f;
        oxygenText.text = Mathf.RoundToInt(percent) + "%";
    }
}
