using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public Vector2 referenceResolution = new Vector2(512, 288);
    public Camera camera;
    private GameObject player;


    void Start()
    {
        if (camera == null) camera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
