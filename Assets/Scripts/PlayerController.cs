using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    [Header("Graphics")]
    public Animator hud;
    public GameObject deathParticles;

    [Header("Collisions")]
    public bool isGrounded = false;
    public bool isTouchingWall = false;
    public float collisionBias = 0.04f;

    [Header("Movement")]
    public float groundSpeed = 200f;
    public float maxVelocity = 3f;
    public float friction = 20f;
    public float airSpeed = 2f;
    public float airResistance = 10f;
    public float jumpSpeed = 5f;
    public float walljumpInputDelay = 0.5f;

    [Header("Grappling Hook")]
    public bool isGrappling = false;
    public float grappleSpeed = 5f;
    public float grappleForce = 5f;
    public float grappleManoevreSpeed = 3f;
    public float grappleRange = 5f;
    public float grappleMinDistance = .5f;
    
    [Header("Battery")]
    public int maxBattery = 5;
    public int currentBattery = 5;

    [Header("Checkpoints")]
    public GameObject checkpointPrefab;
    public Checkpoint checkpoint;

    private float elapsedTime = 0f;
    private float startTime = 0f;
    private float grappleDistance = 0f;
    private Vector2 wallNormal = Vector2.zero;
    private Vector2 inputVector = Vector2.zero;
    private Vector2 grappleVector = Vector2.zero;
    private Vector2 grapplePoint = Vector2.zero;
    private BoxCollider2D boxCollider;
    private List<Vector2> contactVectors = new List<Vector2>();
    private Vector2[] failsafeVectors = { Vector2.right, Vector2.left, Vector2.down };
    private List<RaycastHit2D> contacts = new List<RaycastHit2D>();
    private Rigidbody2D rb;
    private RaycastHit2D grappleRaycastHit;
    private SpringJoint2D grappleJoint;
    private LineRenderer lr;
    private bool hasInput = true;
    private bool hasGroundInput = true;
    private Animator anim;
    private SpriteRenderer sprite;
    private PickUp[] pickUps;
    private GameObject crosshair;
    private Animator crosshairAnim;

    void Start() {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        GameObject[] pickupObjects = GameObject.FindGameObjectsWithTag("PowerUp");
        pickUps = new PickUp[pickupObjects.Length];
        crosshair = GameObject.FindGameObjectWithTag("Crosshair");
        crosshairAnim = crosshair.GetComponent<Animator>();
        for(int i = 0; i < pickUps.Length; i++) {
            pickUps[i] = pickupObjects[i].GetComponent<PickUp>();
        }
        checkpoint = Instantiate(checkpointPrefab, transform.position, Quaternion.identity).GetComponent<Checkpoint>();

    }

    void Update() {
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        crosshair.transform.position =  mousePosWorld + new Vector3(0, 0, 1);

        RaycastHit2D temp = Physics2D.Raycast(transform.position, mousePosWorld - transform.position, grappleRange, LayerMask.GetMask("Environment"));
        crosshairAnim.SetBool("inRange", temp.collider != null);
        InputPoll();

        if (isGrounded) rb.velocity = AccelerateGround();
        else if (isGrappling) rb.velocity = AccelerateGrapple();
        else rb.velocity = AccelerateAir();

       
        
        // update animation variables
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isTouchingWall", isTouchingWall);
        anim.SetFloat("xVel", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("yVel", rb.velocity.y);

        if (rb.velocity.x < 0) sprite.flipX = true;
        else if (rb.velocity.x > 0) sprite.flipX = false;

        if (!isGrounded && isTouchingWall) {
            if (wallNormal.x > 0) sprite.flipX = false;
            else sprite.flipX = true;
        }


    }
    void FixedUpdate() {
        UpdateContactNormals();
        if (isGrappling) {
            grappleDistance = Vector2.Distance(transform.position, grapplePoint) - grappleSpeed;
            if (grappleDistance > grappleMinDistance) grappleJoint.distance = grappleDistance;
            else grappleJoint.distance = grappleMinDistance;
        }
        if (currentBattery < 1) OnBatteryEmpty();
    }
    void LateUpdate() {
        DrawGrapple();
    }

    // all input-dependent methods go here
    void InputPoll() {
        if (currentBattery <= 0) hasGroundInput = false;
        else hasGroundInput = true;
         
        if (!hasInput) {
            elapsedTime = Time.time - startTime;
            if (elapsedTime >= walljumpInputDelay) hasInput = true;
        }
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");        
        if (Input.GetButton("Fire1") && !isGrappling && currentBattery > 0) {
            Cursor.visible = false;
            Grapple(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        if (isGrappling) {
            if (!Input.GetButton("Fire1")) {
                StopGrapple();
            }
            else {
                Debug.DrawLine(transform.position, grapplePoint);
                grappleVector = grapplePoint - (Vector2)transform.position;
            }
        }
        if (Input.GetButtonDown("Jump")) {
            if (isGrounded && currentBattery > 0)
                Jump();
            else if (isTouchingWall)
                WallJump();
        }
        if (Input.GetButtonDown("Reset")){
            Kill();
        }
    }

    // add collision points to list of rays to cast
    void OnCollisionEnter2D(Collision2D collision) {
        ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contactPoints);
        foreach (ContactPoint2D c in contactPoints) {
            Vector2 contactVector = c.point - (Vector2)transform.position;
            if (!contactVectors.Contains(contactVector)) contactVectors.Add(contactVector);
        }
        if (collision.gameObject.CompareTag("Button")) {
            Button b = collision.collider.GetComponent<Button>();
            foreach (ContactPoint2D c in contactPoints) {
                float angle = Vector2.Angle(c.normal, b.gameObject.transform.up);
                //Debug.Log(angle);
                if ( angle <= 45){
                    b.Press();
                }
            }
        }
    }

    public void Kill() {
        Debug.Log("Player is ded");
        // play animation
        Instantiate(deathParticles, transform.position, transform.rotation);
        // reset battery
        ModifyBattery(5);

        // enable all pickups
        foreach(PickUp p in pickUps) {
            p.Enable();
        }
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Button")) {
            g.GetComponent<Button>().Reset();
        }

        rb.velocity = Vector3.zero;
        transform.position = checkpoint.transform.position;
    }

    void DrawGrapple() {
        if (isGrappling) {
            lr.enabled = true;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, grapplePoint);
        }
        else {
            lr.enabled = false;
        }

    }
    void OnBatteryEmpty() {
        if (!isGrappling && isGrounded && rb.velocity.magnitude < 0.1f) Kill();
    }

    public void ModifyBattery(int amount) {
        currentBattery += amount;
        if (currentBattery > maxBattery) currentBattery = maxBattery;
        if (currentBattery < 0) currentBattery = 0;
        if (hud != null) {
            hud.SetInteger("charge", currentBattery);
            hud.SetTrigger("refresh");
        }
    }

    void UpdateContactNormals() {
        isGrounded = false;
        isTouchingWall = false;
        List<Vector2> toRemove = new List<Vector2>();

        // failsafe cast
        foreach (Vector2 v in failsafeVectors) {

            float rayDistance = v.magnitude / 2 + collisionBias;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, v, rayDistance, LayerMask.GetMask("Environment"));
            if (hit.collider != null) {
                float angle = Vector2.Angle(Vector3.up, hit.normal);
                if (angle > -1 && angle < 1) {
                    isGrounded = true;
                    Debug.DrawRay(hit.point, hit.normal, Color.green);
                }
                else if ((angle < 91 && angle > 89) || (angle < -89 && angle > -91)) {
                    wallNormal = hit.normal;
                    isTouchingWall = true;
                    Debug.DrawRay(hit.point, hit.normal, Color.yellow);
                }
            }
        }

        // continuous contact check
        foreach (Vector2 v in contactVectors) {
            Debug.DrawRay(transform.position, v, Color.red, .2f);
            if (v.magnitude > 2f) toRemove.Add(v);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, v, v.magnitude + collisionBias, LayerMask.GetMask("Environment"));
            if (hit.collider != null) {
                float angle = Vector2.Angle(Vector3.up, hit.normal);
                if (angle > -1 && angle < 1) {
                    isGrounded = true;
                    Debug.DrawRay(hit.point, hit.normal, Color.green);
                }
                else if ((angle < 91 && angle > 89)|| (angle < -89 && angle >-91)) {
                    wallNormal = hit.normal;
                    isTouchingWall = true;
                    Debug.DrawRay(hit.point, hit.normal, Color.yellow);
                }
            }
            else {
                toRemove.Add(v);
            }
        }
        foreach (Vector2 v in toRemove) {
            contactVectors.Remove(v);
        }
    }



    Vector2 Accelerate(Vector2 accelDir, Vector2 currentVel, float accelVel, float maxVel) {
        float projVel = Vector2.Dot(currentVel, accelDir);
        accelVel *= Time.deltaTime;
        if (projVel + accelVel > maxVel) {
            accelVel = maxVel - projVel;
        }
        return currentVel + accelVel * accelDir;
    }
    Vector2 FakeAccelerate(Vector2 accelDir, Vector2 currentVel, float accelVel, float maxVel) {
        float projVel = Vector2.Dot(currentVel, accelDir);
        accelVel *= Time.deltaTime;
        if (projVel < maxVel) {
            return currentVel + accelDir * accelVel;
        }
        else return currentVel;
    }


    Vector2 AccelerateGround() {
        Vector2 inputVectorGround = inputVector;
        
        if (!hasGroundInput)
            inputVectorGround = Vector2.zero;

        float speed = Mathf.Abs(rb.velocity.x);
        if (speed != 0) {
            float slow = speed * friction * Time.deltaTime;
            rb.velocity *= new Vector2(Mathf.Max(speed - slow, 0) / speed, 1);
        }
        return Accelerate(new Vector2(inputVectorGround.x, 0), rb.velocity, groundSpeed, maxVelocity);
    }


    Vector2 AccelerateAir() {
        Vector2 inputVectorAir = inputVector;
        if(!hasInput)
        inputVectorAir = Vector2.zero;

        float speed = Mathf.Abs(rb.velocity.x);
        if (speed != 0) {
            float slow = speed * airResistance * Time.deltaTime;
            rb.velocity *= new Vector2(Mathf.Max(speed - slow, 0) / speed, 1);
        }
        return FakeAccelerate(new Vector2(inputVectorAir.x, 0), rb.velocity, airSpeed, maxVelocity);
    }


    Vector2 AccelerateGrapple() {
        


        return Accelerate(inputVector.normalized, rb.velocity, grappleManoevreSpeed, 999999);
    }


    void Jump() {
        ModifyBattery(-1);
        isGrounded = false;
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        anim.SetTrigger("jump");
    }
    void WallJump() {
        StartDelay();
        rb.velocity = new Vector2(wallNormal.x * jumpSpeed, jumpSpeed);
    }


    void Grapple(Vector2 direction) {
        // raycast for grapple

        grappleRaycastHit = Physics2D.Raycast(transform.position, direction - (Vector2)transform.position, grappleRange, LayerMask.GetMask("Environment"));
        Debug.DrawRay(transform.position, (direction - (Vector2)transform.position).normalized * grappleRange);

        // if grapple hit anything
        if (grappleRaycastHit.collider != null) {
            // set grapple point
            grapplePoint = grappleRaycastHit.point;
            isGrappling = true;
            grappleJoint = gameObject.AddComponent<SpringJoint2D>();
            grappleJoint.autoConfigureConnectedAnchor = false;
            grappleJoint.connectedAnchor = grapplePoint;
            grappleJoint.enableCollision = true;
            grappleJoint.frequency = grappleForce;
            

            grappleDistance = Vector2.Distance(transform.position, grapplePoint);

            grappleJoint.distance = grappleDistance;
            ModifyBattery(-1);
        }
        


    }


    void StopGrapple() {
        isGrappling = false;
        Component.Destroy(grappleJoint);
    }

    void StartDelay() {
        hasInput = false;
        elapsedTime = 0f;
        startTime = Time.time;
    }

    public void ResetPowerups() {
        foreach(PickUp p in pickUps) {
            p.Enable();
        }
    }

}
