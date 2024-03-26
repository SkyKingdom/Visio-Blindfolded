using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTest : MonoBehaviour
{
    public GameObject prefab;
    public float speed;

    // Start is called before the first frame update
    public void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.LogWarning("Calling Update");
        if (prefab != null)
        {
            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                Debug.LogWarning("Shooting ball");
                GameObject ball = Instantiate(prefab, transform.position, Quaternion.identity);
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                rb.velocity = transform.forward * speed;
            }
        }
    }
}
