using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class End_Branch : MonoBehaviour
{
    Image image;

    [Header("�G���f�B���O����̃X�v���C�g")]
    public Sprite Good_sprite;
    public Sprite Normal_sprite;
    public Sprite Bad_sprite;

    //�f�X�J�E���g���Q�b�g����ϐ�
    int Get_Count;
    //�G���f�B���O���S�������I������t���O
    bool Text_Flag;
    //���݂̎��ԂƐ�������
    float Now_time;
    [Header("�e�L�X�g��\�����I����Ă���V�[�����ړ�����܂ł̎���")]
    public float Limit_time;

    //�C���[�W�𕪊򂷂�
    void Image_Branch()
    {
        //�C���[�W���Z�b�g����
        if (Get_Count == 0)
        {
            image.sprite = Good_sprite;
        }
        else if (Get_Count > 0 && Get_Count < 7)
        {
            image.sprite = Normal_sprite;
        }
        else
        {
            image.sprite = Bad_sprite;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
        //�f�X�J�E���g�̒l����肷��
        Get_Count = Ending_Manager.GetDead_Count();
        //�C���[�W���������
        Image_Branch();
        //�e�L�X�g�t���O��|��
        Text_Flag = false;
        //���Ԃ̏������錾������
        Now_time = 0.0f;
    }

    

    // Update is called once per frame
    void Update()
    {   
        if(Text_Flag == false)
        {
            //������\�����邽�߂̏���

        }
        else
        {
            //���O�̃t���[������o�߂������Ԃ𑝂₷
            Now_time += Time.deltaTime;

            if(Now_time >= Limit_time)
            {
                //�^�C�g���Ɉړ�����
                Fade.FadeOut("Title");
            }
        }

    }
}
