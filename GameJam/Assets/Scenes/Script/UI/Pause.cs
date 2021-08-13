using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{

    public GameObject UI;

    Animator anim;

    bool pause;

    // Start is called before the first frame update
    void Start()
    {
        anim = UI.GetComponent<Animator>();
        pause = false;
        UI.SetActive(pause);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
            anim.SetBool("isInfomation", false);
        }

        if (pause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        UI.SetActive(pause);
        anim.SetBool("isPause", pause);
    }

}
