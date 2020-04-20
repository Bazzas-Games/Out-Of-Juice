using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingPlatform : MonoBehaviour
{
    // Transforms to act as start and end markers for journey.
    public Transform pos1, pos2;
    public Transform startPos;
    public bool isPowered = false;

    public float speed = 1.0f;

    Vector3 nextPos;

    private void Start()
    {
        nextPos = startPos.position;
    }

    void Update(){
        if(isPowered == true){
            Move();
        }
    }

    public void Move()
    {
        isPowered = true;
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

    public void Reset(){
        isPowered = false;
        transform.position = pos1.position;
    }

}


