using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingPlatform : MonoBehaviour{
    // Movement Variables.
    // Transforms to act as start and end markers for journey.
    public Transform pos1, pos2;
    public Transform startPos;
    public bool isPowered = false;
    public float speed = 1.0f;
    Vector3 nextPos;

    // Parenting Variables
    Transform tempTrans;
    Rigidbody2D rb;
    public GameObject object1, object2;
    // object1 is platform, object2 is player.

    private void Start(){
        nextPos = startPos.position;
    }

    void Update(){
        if(isPowered == true){
            Move();
        }
    }

    public void Move(){
        isPowered = true;
        if (transform.position == pos1.position){
            nextPos = pos2.position;
        }
        if (transform.position == pos2.position){
            nextPos = pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    public void Reset(){
        isPowered = false;
        transform.position = pos1.position;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Player")){
            ChangeParent();
            rb = GetComponentInChildren<Rigidbody2D>();
        }
        else{
            RevertParent();
        }
    }
    private void OnCollisionExit2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Player")){
            RevertParent();
        }
    }

    // Make Player a child of MPlatform.
    void ChangeParent(){
        tempTrans = object2.transform.parent;
        object2.transform.parent = object1.transform;   
    }

    // Revert the parent of player.
    void RevertParent(){
        object2.transform.parent = tempTrans;
    }
}


