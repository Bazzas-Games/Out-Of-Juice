using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public PlatformController platformController;
    public bool isBeingPushed = false;

    private void Update()
    {
        Pushed();
    }
    public void Pushed()
    {
        if (isBeingPushed == true)
        {
           platformController.Move();
        }
        else
        {
            isBeingPushed = false;
        }

    }
}
