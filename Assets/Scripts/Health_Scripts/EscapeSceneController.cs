using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeSceneController : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Main"); // Replace "Main" with the name of your main game scene
    }
}
