using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_Check : MonoBehaviour
{
    //�X�e�[�W�̑I������
    public enum Game_Stage
    {
        Stage1,     //�X�e�[�W�P
        Stage2,     //�X�e�[�W�Q
        Stage3      //�X�e�[�W�R
    };

    [Header("�S�[����u���X�e�[�W�I��")]
    public Game_Stage Stage;
    [Header("�S�[�����ē������A�j���[�V�����̃I�u�W�F�N�g")]
    public GameObject friend;
    [Header("�V�[���ړ��̒x��")]
    public float Scene_delay;
    [Header("�T�E���h�̒x��")]
    public float Sound_delay;

    //���̃V�[���̖��O��ۑ�����
    string Next_Stage;
    
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
       anim = friend.GetComponent<Animator>();
    }

    void Update()
    {
    
    }

    //�X�e�[�W(�V�[��)���ړ�����
    void Scene_Move()
    {
        //�t�F�[�h,�V�[���ړ�
        switch (Stage)
        {
        //�X�e�[�W�P�̏ꍇ
        case Game_Stage.Stage1:
            //�X�e�[�W�Q�Ɉړ�(�X�e�[�W���ǉ�
            Next_Stage = "stage2";
            break;
            //�X�e�[�W�Q�̏ꍇ
        case Game_Stage.Stage2:
            //�X�e�[�W�R�Ɉړ�(�X�e�[�W���ǉ�
            Next_Stage = "stage3";
            break;
        //�X�e�[�W�R�̏ꍇ
        case Game_Stage.Stage3:
            //�ړ�(�ړ���̖��O�ǉ��\��
            Next_Stage = "Ending";
            break;
        }
        //�t�F�[�h���Ď��̃X�e�[�W��
        Fade.FadeOut(Next_Stage);
    }

    void Goal_SE()
    {
        //�S�[��SE�Đ�
        AudioManager.PlayAudio("Goal", false, false);
    }
    
    //�S�[���I�u�W�F�N�g�ɐڐG����������
    void OnTriggerEnter2D(Collider2D collider)
    {
        //�ڐG�����I�u�W�F�N�g���v���C���[��������
        if(collider.tag == "Player")
        {
            //�Q�[�W���J����SE�Đ�
            AudioManager.PlayAudio("GateOpen",false,false);
            //�A�j���[�V�������Đ�����(trigger��on�ɂ���
            anim.SetTrigger("Goal");
            Invoke("Goal_SE",Sound_delay);
            //�X�e�[�W�̈ړ�
            Invoke("Scene_Move",Scene_delay);
        }
    }

}
