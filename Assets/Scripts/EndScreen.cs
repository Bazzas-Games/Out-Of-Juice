using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    void Start() {
        Cursor.visible = true;
        GameObject timer = GameObject.FindGameObjectWithTag("Timer");
        if(timer != null) timer.GetComponent<SpeedrunTimer>().StopTimer();
    }
    public void BackToStart()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
