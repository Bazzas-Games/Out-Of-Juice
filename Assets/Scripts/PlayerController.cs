using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isGrounded = false;
    public bool isTouchingWall = false;
    public Vector2 inputVector = Vector2.zero;
    private List<Vector2> contactVectors = new List<Vector2>();

    void Update()
    {
        InputPoll();
    }

    void InputPoll()
    {
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");
        inputVector.Normalize();
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


}
