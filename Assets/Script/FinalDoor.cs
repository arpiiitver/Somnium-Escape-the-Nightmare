using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FinalDoor : MonoBehaviour
{
    [Header("References")]
    public GoldKeyManager keyManager;          // Player's key manager
    public KeyCode unlockKey = KeyCode.E;      // Key to activate

    [Header("UI Elements")]
    public Image fadeCanvas;                   // Full-screen black panel (disabled by default)
    public TextMeshProUGUI levelClearedText;   // “Level 2 Cleared” text (disabled by default)

    [Header("Timing")]
    public float fadeDuration = 2f;            // Time to fade from transparent to black
    public float displayDuration = 3f;         // How long the text stays fully visible

    private bool playerNearDoor = false;
    private bool hasTriggered = false;

    void Update()
    {
        if (!hasTriggered
            && playerNearDoor
            && Input.GetKeyDown(unlockKey)
            && keyManager.hasKey)
        {
            hasTriggered = true;
            StartCoroutine(FadeAndShowEnd());
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

    private IEnumerator FadeAndShowEnd()
    {
        // Enable UI elements
        fadeCanvas.gameObject.SetActive(true);
        levelClearedText.gameObject.SetActive(true);

        // Initialize transparency
        Color panelCol = fadeCanvas.color; panelCol.a = 0f; fadeCanvas.color = panelCol;
        Color textCol = levelClearedText.color; textCol.a = 0f; levelClearedText.color = textCol;

        // Fade to black
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeCanvas.color = new Color(panelCol.r, panelCol.g, panelCol.b, alpha);
            levelClearedText.color = new Color(textCol.r, textCol.g, textCol.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure full opacity
        fadeCanvas.color = new Color(panelCol.r, panelCol.g, panelCol.b, 1f);
        levelClearedText.color = new Color(textCol.r, textCol.g, textCol.b, 1f);

        // Hold the final message
        yield return new WaitForSeconds(displayDuration);

        // At this point, the game is complete.
        // You can optionally show a "Quit" or "Main Menu" button here.
    }
}
