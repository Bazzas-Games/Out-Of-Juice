using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public Vector2 referenceResolution = new Vector2(512, 288);
    public Vector2 cameraOffset = new Vector2(-16, -9);
    public int pixelsPerUnit = 16;
    public Camera sceneCamera;

    
    [Header("Editor settings")]
    public Vector2 numberOfScreens = new Vector2(20, 20);
    private Vector2 unitsPerScreen = new Vector2(32, 18);

    private GameObject player;


    void Start() {
        if (sceneCamera == null) sceneCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        unitsPerScreen = new Vector2(referenceResolution.x / pixelsPerUnit, referenceResolution.y / pixelsPerUnit);
    }

    void Update() {
        
    }
    void OnDrawGizmos() {
#if UNITY_EDITOR
        Gizmos.color = Color.grey;
        Vector2 max = new Vector2(numberOfScreens.x * unitsPerScreen.x, numberOfScreens.y * unitsPerScreen.y) + cameraOffset;
        for (int i = (int)(-numberOfScreens.x/2); i < numberOfScreens.x/2; i++) {
            Vector3 start = new Vector3(i * unitsPerScreen.x, max.y/2, 0) + (Vector3)cameraOffset;
            Vector3 end = new Vector3(i * unitsPerScreen.x, -max.y/2, 0) + (Vector3)cameraOffset;
            Gizmos.DrawLine(start, end);
        }
        for(int i = (int)(-numberOfScreens.y / 2); i < numberOfScreens.y / 2; i++) {
            Vector3 start = new Vector3(-max.x/2, i*unitsPerScreen.y, 0) + (Vector3)cameraOffset;
            Vector3 end = new Vector3(max.x/2, i*unitsPerScreen.y, 0) + (Vector3)cameraOffset;
            Gizmos.DrawLine(start, end);
        }
#endif
    }
}
