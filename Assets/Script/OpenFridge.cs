using UnityEngine;
using System.Collections;
public class OpenFridge : MonoBehaviour
{
    // Target open angle (-126 degrees)
    public float openAngle = -110f;
    // Speed at which the door opens/closes
    public float rotationSpeed = 2f;
    // Key to open/close the door
    public KeyCode openKey = KeyCode.F;

    // Store the initial rotation
    private Quaternion closedRotation;
    // Store the open rotation
    private Quaternion openRotation;
    // Door state to track if it’s open or closed
    private bool isOpen = false;
    private Coroutine rotateCoroutine;

    void Start()
    {
        // Save the initial rotation of the door as the closed position
        closedRotation = transform.localRotation;
        // Calculate the open rotation by rotating around the Y-axis to 126 degrees
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        // Open or close the door when pressing the specified key
        if (Input.GetKeyDown(openKey))
        {
            ToggleDoor();
        }
    }

    void ToggleDoor()
    {
        // Stop any ongoing rotation
        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        // Set target rotation (open or closed)
        Quaternion targetRotation = isOpen ? closedRotation : openRotation;
        rotateCoroutine = StartCoroutine(RotateDoor(targetRotation));
        isOpen = !isOpen;
    }

    IEnumerator RotateDoor(Quaternion targetRotation)
    {
        // Rotate smoothly towards the target rotation
        while (Quaternion.Angle(transform.localRotation, targetRotation) > 0.1f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }
        // Snap to the target rotation when close enough
        //transform.localRotation = targetRotation;
        //rotateCoroutine = null;
    }
}
