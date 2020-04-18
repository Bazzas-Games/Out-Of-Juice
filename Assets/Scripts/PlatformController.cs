using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
     // Transforms to act as start and end markers for journey.
    //public Transform startMarker;
    //public Transform endMarker;
    public Transform pos1, pos2;
    public Transform startPos;    

    public float speed = 1.0f;

    Vector3 nextPos;

    private void Start()
    {
        nextPos = startPos.position;
    }

    public void Move()
    {

        if (transform.position == pos1.position)
        {
            nextPos = pos2.position;
        }
        if (transform.position == pos2.position)
        {
            nextPos = pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);

    }
}
