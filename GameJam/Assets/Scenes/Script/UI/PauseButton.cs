using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    Animator anim;

    Button OtherButton;
    Button BackButton;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.parent.gameObject.GetComponent<Animator>();

        OtherButton = transform.parent.GetChild(0).GetComponent<Button>();
        BackButton = transform.parent.GetChild(2).GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
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
        OtherButton.Select();
    }

}
