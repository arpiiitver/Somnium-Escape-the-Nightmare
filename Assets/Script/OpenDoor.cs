using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour
{
    [Header("Positions")]
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Vector3 targetPositionOpen = new Vector3(-57.12f, -2.09f, -17.52f);
    private Vector3 targetPositionClosed = new Vector3(-60.38f, -2.09f, -17.52f);

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Interaction")]
    public KeyCode openKey = KeyCode.E;

    private bool isOpen = false;
    private bool playerInRange = false;
    private Coroutine moveCoroutine;

    void Start()
    {
        closedPosition = targetPositionClosed;
        openPosition = targetPositionOpen;
        transform.localPosition = closedPosition;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(openKey))
        {
            ToggleDoor();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    void ToggleDoor()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);

        Vector3 target = isOpen ? closedPosition : openPosition;
        moveCoroutine = StartCoroutine(MoveDoor(target));
        isOpen = !isOpen;
    }

    IEnumerator MoveDoor(Vector3 target)
    {
        while (Vector3.Distance(transform.localPosition, target) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * moveSpeed);
            yield return null;
        }
        transform.localPosition = target;
        moveCoroutine = null;
    }
}
