using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.parent.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(anim);  
    }

    public void OnClickInfomationEnter()
    {
        anim.SetBool("isInfomation", true);
        anim.SetBool("isPause", false);
    }

    public void OnClickInfomationExit()
    {
        anim.SetBool("isInfomation", false);
    }

}
