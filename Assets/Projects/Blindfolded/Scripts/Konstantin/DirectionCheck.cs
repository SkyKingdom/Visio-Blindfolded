using UnityEngine;

public class DirectionCheck : MonoBehaviour
{
    public AudioClip sound;
    public Transform player;

    public bool isLeft = false;
    public bool isRight = false;
    public bool isFront = false;
    public bool isBack = false;

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

                //Uncoment the code below and replace within the if hit.collider statement to add fron and back detection

                //// Determine if the player is on the left, right, front, or back side
                //Vector3 objectForward = transform.forward;
                //float dotProductForward = Vector3.Dot(objectForward, directionToPlayer);
                //Vector3 objectRight = transform.right;
                //float dotProductRight = Vector3.Dot(objectRight, directionToPlayer);


                //// Define the threshold angle for considering the player in front, behind, left or right
                //float angleThreshold = Mathf.Cos(Mathf.Deg2Rad * 30); // 30 degrees threshold

                //if (dotProductForward > angleThreshold) //(dotProduct > 0)
                //{
                //    isFront = false;
                //    isBack = true;
                //    isLeft = false;
                //    isRight = false;
                //    Debug.Log("Player is in front of the object./The sound came from behind");
                //    //Debug.Log("Player is on the right side of the object./The sound came from the left.");
                //}
                //else if (dotProductForward < -angleThreshold)
                //{
                //    isFront = true;
                //    isBack = false;
                //    isLeft = false;
                //    isRight = false;
                //    Debug.Log("Player is behind the object./The sound came from in front");
                //    //Debug.Log("Player is on the left side of the object./The sound came from the right.");
                //}
                //else if (dotProductRight > angleThreshold)
                //{
                //    isFront = false;
                //    isBack = false;
                //    isLeft = true;
                //    isRight = false;
                //    Debug.Log("Player is on the right side of the object./ The sound came from the left.");
                //    //Debug.Log("Player is directly in front or behind of the object.");
                //}
                //else if (dotProductRight < -angleThreshold)
                //{
                //    isFront = false;
                //    isBack = false;
                //    isLeft = false;
                //    isRight = true;
                //    Debug.Log("Player is on the left side of the object./ The sound came from the right");
                //}
            }
        }
    }
}