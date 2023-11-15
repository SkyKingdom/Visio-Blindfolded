using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCaneHaptics : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
    }
}
