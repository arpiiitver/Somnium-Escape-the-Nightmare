using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float timerDuration = 10f;
    private float timeRemaining;

    // Reference to the TextMeshProUGUI element
    public TextMeshProUGUI timerText;

    // Flag to check if the key has been found
    public bool keyFound = false;

    void Start()
    {
        timeRemaining = timerDuration;
    }

    void Update()
    {
        if (!keyFound)
        {
            timeRemaining -= Time.deltaTime;
        }

        if (timeRemaining < 0)
        {
            timeRemaining = 0;
        }

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

        if (timeRemaining <= 0 && !keyFound)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
