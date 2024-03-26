using UnityEngine;

public class DirectionCheck : MonoBehaviour
{
    public AudioClip sound;
    public Transform player;

    public bool isLeft = false;
    public bool isRight = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //Change that to if the certain node played a sound 
        {
            AudioSource audioSource = GetComponent<AudioSource>(); //integrate with pims manager audio

            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found on this GameObject.");
                return;
            }

            // Play the sound
            audioSource.clip = sound;
            audioSource.Play();

            // Cast a ray from this object towards the player
            Vector3 directionToPlayer = player.position - transform.position;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, directionToPlayer, out hit))
            {
                if (hit.collider.transform == player)
                {
                    // Determine if the player is on the left or right side
                    Vector3 objectRight = transform.right; //maybe here it needs replacement with not just the transform.right but some pointers that we can put on the instruments
                    float dotProduct = Vector3.Dot(objectRight, directionToPlayer);

                    if (dotProduct > 0)
                    {
                        isLeft = true;
                        isRight = false;
                        Debug.Log("Player is on the right side of the object./The sound came from the left.");
                    }
                    else if (dotProduct < 0)
                    {
                        isLeft = false;
                        isRight = true;
                        Debug.Log("Player is on the left side of the object./The sound came from the right.");
                    }
                    else
                    {
                        isLeft = false;
                        isRight = false;
                        Debug.Log("Player is directly in front or behind of the object.");
                    }
                }
            }
        }
    }
}