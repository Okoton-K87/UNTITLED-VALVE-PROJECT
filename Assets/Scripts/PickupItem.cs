using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PickupItem : MonoBehaviour
{
    public string itemName = "Item"; // Set this in Inspector
    public TextMeshProUGUI pickupMessage; // Reference to PickupMessageText
    private bool isInRange = false;
    public GameObject InventoryPanel;

    private void Start()
    {
        if (pickupMessage != null)
        {
            pickupMessage.gameObject.SetActive(false); // Hide initially
            Debug.Log("Pickup message hidden on Start.");
        }
        else
        {
            Debug.LogWarning("Pickup message reference is missing!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger zone.");
            isInRange = true;

            if (pickupMessage != null)
            {
                pickupMessage.gameObject.SetActive(true);
                pickupMessage.text = $"Press F to pick up {itemName}";
                Debug.Log("Pickup message displayed with item name.");
            }
            else
            {
                Debug.LogWarning("Pickup message reference is not assigned in the Inspector.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger zone.");
            isInRange = false;

            if (pickupMessage != null)
            {
                pickupMessage.gameObject.SetActive(false);
                Debug.Log("Pickup message hidden.");
            }
        }
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log($"Player picked up {itemName}.");

            // Add the item to the inventory system
            InventorySystem.Instance.AddItem(itemName);

            // Hide the pickup message
            if (pickupMessage != null)
            {
                pickupMessage.gameObject.SetActive(false);
                Debug.Log("Pickup message hidden after pickup.");
            }

            // Handle specific actions for the M1A1 gun
            if (this.name == "M1A1_thompson")
            {
                // Find the button by name or reference it in the Inspector
                GameObject gunOneButton = InventoryPanel.transform.Find("GunOneButton").gameObject;

                // Enable the button in the inventory
                if (gunOneButton != null)
                {
                    gunOneButton.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("GunOneButton not found in InventoryPanel.");
                }
            }

            // Destroy the item after pickup
            Destroy(gameObject);
            Debug.Log($"{itemName} removed from scene.");
        }
    }
}
