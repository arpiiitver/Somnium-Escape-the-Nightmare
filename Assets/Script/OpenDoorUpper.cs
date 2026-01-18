using UnityEngine;
using System.Collections;

public class OpenDoorUpper : MonoBehaviour
{
    // Positions for the door (closed and open)
    public Vector3 closedPos = new Vector3(-20.75f, 7.978415f, -17.51948f);
    public Vector3 openPos = new Vector3(-17.67f, 7.978415f, -17.51948f);

    // Speed and toggle key
    public float slideSpeed = 2f;
    public KeyCode toggleKey = KeyCode.E;

    // Door state and control flags
    private bool isOpen = false;
    private bool playerNearby = false;
    private Coroutine slidingCoroutine;

    // Jumpscare integration
    public JumpScareTrigger jumpscareTrigger; // Drag your JumpscareTrigger GameObject here in the Inspector
    private bool jumpscarePlayed = false;

    void Start()
    {
        // Ensure door starts closed
        transform.localPosition = closedPos;
    }

    void Update()
    {
        // Only respond to key press if player is in range
        if (playerNearby && Input.GetKeyDown(toggleKey))
        {
            Debug.Log("Toggle key pressed and player is nearby.");
            ToggleDoor();
        }
    }

    void ToggleDoor()
    {
        // Stop any ongoing movement
        if (slidingCoroutine != null)
        {
            StopCoroutine(slidingCoroutine);
        }

        // Decide target position based on current state
        Vector3 target = isOpen ? closedPos : openPos;

        // Begin smooth movement toward target
        slidingCoroutine = StartCoroutine(SlideDoor(target));

        // Toggle door state
        isOpen = !isOpen;

        // Trigger jumpscare only on opening, if provided
        if (isOpen && !jumpscarePlayed && jumpscareTrigger != null)
        {
            jumpscareTrigger.TriggerJumpscare();
            jumpscarePlayed = true;
        }
    }

    IEnumerator SlideDoor(Vector3 target)
    {
        // Smoothly move toward target position
        while (Vector3.Distance(transform.localPosition, target) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * slideSpeed);
            yield return null;
        }
        transform.localPosition = target;
        slidingCoroutine = null;
    }

    // Trigger events to detect when the player is near
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Player entered the door trigger area.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            Debug.Log("Player left the door trigger area.");
        }
    }
}
