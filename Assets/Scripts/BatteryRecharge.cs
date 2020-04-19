using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryRecharge : MonoBehaviour
{
    public int boost = 1;
    void OnTriggerEnter2D(Collider2D otherObj) 
    {
        if (otherObj.gameObject.CompareTag("Player")) 
        {
            otherObj.GetComponent<PlayerController>().ModifyBattery(boost);
            Destroy();
        }
    }
    void Destroy()
    {
        Destroy(gameObject,.2f);
    }
}  
