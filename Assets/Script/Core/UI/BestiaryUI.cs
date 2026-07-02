using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BestiaryUI : MonoBehaviour
{
    public static BestiaryUI instance;

    [System.Serializable]
    public class FishSlot
    {
        public FishData fishData;

        public Image image;

        public Button button;
    }

    [Header("Fish Slots")]
    public FishSlot[] fishSlots;

    public Sprite unknownSprite;

    [Header("Detail Panel")]
    public GameObject detailPanel;

    public Image detailImage;

    public TMP_Text detailName;

    public TMP_Text detailDescription;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        RefreshUI();

        detailPanel.SetActive(false);
    }

    public void RefreshUI()
    {
        foreach (FishSlot slot in fishSlots)
        {
            bool discovered =
                BestiaryManager.instance
                .IsDiscovered(slot.fishData);

            if (discovered)
            {
                slot.image.sprite =
                    slot.fishData.icon;

                slot.button.interactable = true;

                slot.button.onClick.RemoveAllListeners();

                slot.button.onClick.AddListener(() =>
                {
                    ShowDetail(slot.fishData);
                });
            }
            else
            {
                slot.image.sprite =
                    unknownSprite;

                slot.button.interactable = false;
            }
        }
    }

    public void ShowDetail(FishData fish)
    {
        detailPanel.SetActive(true);

        detailImage.sprite = fish.icon;

        detailName.text = fish.fishName;

        detailDescription.text =
            fish.description;
    }

}
