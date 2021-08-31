using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class End_Branch : MonoBehaviour
{
    [Header("image�X�v���C�g�̕\����(�ォ��)")]
    //�ォ�瑝�₷���Ƃ��\
    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;

    [Header("image�X�v���C�g�̐�")]
    public int image_Num;

    [Header("image1�̕���摜")]
    public Sprite Good_sprite;
    public Sprite Normal_sprite;
    public Sprite Bad_sprite;

    [Header("������̓����x��ω�������X�s�[�h0.0f�`1.0f")]
    public float Speed;

    [Header("�e�L�X�g��\�����I����Ă���V�[�����ړ�����܂ł̎���")]
    public float Limit_time;

    //�J���[�`�����l���p�̕ϐ�
    float alfa;     //�����x
    float red;      //��
    float green;    //��
    float blue;     //��
    //���ݕ\�����Ă���image
    int Now_imageNo;
    //�f�X�J�E���g���Q�b�g����ϐ�
    int Get_Count;
    //�G���f�B���O���S�������I������t���O
    bool Text_Flag;
    //���݂̎��ԂƐ�������
    float Now_time;

    //�C���[�W�𕪊򂷂�
    void Image_Branch()
    {
        //�C���[�W���Z�b�g����
        if (Get_Count == 0)
        {
            image1.sprite = Good_sprite;
        }
        else if (Get_Count > 0 && Get_Count < 8)
        {
            image1.sprite = Normal_sprite;
        }
        else
        {
            image1.sprite = Bad_sprite;
        }
    }

    //�ϐ��̏�����
    void Init()
    {
        //�e�L�X�g�t���O��|��
        Text_Flag = false;
        //���Ԃ̏������錾������
        Now_time = 0.0f;
        //�J���[�`�����l���̕ϐ��̏�����
        alfa = 0.0f;
        red = 255.0f;
        green = 255.0f;
        blue = 255.0f;
        //���ݕ\�����Ă���image�ԍ�
        Now_imageNo = 1;
    }

    //image
    void Letter_Display()
    {
        //image�X�v���C�g�̕\��������switch�����𑝒z����
        switch (Now_imageNo)
        {
        case 1:
            image1.GetComponent<Image>().color = new Color(red, green, blue, alfa);
            break;
        case 2:
            image2.GetComponent<Image>().color = new Color(red, green, blue, alfa);
            break;
        case 3:
            image3.GetComponent<Image>().color = new Color(red, green, blue, alfa);
            break;
        case 4:
            image4.GetComponent<Image>().color = new Color(red, green, blue, alfa);
            break;
        default:
            Text_Flag = true;
            break;
        }
        //alfa�̕ϐ��̒l�ɂ���ď�����ύX
        if(alfa >= 1.0f && Text_Flag == false)
        {
            alfa = 0.0f;
            Now_imageNo++;
        }
        else if(Text_Flag == false)
        {
            alfa += Speed;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //�f�X�J�E���g�̒l����肷��
        Get_Count = Ending_Manager.GetDead_Count();
        //�C���[�W���������
        Image_Branch();
        //�ϐ��̏�����
        Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if(Text_Flag == false)
        {
            //������\�����邽�߂̏���
            Letter_Display();
        }
        else
        {
            //���O�̃t���[������o�߂������Ԃ𑝂₷
            Now_time += Time.deltaTime;

            if(Now_time >= Limit_time)
            {
                //�^�C�g���Ɉړ�����
                Fade.FadeOut("TitleScene");
            }
        }

    }
}
