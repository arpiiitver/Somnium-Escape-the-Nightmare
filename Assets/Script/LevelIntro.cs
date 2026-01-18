using UnityEngine;
using TMPro;

public class LevelIntro : MonoBehaviour
{
    [TextArea(4, 8)]
    [Tooltip("The story text to display")]
    public string introText;

    [Tooltip("Panel (with black BG) containing a TextMeshProUGUI child")]
    public GameObject introPanel;

    [Tooltip("The TMP text component inside the panel")]
    public TextMeshProUGUI introTextUI;

    [Tooltip("Scripts to disable until player presses E (e.g. player & AI controllers)")]
    public MonoBehaviour[] scriptsToDisable;

    private bool _waitingForKey = true;

    void Start()
    {
        // 1) Populate text and show UI
        introTextUI.text = introText;
        introPanel.SetActive(true);

        // 2) Disable gameplay scripts
        foreach (var s in scriptsToDisable)
            s.enabled = false;
    }

    void Update()
    {
        if (_waitingForKey && Input.GetKeyDown(KeyCode.E))
        {
            _waitingForKey = false;
            // Hide panel
            introPanel.SetActive(false);
            // Re-enable gameplay
            foreach (var s in scriptsToDisable)
                s.enabled = true;
            // Destroy this so it doesn’t run again
            Destroy(this);
        }
    }
}
