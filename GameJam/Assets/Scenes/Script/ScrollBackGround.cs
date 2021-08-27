using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackGround : MonoBehaviour
{
    public GameObject Cam;

    GameObject CinemaCam;

    [Header("スクロール速度")]
    [SerializeField]float ScrollSpeed;

    [Header("スクロールさせる画像の数")]
    [SerializeField]int BackGroundCount;

    Renderer[] BackGround;

    float Time;

    // Start is called before the first frame update
    void Start()
    {
        CinemaCam = Cam.transform.GetChild(0).gameObject;

        BackGround = new Renderer[BackGroundCount];

        for(int i = 0; i< BackGroundCount; i++)
        {
            BackGround[i] = this.transform.GetChild(i).gameObject.GetComponent<Renderer>();
            BackGround[i].material.SetFloat("_XSpeed", ScrollSpeed);
        }

        Time = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (0.05f < Mathf.Abs(CinemaCam.transform.localPosition.x))
        {
            if (0< CinemaCam.transform.localPosition.x)
                Time++;
            else if (CinemaCam.transform.localPosition.x < 0)
                Time--;
        }

            for (int i = 0; i < BackGroundCount; i++)
                BackGround[i].material.SetFloat("_XSpeed", ScrollSpeed * Time);
    }
}
