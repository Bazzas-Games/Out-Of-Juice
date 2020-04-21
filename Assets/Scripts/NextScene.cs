using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    private SpeedrunTimer timer; 
    void Awake() {
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<SpeedrunTimer>();
    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other) {

        if(other.gameObject.CompareTag("Player")){
            int activeBuildIndex = SceneManager.GetActiveScene().buildIndex;
            if(activeBuildIndex == 0) {
                if (timer.isDisplaying) timer.StartTimer();
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
