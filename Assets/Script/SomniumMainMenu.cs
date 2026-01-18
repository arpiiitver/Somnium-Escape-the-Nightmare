using UnityEngine;
using UnityEngine.SceneManagement;

public class SomniumMainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }
}
