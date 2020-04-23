using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D stageTrigger) {
        if(stageTrigger.gameObject.CompareTag("Player")){
            Debug.Log("STAGE ONE");
        }
    }
}
