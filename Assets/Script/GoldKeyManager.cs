using UnityEngine;

public class GoldKeyManager : MonoBehaviour
{
    public bool hasKey = false;
   // public AudioSource keyPickupSound;

    private GameObject keyInWorld;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Key") && Input.GetKeyDown(KeyCode.F))
        {
            keyInWorld = other.gameObject;
            hasKey = true;
            //keyPickupSound.Play();
            keyInWorld.SetActive(false); // hide the key in world
        }
    }
}
