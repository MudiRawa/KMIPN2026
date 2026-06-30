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
        oxygenText.text = Mathf.RoundToInt(player.oxygen) + "/" + Mathf.RoundToInt(player.maxOxygen);
    }
}
