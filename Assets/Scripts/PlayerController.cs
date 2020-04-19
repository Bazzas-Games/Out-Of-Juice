using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public bool isGrounded = false;
    public bool isTouchingWall = false;
    public float collisionBias = 0.04f;
    public bool isGrappling = false;
    public float groundSpeed = 200f;
    public float airSpeed = 2f;
    public float airResistance = 10f;
    public float jumpSpeed = 5f;
    public float maxVelocity = 3f;
    public float friction = 20f;

    public float grappleSpeed = 5f;
    public float grappleRange = 5f;

    private Vector2 wallNormal = Vector2.zero;
    private Vector2 inputVector = Vector2.zero;
    private Vector2 grappleVector = Vector2.zero;
    private Vector2 grapplePoint = Vector2.zero;
    private List<Vector2> contactVectors = new List<Vector2>();
    private List<RaycastHit2D> contacts = new List<RaycastHit2D>();
    private Rigidbody2D rb;
    private RaycastHit2D grappleRaycastHit;
    private SpringJoint2D grappleJoint;
    private LineRenderer lr;
    private float walljumpInputDelay = 0.5f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        InputPoll();
        if (isGrounded) rb.velocity = AccelerateGround();
        else if (isGrappling) rb.velocity = AccelerateGrapple();
        else rb.velocity = AccelerateAir();
    }
    void FixedUpdate()
    {
        UpdateContactNormals();
    }
    void LateUpdate()
    {
        DrawGrapple();
    }

    // all input-dependent methods go here
    void InputPoll()
    {
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");
        if(Input.GetButton("Fire1") && !isGrappling)
        {
            Grapple(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        if (isGrappling)
        {
            if (!Input.GetButton("Fire1")) {
                StopGrapple();
            }
            else
            {
                Debug.DrawLine(transform.position, grapplePoint);
                grappleVector = grapplePoint - (Vector2)transform.position;
            }
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
                Jump();
            else if (isTouchingWall)
                WallJump();
        }
    }

    // add collision points to list of rays to cast
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

    void DrawGrapple()
    {
        if (!isGrappling) return;

    }


    void UpdateContactNormals()
    {
        isGrounded = false;
        isTouchingWall = false;
        List<Vector2> toRemove = new List<Vector2>();
        foreach(Vector2 v in contactVectors)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, v, v.magnitude + collisionBias, LayerMask.GetMask("Environment"));
            if(hit.collider != null)
            {
                float angle = Vector2.Angle(Vector3.up, hit.normal);
                if (angle == 0)
                {
                    isGrounded = true;
                    Debug.DrawRay(hit.point, hit.normal, Color.green);
                }
                else if(angle == 90 || angle == -90)
                {
                    wallNormal = hit.normal;
                    isTouchingWall = true;
                    Debug.DrawRay(hit.point, hit.normal, Color.yellow);
                }
            }
            else
            {
                toRemove.Add(v);
            }
        }
        foreach(Vector2 v in toRemove)
        {
            contactVectors.Remove(v);
        }
    }



    Vector2 Accelerate(Vector2 accelDir, Vector2 currentVel, float accelVel, float maxVel)
    {
        float projVel = Vector2.Dot(currentVel, accelDir);
        accelVel *= Time.deltaTime;
        if(projVel + accelVel > maxVel)
        {
            accelVel = maxVel - projVel;
        }
        return currentVel + accelVel * accelDir;
    }


    Vector2 AccelerateGround()
    {
        float speed = Mathf.Abs(rb.velocity.x);
        if(speed != 0)
        {
            float slow = speed * friction * Time.deltaTime;
            rb.velocity *= new Vector2(Mathf.Max(speed - slow, 0) / speed, 1);
        }
        return Accelerate(new Vector2(inputVector.x, 0), rb.velocity, groundSpeed, maxVelocity);
    }


    Vector2 AccelerateAir()
    {
        float speed = Mathf.Abs(rb.velocity.x);
        if (speed != 0)
        {
            float slow = speed * airResistance * Time.deltaTime;
            rb.velocity *= new Vector2(Mathf.Max(speed - slow, 0) / speed, 1);
        }
        return Accelerate(new Vector2(inputVector.x, 0), rb.velocity, airSpeed, maxVelocity);
    }


    Vector2 AccelerateGrapple()
    {

        return Accelerate(inputVector.normalized, rb.velocity, airSpeed, maxVelocity);
    }




    void Jump()
    {
        isGrounded = false;
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }
    void WallJump()
    {
        if(!isGrounded)
        rb.velocity = new Vector2(wallNormal.x * jumpSpeed, jumpSpeed);
    }
    
    void Grapple(Vector2 direction)
    {
        // raycast for grapple
        grappleRaycastHit = Physics2D.Raycast(transform.position, direction - (Vector2)transform.position, grappleRange, LayerMask.GetMask("Environment"));
        Debug.DrawRay(transform.position, (direction - (Vector2)transform.position).normalized * grappleRange);
        
        // if grapple hit anything
        if (grappleRaycastHit.collider != null)
        {
            // set grapple point
            grapplePoint = grappleRaycastHit.point;
            isGrappling = true;
        }
    }
    void StopGrapple()
    {
        isGrappling = false;
    }


}
