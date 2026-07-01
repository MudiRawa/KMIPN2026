using TMPro;
using UnityEngine;

public class BestiaryUI : MonoBehaviour
{
    public static BestiaryUI instance;

    public TMP_Text textUI;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        textUI.text = "BESTIARY\n\n";

        foreach (string fishName in BestiaryManager.instance.GetDiscoveredFishNames())
        {
            textUI.text += "- " + fishName + "\n";
        }
    }

}
