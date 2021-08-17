using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    GameObject Menu;

    GameObject Infomation;

    Button RetryButton;

    Button InfomationButton;

    Button BackButton;

    Animator anim;

    bool bPause;

    public bool bRetry;

    // Start is called before the first frame update
    void Start()
    {
        Menu = this.transform.GetChild(0).gameObject;

        Infomation = this.transform.GetChild(1).gameObject;

        RetryButton = Menu.transform.GetChild(0).GetComponent<Button>();

        InfomationButton = Menu.transform.GetChild(1).GetComponent<Button>();

        BackButton = Infomation.transform.GetChild(0).GetComponent<Button>();

        anim = GetComponent<Animator>();
        bPause = false;
        bRetry = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bPause = !bPause;
            anim.SetBool("isInfomation", false);
        }

        if (bPause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        if (bRetry)
        {
            Time.timeScale = 1f;
            anim.SetBool("isRetry", false);
            bRetry = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        anim.SetBool("isPause", bPause);
    }

    public void OnClickInfomationEnter()
    {
        anim.SetBool("isInfomation", true);
        anim.SetBool("isPause", false);

        BackButton.Select();
    }

    public void OnClickInfomationExit()
    {
        anim.SetBool("isInfomation", false);

        InfomationButton.Select();
    }

    public void OnClickRetryEnter()
    {
        anim.SetBool("isRetry", true);
    }
}
