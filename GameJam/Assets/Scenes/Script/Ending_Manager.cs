using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending_Manager : MonoBehaviour
{
    //�v���C���[�̃f�X�J�E���g��ێ�����
    static int Dead_Count;

    // Start is called before the first frame update
    void Start()
    {
        //�V�[���ړ������Ƃ��ɃI�u�W�F�N�g��j�������Ȃ�   
        //DontDestroyOnLoad(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�f�X�J�E���g�̏�����(�X�^�[�g�{�^�����������Ƃ��ɌĂяo��
    public static void Reset()
    {
        Dead_Count = 0;
    }

    //�f�X�J�E���g�𑝂₷(�v���C���[���M�~�b�N�ɐڐG�����ꍇ�ɌĂяo��
    public static void AddDead_Count()
    {
        Dead_Count++;
       
    }

    public static int GetDead_Count()
    {
        return Dead_Count;
    }
}
