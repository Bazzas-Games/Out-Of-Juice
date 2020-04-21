using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject[] interactObjects;
    public bool opensPermanently = false;
    public Button toggleButton;
    private Animator anim;
    public bool isPowered = false;

    void Start() {
        anim = GetComponent<Animator>();
        if(toggleButton != null) {
            toggleButton.isPowered = !isPowered;
        }
        anim.SetBool("isPowered", isPowered);
    }

    public void Press()
    {
        if (!isPowered) {
            isPowered = true;
            foreach (GameObject i in interactObjects) {
                Door d;
                LoopingPlatform p;
                SpeedrunTimer s;
                if (i.TryGetComponent<Door>(out d)) {
                    d.Move();
                }
                if (i.TryGetComponent<LoopingPlatform>(out p)) {
                    p.Move();
                }
                if(i.TryGetComponent<SpeedrunTimer>(out s)){
                    s.isDisplaying = isPowered;
                }
            }
            if (toggleButton != null) toggleButton.ForceReset();
            anim.SetBool("isPowered", isPowered);
        }
    }

    public void Reset() {
        if (!opensPermanently) isPowered = false;
        foreach (GameObject i in interactObjects) {
            Door d;
            LoopingPlatform p;
            SpeedrunTimer s;
            if (i.TryGetComponent<Door>(out d) && !opensPermanently) {
                d.Reset();
            }
            if (i.TryGetComponent<LoopingPlatform>(out p) && !opensPermanently) {
                p.Reset();
            }
            if (i.TryGetComponent<SpeedrunTimer>(out s)) {
                s.isDisplaying = isPowered;
            }
        }
        anim.SetBool("isPowered", isPowered);
    }
    public void ForceReset() {
        isPowered = false;
        foreach (GameObject i in interactObjects) {
            Door d;
            LoopingPlatform p;
            SpeedrunTimer s;
            if (i.TryGetComponent<Door>(out d)) {
                d.Reset();
            }
            if (i.TryGetComponent<LoopingPlatform>(out p)) {
                p.Reset();
            }
            if(i.TryGetComponent<SpeedrunTimer>(out s)) {
                s.isDisplaying = isPowered;
            }
        }
        anim.SetBool("isPowered", isPowered);
    }
}
