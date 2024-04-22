using UnityEngine;
using UnityEngine.VFX;

public class ActivatePortal : MonoBehaviour
{
    public VisualEffect vfx; // Reference to your Visual Effect component

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming your player has a "Player" tag
        {
            vfx.Play(); // Play the Visual Effect
        }
    }
}