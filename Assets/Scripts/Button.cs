using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public LoopingPlatform loopingPlatform;
    public bool isPowered = false;

    private void Update()
    {
        Pushed();
    }
    public void Pushed()
    {
        if (isPowered == true)
        {
            loopingPlatform.Move();
        }
        else
        {
            isPowered = false;
        }

    }
}
