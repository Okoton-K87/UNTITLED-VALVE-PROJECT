using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance; // Singleton instance

    [Header("UI References")]
    public GameObject inventoryPanel; // The inventory panel
    public TextMeshProUGUI pickupMessage; // The pickup message
    public Transform inventoryButtonParent; // Parent for inventory buttons

    [Header("Prefabs")]
    public GameObject inventoryButtonPrefab; // Button prefab for inventory items

    [Header("Player Reference")]
    public Transform playerTransform; // Reference to the player's transform

    private List<Item> inventoryItems = new List<Item>(); // List of items in the inventory
    private Item itemInPickupRange; // The item currently in pickup range

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Hide the pickup message and inventory panel at start
        if (pickupMessage != null) pickupMessage.gameObject.SetActive(false);
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        // Handle item pickup
        if (itemInPickupRange != null && Input.GetKeyDown(KeyCode.F))
        {
            //Debug.Log($"Picking up item: {itemInPickupRange.itemName}");
            AddItem(itemInPickupRange);
            Destroy(itemInPickupRange.gameObject); // Remove item from scene
            ClearItemInRange();
        }
    }
    public void AddItem(Item item)
    {
        inventoryItems.Add(item);
        Debug.Log($"{item.itemName} added to inventory.");

        // Store prefab reference before destroying the item
        GameObject itemPrefab = item.prefabReference;

        // Create a button in the inventory panel
        GameObject button = Instantiate(inventoryButtonPrefab, inventoryButtonParent);

        // Ensure the button is active
        button.SetActive(true);

        // Get the InventoryButton script from the button prefab
        InventoryButton buttonScript = button.GetComponent<InventoryButton>();
        if (buttonScript != null)
        {
            // Initialize the button with the item's name and icon
            buttonScript.Initialize(item.itemName, item.itemIcon);
        }
        else
        {
            Debug.LogWarning("InventoryButton script is missing on the button prefab!");
        }

        // Add drop functionality to the button
        button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            DropItem(itemPrefab, button);
        });

        // Destroy the item in the world
        Destroy(item.gameObject);

        // Log current inventory
        LogInventoryContents();
    }

    public void DropItem(GameObject itemPrefab, GameObject button)
    {
        if (itemPrefab == null)
        {
            Debug.LogWarning("No prefab reference found for the item!");
            return;
        }

        // Ensure we have a valid playerTransform
        if (playerTransform == null)
        {
            Debug.LogError("Player transform not assigned to InventoryManager!");
            return;
        }

        // Instantiate the item in front of the player
        Vector3 dropPosition = playerTransform.position + playerTransform.forward * 2f + Vector3.up * 1f; // Offset in front of the player
        GameObject droppedItem = Instantiate(itemPrefab, dropPosition, Quaternion.identity);

        // Ensure the prefabReference is set on the newly instantiated item
        Item droppedItemScript = droppedItem.GetComponent<Item>();
        if (droppedItemScript != null)
        {
            droppedItemScript.prefabReference = itemPrefab;
            Debug.Log($"Set prefab reference for dropped item: {droppedItemScript.itemName}");

            // Flag for successful removal
            bool itemRemoved = false;

            // Locate and remove the first matching item from the inventory by name
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].itemName == droppedItemScript.itemName)
                {
                    Debug.Log($"Removing item: {inventoryItems[i].itemName}");
                    inventoryItems.RemoveAt(i);
                    itemRemoved = true;
                    break; // Stop after removing the first match
                }
            }

            // Check if the item was removed
            if (itemRemoved)
            {
                Debug.Log($"{droppedItemScript.itemName} successfully removed from inventory.");
            }
            else
            {
                Debug.LogWarning($"Failed to remove item: {droppedItemScript.itemName} (not found in inventory).");
            }
        }
        else
        {
            Debug.LogWarning("Dropped item does not have an Item script!");
        }

        // Remove the button from the inventory UI
        Destroy(button);

        Debug.Log($"Dropped item at {dropPosition}");

        // Log current inventory
        LogInventoryContents();
    }

    public void SetItemInRange(Item item)
    {
        itemInPickupRange = item;

        // Show the pickup message
        if (pickupMessage != null)
        {
            pickupMessage.gameObject.SetActive(true);
            pickupMessage.text = $"Press F to pick up {item.itemName}";
            //Debug.Log($"Pickup message displayed for item: {item.itemName}");
        }
    }

    public void ClearItemInRange()
    {
        itemInPickupRange = null;

        // Hide the pickup message
        if (pickupMessage != null)
        {
            pickupMessage.gameObject.SetActive(false);
            //Debug.Log("Pickup message hidden.");
        }
    }
    public void LogInventoryContents()
    {
        Debug.Log("Current Inventory:");
        if (inventoryItems.Count == 0)
        {
            Debug.Log("Inventory is empty.");
            return;
        }

        foreach (var item in inventoryItems)
        {
            Debug.Log($"- {item.itemName}");
        }
    }

}
