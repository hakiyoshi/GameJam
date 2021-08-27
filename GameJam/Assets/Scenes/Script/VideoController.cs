using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoController : MonoBehaviour
{
    public GameObject Video;

    bool bPlay;
    // Start is called before the first frame update
    void Start()
    {
        bPlay = false;

        //Video = GameObject.Find("Video").GetComponent<GameObject>();

        Video.SetActive(bPlay);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V) && !bPlay)
            bPlay = true;
        else if (Input.anyKeyDown && bPlay)
            bPlay = false;

        Video.SetActive(bPlay);
    }
}
