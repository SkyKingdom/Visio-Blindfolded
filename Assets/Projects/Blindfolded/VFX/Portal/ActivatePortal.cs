using UnityEngine;
using UnityEngine.VFX;

public class ActivatePortal : MonoBehaviour
{
    public VisualEffect vfx; // Reference to your Visual Effect Graph component
    private Collider triggerCollider; // Reference to the Collider component
    private Timer animationTimer;
    private Timer waitTimer;
    [SerializeField] private Levels.levels level;
    private OVRPlayerController playerController;

    private void Start()
    {
        animationTimer = new Timer();
        waitTimer = new Timer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming your player has a "Player" tag
        {
            // Play the Visual Effect
            //Add audio effect.
            vfx.Play();
            animationTimer.SetTimer(2);
            waitTimer.SetTimer(0.5f);
            playerController = other.GetComponent<OVRPlayerController>();
        }
    }


    private void Update()
    {
        if (animationTimer.isActive && animationTimer.TimerDone())
        {
            animationTimer.StopTimer();
            GameManager.SceneLoader(level);
        }

        if (waitTimer.isActive && waitTimer.TimerDone())
        {
            waitTimer.StopTimer();
            ToggleMovement(playerController , false);
        }
    }


    public void ToggleMovement(OVRPlayerController controller, bool toggle)
    {
        if (toggle)
        {
            controller.Acceleration = 0.1f;
        }
        else
        {
            controller.Acceleration = 0f;
        }
    }
}