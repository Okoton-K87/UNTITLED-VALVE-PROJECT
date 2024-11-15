using UnityEngine;
using TMPro;

public class PickupItem : MonoBehaviour
{
    public string itemName = "Item"; // Set this in Inspector
    public TextMeshProUGUI pickupMessage; // Reference to PickupMessageText
    private bool isInRange = false;

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
            InventorySystem.Instance.AddItem(itemName);
            Destroy(gameObject); // Destroy item after pickup
            Debug.Log($"{itemName} removed from scene.");
        }
    }
}
