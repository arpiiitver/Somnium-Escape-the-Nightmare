using UnityEngine;
using System.Collections;

public class OpenChest : MonoBehaviour
{
    [Header("Lid Settings")]
    [Tooltip("The chest lid transform")]
    public Transform lid;
    [Tooltip("Local X rotation when closed (e.g. –89.98)")]
    public float closedAngle = -89.98f;
    [Tooltip("Local X rotation when open (e.g. –148.3)")]
    public float openAngle = -148.3f;
    [Tooltip("How fast the lid opens")]
    public float rotationSpeed = 2f;

    [Header("Interaction")]
    [Tooltip("Key to open the chest")]
    public KeyCode openKey = KeyCode.E;
    [Tooltip("How close the player must be")]
    public float openDistance = 2f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;
    private Coroutine rotateCoroutine;
    private Transform player;

    void Start()
    {
        // cache the player transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // initialize closed and open rotations based on lid's localEulerAngles
        Vector3 e = lid.localEulerAngles;
        closedRotation = Quaternion.Euler(closedAngle, e.y, e.z);
        openRotation = Quaternion.Euler(openAngle, e.y, e.z);

        // set the lid to the closed rotation
        lid.localRotation = closedRotation;
    }

    void Update()
    {
        // check key press and proximity
        if (!isOpen &&
            Input.GetKeyDown(openKey) &&
            Vector3.Distance(player.position, transform.position) <= openDistance)
        {
            ToggleChest();
        }
    }

    void ToggleChest()
    {
        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        rotateCoroutine = StartCoroutine(RotateLid(isOpen ? closedRotation : openRotation));
        isOpen = !isOpen;
    }

    IEnumerator RotateLid(Quaternion targetRotation)
    {
        while (Quaternion.Angle(lid.localRotation, targetRotation) > 0.1f)
        {
            lid.localRotation = Quaternion.Slerp(
                lid.localRotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
            yield return null;
        }
        // snap to exact rotation
        lid.localRotation = targetRotation;
    }
}
