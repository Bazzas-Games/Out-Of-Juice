using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SpeedrunTimer : MonoBehaviour
{
    public float startTime;
    public float elapsedTime;
    public bool isDisplaying = false;
    public bool isTiming = false;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDisplaying) {
            if (isTiming) elapsedTime = Time.time - startTime;
            text.enabled = true;
            text.text = (Mathf.Floor(elapsedTime / 60) + ":" + Math.Round(elapsedTime % 60, 2));

        }
        else {
            text.enabled = false;
        }
    }

    public void StartTimer() {
        startTime = Time.time;
        isTiming = true;
        elapsedTime = 0f;
    }
    public void StopTimer() {
        isTiming = false;
    }
}
