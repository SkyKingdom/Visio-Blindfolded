using UnityEngine;
using UnityEngine.VFX;

public class ActivatePortal : MonoBehaviour
{
    public VisualEffect vfx; // Reference to your Visual Effect Graph component
    private Collider triggerCollider; // Reference to the Collider component
    private Timer animationTimer;
    [SerializeField] private Levels.levels level;
    
    private void Start()
    {
      animationTimer = new Timer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming your player has a "Player" tag
        {
            // Play the Visual Effect
            vfx.Play();
            animationTimer.SetTimer(2);
        }
    }


    private void Update()
    {
        if (animationTimer.isActive && animationTimer.TimerDone())
        {
            animationTimer.StopTimer();
            GameManager.SceneLoader(level);
        }
    }
}