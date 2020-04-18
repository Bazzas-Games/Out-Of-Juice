using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public bool isGrounded = false;
    public bool isTouchingWall = false;
    public bool isGrappling = false;
    public float groundSpeed = 5f;
    public float airSpeed = 2f;

    public float grappleSpeed = 5f;
    public float grappleRange = 5f;

    private Vector2 inputVector = Vector2.zero;
    private Vector2 grappleVector = Vector2.zero;
    private List<Vector2> contactVectors = new List<Vector2>();
    private Rigidbody2D rb;
    private RaycastHit2D grapplePoint;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        InputPoll();
        if (isGrounded) AccelerateGround();
        else AccelerateAir();
    }


    void InputPoll()
    {
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");
        if(Input.GetAxisRaw("Fire1") == 1 && !isGrappling)
        {
            Grapple(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        if(Input.GetAxisRaw("Fire1") == 0 && isGrappling)
        {
            isGrappling = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contactPoints);
        foreach(ContactPoint2D c in contactPoints)
        {
            Vector2 contactVector = c.point - (Vector2)transform.position;
            if (!contactVectors.Contains(contactVector)) contactVectors.Add(contactVector);
        }
    }
    public void Kill()
    {
        Debug.Log("Player is ded");
    }

    void AccelerateGround()
    {
        rb.AddForce(new Vector2(inputVector.x * groundSpeed, 0));
    }

    void AccelerateAir()
    {
        rb.AddForce(new Vector2(inputVector.x * airSpeed, 0));
    }

    void AccelerateGrapple()
    {
        rb.AddForce(grappleVector * grappleSpeed);
        rb.AddForce(inputVector.normalized * airSpeed);
    }
    
    void Grapple(Vector2 direction)
    {
        grapplePoint = Physics2D.Raycast(transform.position, direction - (Vector2)transform.position, grappleRange, LayerMask.GetMask("Environment"));
        Debug.DrawRay(transform.position, (direction - (Vector2)transform.position).normalized * grappleRange);
        if (grapplePoint.collider != null)
        {
            isGrappling = true;
            Debug.Log("hit "+ grapplePoint.collider.name);
        }
    }


}
