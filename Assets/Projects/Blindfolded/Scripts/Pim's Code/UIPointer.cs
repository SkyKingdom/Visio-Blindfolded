using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPointer : MonoBehaviour
{
    public Transform controller; 
    public LineRenderer lineRenderer;
    public EventSystem eventSystem;
    public LayerMask uiLayer; 


    /// <summary>
    /// Raycast for the UI laserpointer.
    /// </summary>
    private void Update()
    {
      
        Vector3 forwardDirection = controller.forward;
        Ray ray = new Ray(controller.position, forwardDirection);
        RaycastHit hit;

        lineRenderer.SetPosition(0, ray.origin);

        if (Physics.Raycast(ray, out hit, 100, uiLayer))
        {
            lineRenderer.SetPosition(1, hit.point);

       
            var pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Camera.main.WorldToScreenPoint(hit.point);

            List<RaycastResult> results = new List<RaycastResult>();
            eventSystem.RaycastAll(pointerEventData, results);

            foreach (var result in results)
            {
                if (result.gameObject.GetComponent<Button>() != null)
                {

                    var button = result.gameObject.GetComponent<Button>();

                  
                    var selectable = result.gameObject.GetComponent<Selectable>();
                    if (selectable != null)
                    {
                        selectable.Select();
                    }

                 
                    if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                    {
                        button.onClick.Invoke();
                    }
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(1, ray.origin + ray.direction * 100);
        }
    }
}
