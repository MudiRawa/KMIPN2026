using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shop : MonoBehaviour
{
    public GameObject textE;
    public GameObject panelJual;

    private bool playerInTrigger = false;
    private bool isShopOpen = false;

    private PlayerInput playerInput;

    [Header("Shop UI")]
    public Transform slotParent;

    public GameObject slotPrefab;

    void Start()
    {
        playerInput =
            FindAnyObjectByType<PlayerInput>();
    }

    void Update()
    {
        if (playerInTrigger &&
            playerInput != null)
        {
            if (
                playerInput
                .actions["Interact"]
                .WasPressedThisFrame()
            )
            {
                Interact();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (
            other.CompareTag("Player") &&
            !isShopOpen
        )
        {
            playerInTrigger = true;

            textE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;

            textE.SetActive(false);
        }
    }

    void Interact()
    {
        if (!isShopOpen)
        {
            textE.SetActive(false);

            isShopOpen = true;

            panelJual.SetActive(true);

            PlayerMovement.instance
                .DisableMovement();

            RefreshShop();
        }
    }

    void RefreshShop()
    {
        // INVENTORY SINGLETON
        Inventory inventory =
            Inventory.instance;

        if (inventory == null)
        {
            Debug.LogError(
                "Inventory NULL!"
            );

            return;
        }

        // hapus slot lama
        foreach (Transform child
            in slotParent)
        {
            Destroy(child.gameObject);
        }

        // buat slot baru
        foreach (
            FishInventoryItem fishItem
            in inventory.fishes
        )
        {
            // safety check
            if (
                fishItem == null ||
                fishItem.fishData == null
            )
            {
                Debug.LogError(
                    "Fish item NULL!"
                );

                continue;
            }

            GameObject obj =
                Instantiate(
                    slotPrefab,
                    slotParent
                );

            ShopSlotUI slotUI =
                obj.GetComponent<ShopSlotUI>();

            if (slotUI == null)
            {
                Debug.LogError(
                    "ShopSlotUI tidak ada di prefab!"
                );

                continue;
            }

            slotUI.Setup(fishItem);
        }
    }

    public void SellFish()
    {
        Inventory inventory = Inventory.instance;

        InventoryUI inventoryUI = FindAnyObjectByType<InventoryUI>();

        int totalCoins = 0;

        List<FishInventoryItem> fishesToRemove = new List<FishInventoryItem>();

        foreach (Transform child in slotParent)
        {
            ShopSlotUI slot = child.GetComponent<ShopSlotUI>();

            if (slot != null && slot.selected)
            {
                totalCoins += slot.fishData.sellPrice;
                fishesToRemove.Add(slot.fishItem);
            }
        }

        // hapus ikan
        foreach (FishInventoryItem fish in fishesToRemove)
        {
            inventory.RemoveFish(fish);
        }

        // tambah coin
        CoinManager.instance.AddCoins(totalCoins);

        // refresh UI
        if (inventoryUI != null)
        {
            inventoryUI.UpdateInventoryUI(inventory.fishCount);
        }

        RefreshShop();
    }

    public void CloseShop()
    {
        isShopOpen = false;

        panelJual.SetActive(false);

        PlayerMovement.instance.EnableMovement();
    }
}