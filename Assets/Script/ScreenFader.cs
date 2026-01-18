using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class ScreenFader : MonoBehaviour
{
    public CanvasGroup fadePanel; // assign the FadePanel here
    public float fadeDuration = 2f;

    void Start()
    {
        fadePanel.alpha = 0; // start invisible
    }

    public void FadeToBlack()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        fadePanel.alpha = 1;
        // After fade: load Level 2 or do something else
        Debug.Log("Fade complete — load Level 2 here.");
    }
}
