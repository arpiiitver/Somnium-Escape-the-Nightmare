using UnityEngine;
using System.Collections;
public class OpenDrawer1R1 : MonoBehaviour
{
    public float openAngle = -90f; // How much to rotate on Z
    public float rotationSpeed = 2f;
    public KeyCode openKey = KeyCode.E;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;
    private Coroutine rotateCoroutine;

    private bool playerInRange = false; // To detect if player is near

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(openKey))
        {
            ToggleDrawer();
        }
    }

    void ToggleDrawer()
    {
        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        Quaternion targetRotation = isOpen ? closedRotation : openRotation;
        rotateCoroutine = StartCoroutine(RotateDrawer(targetRotation));
        isOpen = !isOpen;
    }

    IEnumerator RotateDrawer(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.localRotation, targetRotation) > 0.1f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }
        transform.localRotation = targetRotation;
        rotateCoroutine = null;
    }

    // Trigger detection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}


/*
 *
     // These will store the closed and open rotations.
    public Quaternion closedRotation;
    public Quaternion openRotation;

    // Speed factor for the rotation transition.
    public float rotationSpeed = 2f;

    // Key to toggle the door.
    public KeyCode toggleKey = KeyCode.E;

    // Tracks the current state (open/closed).
    private bool isOpen = false;

    // Used to run the rotation coroutine.
    private Coroutine rotateCoroutine;

    void Start()
    {
        // Initialize the rotations.
        // Set the closed rotation to the current rotation.
        closedRotation = transform.rotation;

        // Define the open rotation.
        // Adjust the Euler angles below as needed; for example, rotating 90° around the Y axis.
        openRotation = transform.rotation * Quaternion.Euler(0, 90f, 0);
    }

    void Update()
    {
        // When the toggle key is pressed, switch the door state.
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleDrawer();
        }
    }

    void ToggleDrawer()
    {
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
        }

        // Decide the target rotation based on the current state.
        Quaternion targetRotation = isOpen ? closedRotation : openRotation;

        // Start the smooth rotation coroutine.
        rotateCoroutine = StartCoroutine(RotateTo(targetRotation));

        // Toggle the state.
        isOpen = !isOpen;
    }

    IEnumerator RotateTo(Quaternion targetRotation)
    {
        // Continue until the rotation is nearly equal to the target.
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        // Ensure the final rotation is exactly set.
        transform.rotation = targetRotation;
        rotateCoroutine = null;
    }
 */