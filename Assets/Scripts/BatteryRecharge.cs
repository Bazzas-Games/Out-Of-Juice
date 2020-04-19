using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryRecharge : MonoBehaviour
{
    public float boost = 1f;

    void OnTriggerEnter2D(Collider2D otherObj) 
    {
        if (otherObj.gameObject.CompareTag("Player")) 
        {
            //GetComponent<PlayerController>().ModifyBattery();
            Destroy();
        }
    }
    void Destroy()
    {
        Destroy(gameObject,.2f);
    }
}  
