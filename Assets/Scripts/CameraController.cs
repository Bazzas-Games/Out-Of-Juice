using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float amount = 5f;

    private void FixedUpdate()
    {
        transform.position += new Vector3(amount, 0, 0);
	}
}
