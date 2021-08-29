using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackGround : MonoBehaviour
{
    public GameObject Cam;

    GameObject CinemaCam;

    ChangeCamera cg;

    [Header("�X�N���[�����x")]
    [SerializeField]float ScrollSpeed;

    [Header("�X�N���[��������摜�̐�")]
    [SerializeField]int BackGroundCount;

    Renderer[] BackGround;

    float Time;

    float Alpha;

    [Header("�{�X�킪���邩�ǂ���")]
    [SerializeField]bool bChange = false;

    [Header("�������X�s�[�h")]
    [SerializeField]float Minus_Alpha = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        CinemaCam = Cam.transform.GetChild(0).gameObject;

        cg = GameObject.Find("Main Camera").GetComponent<ChangeCamera>();

        BackGround = new Renderer[BackGroundCount];

        for(int i = 0; i< BackGroundCount; i++)
        {
            BackGround[i] = this.transform.GetChild(i).gameObject.GetComponent<Renderer>();
            BackGround[i].material.SetFloat("_XSpeed", ScrollSpeed);
        }

        Time = 0f;

        Alpha = 1.0f;
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


        if (cg.IfCameraFlag(ChangeCamera.CAMERAFLAG.MAIN) && Alpha < 1)
            Alpha += 0.01f;
        else if (!cg.IfCameraFlag(ChangeCamera.CAMERAFLAG.MAIN) && 0 < Alpha)
            Alpha -= 0.1f;

        if (bChange)
            BackGround[1].material.SetFloat("_Alpha",Alpha);
    }
}
