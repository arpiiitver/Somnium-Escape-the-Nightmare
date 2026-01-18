using UnityEngine;
using System.Collections;

public class HingeDoor : MonoBehaviour
{
    // Angle (in degrees) to rotate when the door is open.
    public float openAngle = 90f;

    // Rotation speed factor.
    public float rotationSpeed = 2f;

    // The door's closed and open rotations.
    private Quaternion closedRotation;
    private Quaternion openRotation;

    // Door state.
    private bool isOpen = false;
    private Coroutine rotateCoroutine;

    void Start()
    {
        // Store the door's initial (closed) rotation.
        closedRotation = transform.rotation;

        // Calculate the open rotation.
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        // Toggle the door state when the "O" key is pressed.
        if (Input.GetKeyDown(KeyCode.O))
        {
            ToggleDoor();
        }
    }

    public void ToggleDoor()
    {
        // Stop any ongoing rotation coroutine.
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
        }

        // Determine target rotation.
        Quaternion targetRotation = isOpen ? closedRotation : openRotation;

        // Start a coroutine to rotate to the target.
        rotateCoroutine = StartCoroutine(RotateDoor(targetRotation));

        // Toggle door state.
        isOpen = !isOpen;
    }

    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        // Rotate until the door is nearly at the target rotation.
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        // Snap to the target rotation.
        transform.rotation = targetRotation;
        rotateCoroutine = null;
    }
}
