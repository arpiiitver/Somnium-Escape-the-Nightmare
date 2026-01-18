using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEditor.Rendering.LookDev;
public class Level1Manager : MonoBehaviour
{
    // Drag your FadePanel Image (the black panel) here.
    public Image fadePanel;
    // Drag your LevelClearedText (TextMeshProUGUI) here.
    public TextMeshProUGUI levelClearedText;
    // Duration of the fade (in seconds).
    public float fadeDuration = 2f;
    // The key that triggers the fade.
    public KeyCode triggerKey = KeyCode.E;

    // This flag will be set by your door trigger collider.
    private bool playerNearDoor = false;

    void Update()
    {
        // Only trigger fade when the player is near the door and presses the trigger key.
        if (playerNearDoor && Input.GetKeyDown(triggerKey))
        {
            StartCoroutine(FadeToLevelEnd());
        }
    }

    IEnumerator FadeToLevelEnd()
    {
        float timer = 0f;
        // Store the original colors (we assume starting alpha is 0)
        Color panelColor = fadePanel.color;
        Color textColor = levelClearedText.color;

        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            // Set the alpha for the fade panel.
            fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, alpha);
            // Set the alpha for the level cleared text.
            levelClearedText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }
        // Ensure both are fully opaque.
        fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, 1f);
        levelClearedText.color = new Color(textColor.r, textColor.g, textColor.b, 1f);
    }

    // When the player enters the door trigger area...
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearDoor = true;
    }

    // When the player exits the door trigger area...
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearDoor = false;
    }
}


/*
    public Image fadePanel;               // Assign FadePanel (black image)
public TextMeshProUGUI levelText;     // Assign TMP text object
public float fadeDuration = 1f;

private bool playerNearDoor = false;

void Update()
{
    if (playerNearDoor && Input.GetKeyDown(KeyCode.E))
    {
        StartCoroutine(FadeAndShowText());
    }
}

IEnumerator FadeAndShowText()
{
    // Fade to black
    float t = 0;
    Color fadeColor = fadePanel.color;

    while (t < fadeDuration)
    {
        t += Time.deltaTime;
        fadeColor.a = Mathf.Lerp(0, 1, t / fadeDuration);
        fadePanel.color = fadeColor;
        yield return null;
    }

    // Show "LEVEL 1 CLEARED"
    levelText.color = new Color(1, 1, 1, 1); // fully visible
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
 */