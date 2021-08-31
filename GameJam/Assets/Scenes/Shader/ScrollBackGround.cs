using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackGround : MonoBehaviour
{
    //�J�����̎擾�p
    public GameObject Cam;

    //CinemaScene�J�����̎擾�p
    GameObject CinemaCam;

    //�`�F���W�J�����N���X�擾�p
    ChangeCamera changeCamera;

    [Header("�X�N���[�����x")]
    [SerializeField]float ScrollSpeed;

    [Header("�X�N���[��������摜�̐�")]
    [SerializeField]int BackGroundCount;

    //�w�i�̔z��
    Renderer[] BackGround;

    //�X�N���[���X�s�[�h�Ɋ|�����킹��l
    float Time;

    //�w�i�̓����x��ς��鎞�Ɏg���l
    float Alpha;

    [Header("�{�X�킪���邩�ǂ���")]
    [SerializeField]bool bChange = false;

    [Header("�������X�s�[�h")]
    [SerializeField]float Minus_Alpha = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        //CinemaScene�J����������
        CinemaCam = Cam.transform.GetChild(0).gameObject;

        //ChangeCamera�N���X���擾
        changeCamera = GameObject.Find("Main Camera").GetComponent<ChangeCamera>();

        //�w�i�̏�����
        BackGround = new Renderer[BackGroundCount];
        for(int i = 0; i< BackGroundCount; i++)
        {
            BackGround[i] = this.transform.GetChild(i).gameObject.GetComponent<Renderer>();
            BackGround[i].material.SetFloat("_XSpeed", ScrollSpeed);
        }

        //�X�N���[���X�s�[�h�Ɋ|�����킹��l�̏�����
        Time = 0f;

        //�����x�̏�����
        Alpha = 1.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�G�ǐՎ��Ɏ擾����CinemaScene�J������ύX����
        if (changeCamera.IfCameraFlag(ChangeCamera.CAMERAFLAG.DOLLY))
            CinemaCam = Cam.transform.GetChild(1).gameObject;
        else
            CinemaCam = Cam.transform.GetChild(0).gameObject;

        //CinemaScene�J�����������Ă邩����
        if (0.05f < Mathf.Abs(CinemaCam.transform.localPosition.x))
        {
            if (0 < CinemaCam.transform.localPosition.x)
                Time++;
            else if (CinemaCam.transform.localPosition.x < 0)
                Time--;
        }

        //�w�i�̃V�F�[�_�[�̈ړ��l��ύX����
        for (int i = 0; i < BackGroundCount; i++)
            BackGround[i].material.SetFloat("_XSpeed", ScrollSpeed * Time);

        //�G�ޏo���ɓ����x�����ɖ߂�
        if (changeCamera.IfCameraFlag(ChangeCamera.CAMERAFLAG.MAIN) && Alpha < 1)
            Alpha += 0.01f;
        //�G�o�����ɓ����x��ύX����
        else if (!changeCamera.IfCameraFlag(ChangeCamera.CAMERAFLAG.MAIN) && 0 < Alpha)
            Alpha -= 0.1f;

        //�G���o������X�e�[�W�ł���΃V�F�[�_�[�ɓ����x��K������
        if (bChange)
            BackGround[1].material.SetFloat("_Alpha",Alpha);
    }
}
