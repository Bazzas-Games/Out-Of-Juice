using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentumPlatform : MonoBehaviour
{
    // Transforms to act as start and end markers for the journey.
    public Transform startMarker;
    public Transform endMarker;
    Transform tempTrans;

    // Movement speed in units per second.
    public float speed = 4.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    Rigidbody2D rb;
    public GameObject object1, object2;

    public float thrust = 4.0f;

    void Start()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }

    // Move to the target end position.
    void Update()
    {
        // Distance moved equals elapsed time times speed.
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);

        if(Input.GetButtonDown("Jump"))
        {
            rb.AddForce(transform.up * thrust, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ChangeParent();
            rb = GetComponentInChildren<Rigidbody2D>();
        }
        else
        {
            RevertParent();
        }
    }

    private void OnCollisionExit2D(Collision2D collision) 
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            RevertParent();
        }
    }

    // Make Player a child of MPlatform.
    void ChangeParent()
    {
        tempTrans = object2.transform.parent;
        object2.transform.parent = object1.transform;
       
    }

    // Revert the parent of player.
    void RevertParent()
    {
        object2.transform.parent = tempTrans;
    }

    
}
