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

    //���̃V�[���̖��O��ۑ�����
    string Next_Stage;

    // Start is called before the first frame update
    void Start()
    {
       
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
            
            break;
        }
        //�t�F�[�h���Ď��̃X�e�[�W��
        Fade.FadeOut(Next_Stage);
    }
    
    //�S�[���I�u�W�F�N�g�ɐڐG����������
    void OnTriggerEnter2D(Collider2D collider)
    {
        //�ڐG�����I�u�W�F�N�g���v���C���[��������
        if(collider.tag == "Player")
        {
            //�S�[��SE�Đ�(���O�ǉ��\��
            //AudioManager.PlayAudio("",false,false);
            //�X�e�[�W�̈ړ�
            Scene_Move();
        }
    }

}
