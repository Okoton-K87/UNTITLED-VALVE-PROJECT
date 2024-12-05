using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private void Start()
    {
        // Unlock and show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
