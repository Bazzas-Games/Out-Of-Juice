using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviroHazard : MonoBehaviour

{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            GetComponent<PlayerController>().Kill();
        }
    }

    public void Death()
    {
        Debug.Log("Am dead");
    }
}
