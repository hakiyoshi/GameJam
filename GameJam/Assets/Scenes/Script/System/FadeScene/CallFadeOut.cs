using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallFadeOut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Fade.FadeOut("Test_Kitauchi");
        }
    }
}
