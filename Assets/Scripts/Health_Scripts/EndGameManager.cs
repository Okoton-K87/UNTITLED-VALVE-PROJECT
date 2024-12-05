using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Main"); // Replace "Main" with the name of your game scene
    }
}
