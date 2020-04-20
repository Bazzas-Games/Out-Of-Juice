using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D otherObj) 
    {
        if (otherObj.gameObject.CompareTag("Player")){
            Disable();
            Debug.Log("Ping!");
        }
    }
    public void Enable(){
        gameObject.SetActive(true);
    }
    public void Disable(){
        gameObject.SetActive(false);
    }
}
