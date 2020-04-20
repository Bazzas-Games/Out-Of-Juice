using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerController player = other.GetComponent<PlayerController>();
            player.checkpoint = this;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, .5f);

    }
}
