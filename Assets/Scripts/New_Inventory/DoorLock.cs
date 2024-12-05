using UnityEngine;
using TMPro;

public class DoorLock : MonoBehaviour
{
    [Header("Key Settings")]
    public string requiredKeyName = "Green_Key"; // Name of the required key

    [Header("Door References")]
    public GameObject doorClosed; // Reference to Door01 (closed state)
    public GameObject doorOpen;   // Reference to Door01_Open (open state)

    [Header("Player Detection")]
    public float detectionRadius = 2f; // Radius to detect player near the lock
    public Transform playerTransform;  // Reference to the player's transform

    [Header("UI Reference")]
    public TextMeshProUGUI messageUI; // Reference to the TextMeshPro UI for messages

    private bool isPlayerInRange = false; // Is the player near the lock?

    private void Update()
    {
        // Check if the player is near the lock
        if (Vector3.Distance(transform.position, playerTransform.position) <= detectionRadius)
        {
            if (!isPlayerInRange)
            {
                isPlayerInRange = true;

                // Check if the player has the key
                if (InventoryManager.Instance.InventoryContains(requiredKeyName))
                {
                    ShowMessage($"Press E to unlock the door using {requiredKeyName}.");
                }
                else
                {
                    ShowMessage($"You need the {requiredKeyName} to unlock this door.");
                }
            }

            // Handle unlocking
            if (Input.GetKeyDown(KeyCode.E) && InventoryManager.Instance.InventoryContains(requiredKeyName))
            {
                UnlockDoor();
            }
        }
        else
        {
            if (isPlayerInRange)
            {
                isPlayerInRange = false;
                HideMessage();
            }
        }
    }

    private void ShowMessage(string message)
    {
        if (messageUI != null)
        {
            messageUI.text = message;
            messageUI.gameObject.SetActive(true);
        }
    }

    private void HideMessage()
    {
        if (messageUI != null)
        {
            messageUI.gameObject.SetActive(false);
        }
    }

    private void UnlockDoor()
    {
        // Remove the key from the inventory
        InventoryManager.Instance.RemoveItemByName(requiredKeyName);

        // Toggle door states
        if (doorClosed != null) doorClosed.SetActive(false);
        if (doorOpen != null) doorOpen.SetActive(true);

        ShowMessage("Door unlocked!");
        Debug.Log("Door unlocked!");
    }
}
