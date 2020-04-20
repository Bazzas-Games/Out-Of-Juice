using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   public GameObject followObject;
   public Vector2 followOffset;
   public float speed = 3;
   private Vector2 threshold;
   private Rigidbody2D rb;

   
   /// Start is called on the frame when a script is enabled just before
      void Start(){
       threshold = calculateThreshold();
       rb = followObject.GetComponent<Rigidbody2D>();
   }
   
   /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
   void FixedUpdate(){
       // define followObjects position in each frame.
       Vector2 follow = followObject.transform.position;
       float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
       float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);

       Vector3 newPosition = transform.position;
       if(Mathf.Abs(xDifference)    >=  threshold.x){
           newPosition.x = follow.x;
       }
       if(Mathf.Abs(yDifference)    >=  threshold.y){
           newPosition.y = follow.y;
       }
       // rb.velocity.magnitude is a float value equal to the highest velocity value.

       float moveSpeed = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed;
       transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
    }

   private Vector3 calculateThreshold(){
       Rect aspect = Camera.main.pixelRect;
       // use Camera's orthographic size to figure out height & width of screen bondary box.
       Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
       // Calculate threshold, subtract boundary box value by followOffset value.
       t.x -= followOffset.x;
       t.y -= followOffset.y;
       return t;
   }
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Vector2 border = calculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x *2, border.y *2, 1));
    }

}
