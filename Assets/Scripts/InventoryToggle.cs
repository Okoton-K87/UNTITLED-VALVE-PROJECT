using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject player; // Reference to the player object to disable movement
    public PlayerController playerMovementScript; // Reference to the player's movement script
    public PlayerController cameraControlScript; // Reference to the camera control script

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool isActive = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(isActive);

            // Enable/Disable player controls and lock/unlock the cursor
            if (isActive)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                playerMovementScript.enabled = false;
                cameraControlScript.enabled = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                playerMovementScript.enabled = true;
                cameraControlScript.enabled = true;
            }
        }
    }
}
