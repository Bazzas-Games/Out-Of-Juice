using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    private List<GameObject> beams = new List<GameObject>();
    public GameObject[] laser;
    public bool isPowered = true;

    // Time when the timer started.
    private float startTime;
    private float elapsedTime;

    public float targetTime = 2f;

    private void Start()
    {

        startTime = Time.time;
        isPowered = true;
        foreach (GameObject l in laser)
        {
            if(l != null)
            beams.Add(l.GetComponentInChildren<EnviroHazard>().gameObject);
        }
    }

    private void Update()
    {
        elapsedTime = (Time.time - startTime);
        if (elapsedTime >= targetTime)
        {
            PowerToggle();
        }
        if (isPowered == true)
        {
           foreach(GameObject b in beams)
            {
                if (b != null) b.SetActive(true);
            }
        }
        else
        {
            foreach(GameObject b in beams)
            {
               if (b != null) b.SetActive(false);
            }
            
        }

    }
    void PowerToggle()
    {
        isPowered = !isPowered;
        startTime = Time.time;
    }
}
