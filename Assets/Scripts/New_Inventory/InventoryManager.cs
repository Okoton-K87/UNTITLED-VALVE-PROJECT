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
            Debug.Log($"Picking up item: {itemInPickupRange.itemName}");
            AddItem(itemInPickupRange);
            Destroy(itemInPickupRange.gameObject); // Remove item from scene
            ClearItemInRange();
        }
    }

    public void AddItem(Item item)
    {
        inventoryItems.Add(item);
        Debug.Log($"{item.itemName} added to inventory.");

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
            DropItem(item, button);
        });
    }


    public void DropItem(Item item, GameObject button)
    {
        inventoryItems.Remove(item);

        // Instantiate the item in front of the player
        Vector3 dropPosition = transform.position + transform.forward * 2f;
        Instantiate(item.gameObject, dropPosition, Quaternion.identity);

        // Remove the button from the inventory UI
        Destroy(button);
        Debug.Log($"{item.itemName} dropped at {dropPosition}");
    }

    public void SetItemInRange(Item item)
    {
        itemInPickupRange = item;

        // Show the pickup message
        if (pickupMessage != null)
        {
            pickupMessage.gameObject.SetActive(true);
            pickupMessage.text = $"Press F to pick up {item.itemName}";
            Debug.Log($"Pickup message displayed for item: {item.itemName}");
        }
    }

    public void ClearItemInRange()
    {
        itemInPickupRange = null;

        // Hide the pickup message
        if (pickupMessage != null)
        {
            pickupMessage.gameObject.SetActive(false);
            Debug.Log("Pickup message hidden.");
        }
    }
}
