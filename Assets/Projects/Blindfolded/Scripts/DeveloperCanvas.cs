using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperCanvas : MonoBehaviour
{

    [SerializeField] GameObject Console;
    // Update is called once per frame
    void Update()
    {
        //OVRInput.Update();

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Console.SetActive(!Console.activeSelf);
        }
    }
}
