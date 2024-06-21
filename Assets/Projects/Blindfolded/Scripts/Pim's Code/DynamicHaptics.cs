using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DynamicHaptics : MonoBehaviour
{
    /// <summary>
    ///Feedback of the whitecane when it touches a collider
    ///To refine this maybe use HapticsSDK
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        /*  if (!other.isTrigger)
          {
              StartCoroutine(Haptics(1, 1, 0.1f, true, false));
          }
        */
        //Dynamic based on the surface
        //Should base it off of cane direction
        switch (other.tag)
        {
            case "Grass":
                Haptics(4000f, 4000f, 0.5f, true, false);
                //Play a sound.
                break;
            case "Gravel":
                Haptics(1.5f, 2f, 0.5f, true, false);
                break;
            case "Floor":
                Haptics(0.2f, 0.5f, 0.5f, true, false);
                break;
            case "Tactile Paving":
                Haptics(10f, 10f, 0.5f, true, false);
                break;
            case "Ground":
                Haptics(500f, 100f, 0.5f, true, false);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        LayerMask layer = LayerMask.NameToLayer("Wall");
        if (collision.collider.gameObject.layer == layer)
        {
            Haptics(2000f, 2000f, 0.5f, true, false);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }

    /// <summary>
    /// When exiting a trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }

    /// <summary>
    /// Haptic feedback Enumarator
    /// </summary>
    /// <param name="frequency"></param>
    /// <param name="amplitude"></param>
    /// <param name="duration"></param>
    /// <param name="rightHand"></param>
    /// <param name="leftHand"></param>
    /// <returns></returns>
    public void Haptics(float frequency, float amplitude, float duration, bool rightHand, bool leftHand)
    {
        if (rightHand) OVRInput.SetControllerVibration(frequency, amplitude, OVRInput.Controller.RTouch);
        if (leftHand) OVRInput.SetControllerVibration(frequency, amplitude, OVRInput.Controller.LTouch);
    }

}
