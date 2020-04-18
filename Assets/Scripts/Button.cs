using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Door[] door;
    public LoopingPlatform[] loopingPlatform;
    public bool isPowered = false;

    private void Update()
    {
        Pushed();
    }
    public void Pushed()
    {
        if (isPowered == true)
        {
            foreach(Door d in door)
            {
                d.Move();
            }
            foreach(LoopingPlatform l in loopingPlatform)
            {
                l.Move();
            }
        }
        else
        {
            isPowered = false;
        }

    }
}
