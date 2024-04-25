using UnityEngine;
using UnityEngine.VFX;

public class ActivatePortal : MonoBehaviour
{
    public VisualEffect vfx; // Reference to your Visual Effect Graph component
    private Collider triggerCollider; // Reference to the Collider component

    private void Start()
    {
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming your player has a "Player" tag
        {
            // Play the Visual Effect
            vfx.Play();
        }
    }
}