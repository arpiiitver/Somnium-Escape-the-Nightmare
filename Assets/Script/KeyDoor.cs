using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class KeyDoor : MonoBehaviour
{
    // Reference to your key manager (attached to your player)
    public GoldKeyManager keyManager;
    // The key to unlock the door (press E)
    public KeyCode unlockKey = KeyCode.E;

    // Drag your FadePanel UI Image here (which is disabled by default)
    public Image fadeCanvas;
    // Drag your LevelClearedText (TextMeshProUGUI) UI element here (disabled by default)
    public TextMeshProUGUI levelClearedText;

    // Duration of the fade effect (in seconds)
    public float fadeDuration = 2f;
    // How long to hold the cleared text before loading next scene
    public float displayDuration = 1f;
    // Name of the next scene to load
    public string nextSceneName = "Level2";

    private bool playerNearDoor = false;

    void Update()
    {
        if (playerNearDoor && Input.GetKeyDown(unlockKey) && keyManager.hasKey)
        {
            StartCoroutine(FadeAndShowMessage());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearDoor = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearDoor = false;
    }

    IEnumerator FadeAndShowMessage()
    {
        // Enable UI
        fadeCanvas.gameObject.SetActive(true);
        levelClearedText.gameObject.SetActive(true);

        // Initialize transparency
        Color panelColor = fadeCanvas.color; panelColor.a = 0f; fadeCanvas.color = panelColor;
        Color textColor = levelClearedText.color; textColor.a = 0f; levelClearedText.color = textColor;

        // Fade in
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeCanvas.color = new Color(panelColor.r, panelColor.g, panelColor.b, alpha);
            levelClearedText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        // Snap to full opacity
        fadeCanvas.color = new Color(panelColor.r, panelColor.g, panelColor.b, 1f);
        levelClearedText.color = new Color(textColor.r, textColor.g, textColor.b, 1f);

        // Hold the message
        yield return new WaitForSeconds(displayDuration);

        // Load Level 2
        SceneManager.LoadScene(nextSceneName);
    }
}
