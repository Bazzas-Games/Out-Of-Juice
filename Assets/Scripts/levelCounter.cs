using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//public TextMeshProGUI Level = SceneManager.GetActiveScene().buildIndex.ToString("0");

public class levelCounter : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public int buildIndex;
    public GameObject stageCounter;
    public float sec = 12f;

    // Start is called before the first frame update
    private void Start()
    {
        stageCounter.SetActive (true);

        buildIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(SceneManager.GetActiveScene().buildIndex + 1);
        levelText.text = "Stage " + buildIndex.ToString("0");

        //Start the coroutine DeactivationRoutine().
        StartCoroutine(DeactivationRoutine());
    }

    public void levelUpdate()
    {
        Debug.Log("Triggered");
        levelText.text = "Stage " + buildIndex.ToString("0");
        Debug.Log(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator DeactivationRoutine()
    {
        //Wait for sec.
        yield return new WaitForSeconds(sec);
        stageCounter.SetActive (false);
    }

}
