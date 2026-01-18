using UnityEngine;
using System.Collections;
public class OpenDrawer1L1 : MonoBehaviour
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
